﻿Shader "Hidden/SimpleDepth"
{
    Properties
    {
        //_DepthMap ("Color Map", 2D) = "" {} 
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float depth : DEPTH;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = -UnityObjectToViewPos(v.vertex).z * _ProjectionParams.w; // 1/farplane
                return o;
            }

            uniform sampler2D _DepthMap;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col;
                fixed depth = i.depth;
                return fixed4(depth,depth,depth,1.0);
            }
            ENDCG
        }
    }
}
