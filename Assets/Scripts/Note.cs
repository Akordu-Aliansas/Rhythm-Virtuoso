using UnityEngine;

public class Note : MonoBehaviour
{
    public MoveSpeedControl control;
    public float speed;  // Speed of note movement along Z-axis
    public float startZ = 10f;  // Starting position Z
    public float endZ = 0f;    // End position Z
    public bool resetsCombo = true;
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
                Destroy(gameObject);
            }
        }

        // If the note reaches Z=0, but it hasn't been hit, destroy it
        if (transform.position.z <= endZ && !hasBeenHit && !canBeHit)
        {
            Destroy(gameObject); // Destroy the note when it passes Z = 0 without being hit
        }
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, 0), Color.red);
        if (transform.position.z <= endZ && !hasBeenHit && !canBeHit)
        {
            if(resetsCombo) ComboCounter.Instance.ResetCombo(); // Add this line
            Destroy(gameObject);
        }
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
        ComboCounter.Instance.IncrementCombo(); // Add this line
        Destroy(gameObject);  // Destroy the note
    }
}
