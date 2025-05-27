using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Note : MonoBehaviour
{
    [Header("Movement Settings")]
    public MoveSpeedControl control;
    public float speed;
    public float startZ = 10f;
    public float endZ = 2f;
    public bool resetsCombo = true;
    public bool isSpecial = false;
    [Header("Particle Settings")]
    public ParticleSystem hitParticles; // Assign in Inspector
    public bool canBeHit = false;
    public bool hasBeenHit = false;
    private bool particlesInitialized = false;
    public Material setMaterial;    // Set material of note
    public Material specialMaterial;   // Set material of special notes
    public new MeshRenderer renderer;
    public NoteSpawner spawner;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        spawner = FindAnyObjectByType<NoteSpawner>();
        isSpecial = spawner.spawnSpecial;
        if (isSpecial) renderer.material = specialMaterial;
        else renderer.material = setMaterial;
        speed = control.moveSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
        InitializeParticles();
    }

    void InitializeParticles()
    {
        if (hitParticles != null && !particlesInitialized)
        {
            var main = hitParticles.main;
            main.playOnAwake = false;
            main.stopAction = ParticleSystemStopAction.Destroy;

            // Stop any particles that might be playing
            hitParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            particlesInitialized = true;
        }
    }

    void Update()
    {
        if (isSpecial && !spawner.isSpecial)
        {
            renderer.material = setMaterial;
            isSpecial = false;
        }

        if (!hasBeenHit)
        {
            // Move the note
            if (transform.position.z > endZ)
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
            else if (!canBeHit) // Only auto-destroy if not hittable
            {
                if (resetsCombo) ComboCounter.Instance.ResetCombo();
                Destroy(gameObject);
            }
        }
    }

    public void SetCanBeHit(bool state) => canBeHit = state;
    public bool CanBeHit() => canBeHit;

    public void DestroyNote()
    {
        if (hasBeenHit) return; // Prevent double hits
        hasBeenHit = true;
        ComboCounter.Instance.IncrementCombo();
        PlayHitEffect();
        // Disable visuals immediately but delay destruction
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, 2f); // Increased time for particles to finish
    }

    public void PlayHitEffect()
    {
        if (hitParticles == null) return;

        // Reset and position the original particle system
        hitParticles.transform.position = transform.position;

        // Make sure it's stopped and cleared first
        hitParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Configure main settings
        var main = hitParticles.main;
        main.playOnAwake = false;
        main.duration = 0.2f; // Short burst duration

        // Set particle color to blue
        //Color customColor = new Color32(0, 100, 255, 255); // Pure blue
        //main.startColor = customColor;

        // Use Rate over Time for reliable particle emission
        var emission = hitParticles.emission;
        emission.enabled = true;
        emission.rateOverTime = 250; // High rate for short duration = burst effect

        // Disable bursts since we're using rate over time
        emission.SetBursts(new ParticleSystem.Burst[0]);

        // Make sure the particle system is active
        hitParticles.gameObject.SetActive(true);

        // Play the particles
        hitParticles.Play();

        // Stop after the short duration
        StartCoroutine(StopParticlesAfterDuration());
    }

    private IEnumerator StopParticlesAfterDuration()
    {
        yield return new WaitForSeconds(0.3f); // Wait a bit longer than duration
        if (hitParticles != null && hitParticles.isPlaying)
        {
            hitParticles.Stop();
        }
    }
}