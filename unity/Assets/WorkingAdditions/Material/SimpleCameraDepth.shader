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
                float4 screenuv : TEXCOORD1;
            };
           
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenuv = ComputeScreenPos(o.pos);
                return o;
            }
           
            sampler2D _CameraDepthTexture;
 
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screenuv.xy / i.screenuv.w;
                float depth = 1.0-(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
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