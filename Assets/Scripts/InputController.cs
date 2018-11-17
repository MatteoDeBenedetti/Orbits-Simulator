using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour 
{
    Camera mainCamera;

    float scale = 3f;
    float zoomSmoothfactor = 10f; //0.1f;
    float newCameraSize;

	void Start () 
	{
        mainCamera = FindObjectOfType<Camera>();
        newCameraSize = mainCamera.orthographicSize;

    }
	
	void Update () 
	{
        if (Input.mouseScrollDelta.y != 0)
        {
            newCameraSize =
                mainCamera.orthographicSize - Input.mouseScrollDelta.y * scale;
            newCameraSize = Mathf.Clamp(newCameraSize, 0f, 30f);
        }

        if (Mathf.Abs(newCameraSize - mainCamera.orthographicSize) > 0.1f)
        { 
            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize, 
                newCameraSize, 
                zoomSmoothfactor * Time.unscaledDeltaTime);
        }
    }
}
