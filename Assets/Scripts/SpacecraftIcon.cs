using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacecraftIcon : MonoBehaviour 
{

    Transform spacraftTransform;

	void Start () 
	{
        spacraftTransform = FindObjectOfType<Spacecraft>().transform;
	}
	
	void Update () 
	{
        transform.position = spacraftTransform.position;
	}
}
