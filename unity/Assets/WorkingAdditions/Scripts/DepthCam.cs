using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCam : MonoBehaviour {
    public Material mat;

    // Start is called before the first frame update
    void Start() {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

  
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }

}
