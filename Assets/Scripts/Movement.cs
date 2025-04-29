using UnityEngine;

public class Movement : MonoBehaviour
{
    public new Rigidbody rigidbody;
    public MoveSpeedControl control;
    public float deadZone = 0;
    private float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = control.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.position += Vector3.back * Time.deltaTime * moveSpeed;
        if (rigidbody.position.z <= deadZone) Destroy(gameObject);
    }
}
