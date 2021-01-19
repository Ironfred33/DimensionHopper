using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public EVPlayer externalPlayer;
    public Text timerText;
    void Start()
    {
        timerText.text = externalPlayer.timeLimit.ToString();
    }

    void Update()
    {
        externalPlayer.timeLimit -= Time.deltaTime;
        timerText.text = Mathf.Round(externalPlayer.timeLimit).ToString();
    }
}
