using System.Collections;
using UnityEngine;

public class TickRateObject : MonoBehaviour
{
    public TickRate tickRate;
    private float timePerTick;
    FadeInOut fade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fade = FindAnyObjectByType<FadeInOut>();
        fade.FadeOut();
        timePerTick = 60f / tickRate.bpm * tickRate.resolution;
    }
}
