using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeWarpText : MonoBehaviour 
{
    Text timeWarpText;
    GameSession gameSession;

    void Start () 
	{
        timeWarpText = GetComponent<Text>();
        gameSession = FindObjectOfType<GameSession>();
	}
	
	void Update () 
	{
        timeWarpText.text = gameSession.GetTimeWarp().ToString();
    }
}
