using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    private GlobalTimer globalTimer;
    private Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        globalTimer = GetComponent<GlobalTimer>();
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = globalTimer.currentTime.ToString();
    }
}
