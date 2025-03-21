using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;  // Speed of note movement along Z-axis
    private float startZ = 10f;  // Starting position Z
    private float endZ = 0f;    // End position Z

    void Start()
    {
        // Set the initial position to Z = 10
        transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
    }

    void Update()
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
        if (transform.position.z <= 0)
        {
            Destroy(gameObject); // Destroy the note
        }
    }
}
