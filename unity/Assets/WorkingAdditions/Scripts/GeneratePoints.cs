using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoints : MonoBehaviour
{
    public ParticleSystem ps;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCloudData(Texture2D texture, Matrix4x4 invProjection, Matrix4x4 invView) { 
        int w = texture.width;
        int h = texture.height;    
        int interval = 8;
        Color[] imgdata = texture.GetPixels(0,0,w,h);

        Debug.Log(invProjection);
        Debug.Log(invView);
        
        // ParticleSystem.Particle[] particles = new ParticleSystem.Particle[w*h/interval];
        var emitparams = new ParticleSystem.EmitParams();

        for (int i = 0; i < w*h; i+= interval) {
            float x = (float)(i%w)/(float)w;
            float y = (float)(i/w)/(float)h;
            Vector4 vndc = new Vector4(
                (2f*x)-1f,
                (2f*y)-1f,
                2f*imgdata[i].r-1f,
                1f
            );
            Vector4 vcam = invProjection * vndc;
            Vector4 vworld = invView * vcam;
            vworld/=vworld.w;

            emitparams.startLifetime = 15;
            emitparams.startSize = 0.005f;
            emitparams.startColor = Color.red;
            emitparams.position = vworld;
            ps.Emit(emitparams,1);
        }

        // ps.SetParticles(particles, particles.Length);
    }
}
