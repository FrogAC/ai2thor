// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SimpleCameraDepth"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
       
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 screenuv : TEXCOORD0;
            };
           
           

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex.xyz);   
                o.screenuv = ComputeScreenPos(o.pos);
                return o;
            }
           
            sampler2D _CameraDepthTexture;
 
            fixed4 frag (v2f i) : SV_Target
            {
                float depth = 1-(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.screenuv.xy));
                // encode depth
                float4 enc = float4(1.0, 255.0, 65025.0, 16581375.0) * depth;
                enc = frac(enc);
                enc -= enc.yzww * float4(1.0/255.0,1.0/255.0,1.0/255.0,0.0);
                return enc;
            }
            ENDCG
        }
    }
}