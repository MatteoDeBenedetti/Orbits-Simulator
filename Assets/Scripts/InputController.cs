using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour 
{
    Camera mainCamera;

    float scale = 5f;

	void Start () 
	{
        mainCamera = FindObjectOfType<Camera>();
	}
	
	void Update () 
	{
        if ( Input.mouseScrollDelta.y != 0)
        {
            mainCamera.orthographicSize -= Input.mouseScrollDelta.y * scale;
        }
    }
}
