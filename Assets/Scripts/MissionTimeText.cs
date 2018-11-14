using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MissionTimeText : MonoBehaviour 
{
    Text missionTypeText;
    GameSession gameSession;

    float timeSec;

    void Start () 
	{
        missionTypeText = GetComponent<Text>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update () 
	{
        timeSec = gameSession.GetTimeFromPeriapsis();
        missionTypeText.text = "Mission Time: " +
            (timeSec / 60).ToString("#00.") + " min " + 
            (timeSec % 60).ToString("#00.") + " sec";

        //Debug.Log(timeSec);
    }
}
