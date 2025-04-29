using UnityEngine;

public class HeldNote : Note
{
    public bool isHeld = false;
    public bool wasReleased = false;
    new MeshRenderer renderer;
    Material material;
    void Start()
    {
        speed = control.moveSpeed;
        renderer = GetComponent<MeshRenderer>();
        resetsCombo = false;
        // Set the initial position to Z = 10
        transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
        endZ = -100000f;
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
