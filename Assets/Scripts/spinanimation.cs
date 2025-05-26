using UnityEngine;

public class SlowOscillate : MonoBehaviour
{
    [Header("Oscillation Settings")]
    public float maxAngle = 30f; // Maximum rotation angle
    public float oscillationSpeed = 1f; // How fast it oscillates
    public Vector3 rotationAxis = Vector3.up; // Y-axis

    private Vector3 startRotation;

    void Start()
    {
        // Store the initial rotation
        startRotation = transform.eulerAngles;
    }

    void Update()
    {
        // Calculate the oscillating angle using sine wave
        float angle = Mathf.Sin(Time.time * oscillationSpeed) * maxAngle;

        // Apply the rotation
        if (rotationAxis == Vector3.up) // Y-axis
        {
            transform.eulerAngles = new Vector3(
                startRotation.x,
                startRotation.y + angle,
                startRotation.z
            );
        }
        else if (rotationAxis == Vector3.right) // X-axis
        {
            transform.eulerAngles = new Vector3(
                startRotation.x + angle,
                startRotation.y,
                startRotation.z
            );
        }
        else if (rotationAxis == Vector3.forward) // Z-axis
        {
            transform.eulerAngles = new Vector3(
                startRotation.x,
                startRotation.y,
                startRotation.z + angle
            );
        }
    }
}