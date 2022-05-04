using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Steuert das Zeitlimit eines Levels
public class Timer : MonoBehaviour
{
    public EVPlayer externalPlayer;
    public Text timerText;
    public int criticalTime;
    public Color criticalColor;

    void Start()
    {
        timerText.text = externalPlayer.timeLimit.ToString();
        criticalColor.a = 1;
    }

    void Update()
    {
        externalPlayer.timeLimit -= Time.deltaTime;
        timerText.text = Mathf.Round(externalPlayer.timeLimit).ToString();

        if(externalPlayer.timeLimit <= criticalTime)
        {
            timerText.color = criticalColor;
            
        }
    }
}
