using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoints : MonoBehaviour {
    public ParticleSystem ps;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // projection mat: good precision in center, worse off

    // use typical non-linear 
    public void GenerateCloudData(Texture2D texture, Camera cam, bool useLinear = false) {
        int w = texture.width;
        int h = texture.height;
        int interval = 8;
        Color[] imgdata = texture.GetPixels();

        Matrix4x4 viewMat = cam.worldToCameraMatrix;
        Matrix4x4 projMat = cam.projectionMatrix;
        var invVP = (projMat * viewMat).inverse;

        // main problem encountered is camera.projectionMatrix = ??????? worked but further from camera became more inaccurate
        var emitparams = new ParticleSystem.EmitParams();
        var decoder = new Vector4(1.0f, 1f / 255f, 1f / 65025.0f, 1f / 16581375.0f);

        for (int i = 0; i < w * h; i += interval) {
            float x = (float)(i % w) / (float)w;
            float y = (float)(i / w) / (float)h;
            // decode
            float z = Vector4.Dot(imgdata[i], decoder);

            // z = (z*(cam.farClipPlane-cam.nearClipPlane)+cam.nearClipPlane);
            Vector4 vndc = new Vector4(
                (2f * x) - 1f,
                (2f * y) - 1f,
                2.0f * z - 1,
                1f
            );
            Vector4 vworld = invVP * vndc;
            vworld /= vworld.w;

            emitparams.startLifetime = 15;
            emitparams.startSize = 0.005f;
            emitparams.startColor = Color.red;
            emitparams.position = vworld;
            ps.Emit(emitparams, 1);
        }
    }

    // use a camera ray
    public void GenerateCloudData(Camera cam) {
        int w = cam.pixelWidth * 2;
        int h = cam.pixelHeight * 2;
        int interval = 2;

        var invVP = (cam.worldToCameraMatrix * cam.projectionMatrix).inverse;

        // ParticleSystem.Particle[] particles = new ParticleSystem.Particle[w*h/interval];
        var emitparams = new ParticleSystem.EmitParams();

        for (int x = 0; x < w; x += interval) {
            for (int y = 0; y < h; y += interval) {
                // get z and calc depth : https://files.unity3d.com/talks/Siggraph2011_SpecialEffectsWithDepth_WithNotes.pdf 
                // main problem encountered is camera.projectionMatrix = ??????? worked but further from camera became more inaccurate
                Ray ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    emitparams.startLifetime = 15;
                    emitparams.startSize = 0.005f;
                    emitparams.startColor = Color.green;
                    emitparams.position = hit.point;
                    ps.Emit(emitparams, 1);
                }
            }
        }
        // ps.SetParticles(particles, particles.Length);
    }
}
