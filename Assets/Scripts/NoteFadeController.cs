using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class NoteFadeController : MonoBehaviour
{
    [Header("Fade In Settings")]
    public float fadeStartZ = 10f;  // Z position where fade begins
    public float fadeEndZ = 7f;    // Z position where note becomes fully visible

    private Material _material;
    private float _initialZ;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
        _initialZ = transform.position.z; // Store starting Z position

        // Initialize with correct alpha
        UpdateFade();
    }

    void Update()
    {
        UpdateFade();
    }

    void UpdateFade()
    {
        float currentZ = transform.position.z;

        // Calculate progress through fade zone (0 to 1)
        float fadeProgress = Mathf.InverseLerp(fadeStartZ, fadeEndZ, currentZ);

        // Clamp and apply alpha
        float alpha = Mathf.Clamp01(fadeProgress);

        Color color = _material.color;
        color.a = alpha;
        _material.color = color;
    }
}