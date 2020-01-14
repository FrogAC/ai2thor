﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPointCam : MonoBehaviour
{
    public Shader shader;
    void Awake() { 
        GetComponent<Camera>().SetReplacementShader(shader,"");
    }
}
