using UnityEngine;
using System;

public class StepClock : MonoBehaviour
{
    public static event Action OnStep;

    [Tooltip("Nh?p step toàn game (gi?ng tick)")]
    public float stepInterval = 0.30f;

    void Start()
    {
        StartCoroutine(Tick());
    }

    System.Collections.IEnumerator Tick()
    {
        var wait = new WaitForSeconds(stepInterval);
        while (true)
        {
            yield return wait;
            OnStep?.Invoke();
        }
    }
}
