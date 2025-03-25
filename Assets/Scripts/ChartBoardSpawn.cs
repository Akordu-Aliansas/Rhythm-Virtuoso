using UnityEngine;

public class ChartBoardSpawn : MonoBehaviour
{
    public GameObject ChartBoard;
    public MoveSpeedControl control;
    private float spawnRate;
    private float timer = 0;
    private float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = control.moveSpeed;
        spawnRate = 4 / moveSpeed;
        Instantiate(ChartBoard, transform.position, ChartBoard.transform.rotation);
        Instantiate(ChartBoard, transform.position + Vector3.back * 4, ChartBoard.transform.rotation);
        Instantiate(ChartBoard, transform.position + Vector3.back * 8, ChartBoard.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            Instantiate(ChartBoard, transform.position, ChartBoard.transform.rotation);
            timer = 0;
        }
    }
}
