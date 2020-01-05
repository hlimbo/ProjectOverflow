using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public int gameTimeDuration; // measured in seconds
    public int currentTime;
    void Start()
    {
        currentTime = gameTimeDuration;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while(currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime = currentTime < 0 ? 0 : currentTime - 1;
        }
    }
}
