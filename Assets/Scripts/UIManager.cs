using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour 
{
    // manual refereces
    [SerializeField] Text timeWarpText;
    [SerializeField] Text missionTypeText;
    [SerializeField] Text apoapsisText;
    [SerializeField] Text periapsisText;


    // cached references
    //GameSession gameSession;

    float timeSec;

    void Start () 
	{
        //gameSession = FindObjectOfType<GameSession>();
	}
	
	void Update () 
	{
		
	}

    public void UpdateTimeWarpText(string timeWarp)
    {
        timeWarpText.text = timeWarp; // gameSession.GetTimeWarp().ToString();
    }

    public void UpdateMissionTimeText(float timeSec)
    {
        missionTypeText.text = "Mission Time: " +
            (timeSec / 60).ToString("#00.") + " min " +
            (timeSec % 60).ToString("#00.") + " sec";
    }

    public void UpdateApoapsisText(float rApo)
    {
        apoapsisText.text = "Apoapsis: " + rApo.ToString() + " km";
    }

    public void UpdatePeriapsisText(float rPeri)
    {
        periapsisText.text = "Periapsis: " + rPeri.ToString() + " km";
    }
}
