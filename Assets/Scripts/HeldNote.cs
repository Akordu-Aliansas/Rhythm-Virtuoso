using UnityEngine;

public class HeldNote : Note
{
    public bool isHeld = false;
    public bool wasReleased = false;
    Material material;
    public GameObject note;
    void Start()
    {
        resetsCombo = false;
        speed = control.moveSpeed;
        renderer = GetComponent<MeshRenderer>();
        spawner = FindAnyObjectByType<NoteSpawner>();
        isSpecial = spawner.spawnSpecial;
        if (isSpecial) renderer.material = specialMaterial;
        else renderer.material = setMaterial;
        speed = control.moveSpeed;
        transform.position = note.transform.position;
        // Set the initial position to Z = 10
        //transform.position = new Vector3(transform.position.x, transform.position.y, 10);
        endZ = -100000f;
    }
    private void Update()
    {
        if (isSpecial && !spawner.isSpecial)
        {
            renderer.material = setMaterial;
            isSpecial = false;
        }
        if(note == null || note.GetComponent<Renderer>().enabled == false)
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
        else if (!hasBeenHit)
        {
            // Move the note
            if (transform.position.z > endZ)
            {
                transform.position = note.transform.position;
            }
            else if (!canBeHit) // Only auto-destroy if not hittable
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        endZ = transform.position.z - 4;
    }
    public void Release()
    {
        isHeld = false;
        wasReleased = true;
        renderer.material.color = Color.gray;
    }
}
