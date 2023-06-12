using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    public static float time;

    public  float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;

    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    void Update()
    {
        time += timeRate * Time.deltaTime;
        if (time >= 1.0f)
            time = 0.0f;
    }
}
