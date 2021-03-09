using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    float _rot = -90.0f;

    // Use this for initialization
    void Start()
    {
        RenderSettings.skybox.SetFloat("_Rotation", _rot);    // 回す
    }

    // Update is called once per frame
    void Update()
    {

    }
}
