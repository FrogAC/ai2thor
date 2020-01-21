using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScreenshotCamera : MonoBehaviour {
    public Material mat;
    const int RES_WIDTH = 1920;
    const int RES_HEIGHT = 1080;

    Camera camMain;
    // Start is called before the first frame update
    void Start() {
        camMain = GetComponent<Camera>();
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

  
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            Matrix4x4 viewMat = camMain.worldToCameraMatrix;
            Matrix4x4 projMat = camMain.projectionMatrix;
            Debug.Log(viewMat.inverse.ToString());
            Debug.Log(projMat.inverse.ToString());
            StartCoroutine(TakeScreenShot(camMain, "Depth", 0f,0f));
        }
    }

    private IEnumerator TakeScreenShot(Camera cam, string camid, float x, float y) {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(RES_WIDTH, RES_HEIGHT, 24);
        cam.targetTexture = rt;
        Texture2D capture = new Texture2D(RES_WIDTH, RES_HEIGHT, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        capture.ReadPixels(new Rect(x, y, RES_WIDTH, RES_HEIGHT), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        // Encode texture
        byte[] bytes = capture.EncodeToPNG();
        string path = string.Format("{0}/screenshots/Shot{1}_{2}.png",
            Application.persistentDataPath,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
            camid);
        System.IO.File.WriteAllBytes(path, bytes);
    }
}
