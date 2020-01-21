using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCam : MonoBehaviour {
    public Shader shader;
    public Texture depthGradian;
    void Awake() {
        // GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        // Shader.SetGlobalTexture("_DepthMap", depthGradian);
        GetComponent<Camera>().SetReplacementShader(shader, "");
    }
}
