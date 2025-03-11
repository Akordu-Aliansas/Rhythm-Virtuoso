using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.position += Vector3.back * Time.deltaTime * moveSpeed;
    }
}
