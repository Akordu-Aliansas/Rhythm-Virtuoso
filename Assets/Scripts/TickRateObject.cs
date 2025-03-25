using System.Collections;
using UnityEngine;

public class TickRateObject : MonoBehaviour
{
    public TickRate tickRate;
    private float timePerTick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tickRate.currentTick = 0;
        timePerTick = 60f / tickRate.bpm * tickRate.resolution;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > tickRate.waitTime) tickRate.currentTick = (Time.time - tickRate.waitTime) * timePerTick;
    }
}
