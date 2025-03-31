using UnityEngine;

[CreateAssetMenu(fileName = "TickRate", menuName = "Scriptable Objects/TickRate")]
public class TickRate : ScriptableObject
{
    public float currentTick = 0;
    public float bpm = 120;
    public int resolution = 192;
    public float waitTime = 0;
}
