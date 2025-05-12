using UnityEngine;

public class HeldNote : Note
{
    public bool isHeld = false;
    public bool wasReleased = false;
    Material material;
    void Start()
    {
        resetsCombo = false;
        speed = control.moveSpeed;
        renderer = GetComponent<MeshRenderer>();
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
