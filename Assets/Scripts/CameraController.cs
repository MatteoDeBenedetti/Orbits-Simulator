using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform primaryTransform;

    void Start()
    {

    }

    void Update()
    {
        transform.position = new Vector3(
            primaryTransform.position.x,
            primaryTransform.position.y,
            -10);

    }
}
