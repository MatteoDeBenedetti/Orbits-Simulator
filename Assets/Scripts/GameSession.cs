using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour 
{
    [SerializeField] [Range(1, 100)] float timeWarp = 1;

    UIManager UIManagerRef;

    float[] timeWarpValues = { 0.1f, 1, 2, 10, 50, 100, 500, 1000 };
    float TUFromPeriapsis = 0;
    float timeFromPeriapsis = 0;
    float TU2sec = 806.78f;
    
    void Start () 
	{
        // set up references
        UIManagerRef = FindObjectOfType<UIManager>();

        UIManagerRef.UpdateTimeWarpText(timeWarp.ToString());
    }

    void Update () 
	{
        Time.timeScale = timeWarp;
        //TUFromPeriapsis += Time.deltaTime / TU2sec;
        //timeFromPeriapsis += Time.deltaTime;

        UIManagerRef.UpdateMissionTimeText(Time.timeSinceLevelLoad);
    }

    public float GetTUFromPeriapsis()
    {
        return TUFromPeriapsis;
    }

    public float GetTimeFromPeriapsis()
    {
        return timeFromPeriapsis;
    }

    public float GetTimeWarp()
    {
        return timeWarp;
    }

    public void UpdateTimeWarpSLider(float value)
    {
        timeWarp = timeWarpValues[(int)value];
        UIManagerRef.UpdateTimeWarpText(timeWarp.ToString());
    }
}
