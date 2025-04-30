using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Movement Settings")]
    public MoveSpeedControl control;
    public float speed;
    public float startZ = 10f;
    public float endZ = 0f;
    public bool resetsCombo = true;

    [Header("Particle Settings")]
    public ParticleSystem hitParticles; // Assign in Inspector

    private bool canBeHit = false;
    private bool hasBeenHit = false;
    private bool particlesInitialized = false;

    void Start()
    {
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
            hitParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particlesInitialized = true;
        }
    }

    void Update()
    {
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
        Destroy(gameObject, 0.5f); // Gives time for particles to play
    }

    public void PlayHitEffect()
    {
        if (hitParticles == null) return;

        // Create a new instance of the particles at hit position
        ParticleSystem spawnedParticles = Instantiate(
            hitParticles,
            transform.position,
            Quaternion.identity
        );

        // Configure and play
        var main = spawnedParticles.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        spawnedParticles.Play();

        // Auto-destroy after playing
        Destroy(spawnedParticles.gameObject, main.duration + main.startLifetime.constantMax);
    }
}