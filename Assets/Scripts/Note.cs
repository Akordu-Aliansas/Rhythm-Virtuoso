using UnityEngine;

public class Note : MonoBehaviour
{
    public MoveSpeedControl control;
    private float speed;  // Speed of note movement along Z-axis
    private float startZ = 10f;  // Starting position Z
    private float endZ = 0f;    // End position Z
    private bool canBeHit = false;
    private bool hasBeenHit = false; // Track if the note has been hit

    void Start()
    {
        speed = control.moveSpeed;
        // Set the initial position to Z = 10
        transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
    }

    void Update()
    {
        if (!hasBeenHit)  // Only move if it hasn't been hit
        {
            // Move the note along the Z-axis towards 0
            if (transform.position.z > endZ)
            {
                transform.position += Vector3.back * speed * Time.deltaTime; // Move in the negative Z direction
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, endZ);
            }
        }

        // If the note reaches Z=0, but it hasn't been hit, destroy it
        if (transform.position.z <= 0 && !hasBeenHit && !canBeHit)
        {
            Destroy(gameObject); // Destroy the note when it passes Z = 0 without being hit
        }
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, 0), Color.red);
    }

    public void SetCanBeHit(bool state)
    {
        canBeHit = state;
    }

    public bool CanBeHit()
    {
        return canBeHit;
    }

    public void DestroyNote()
    {
        hasBeenHit = true;  // Mark the note as hit
        Destroy(gameObject);  // Destroy the note
    }
}
