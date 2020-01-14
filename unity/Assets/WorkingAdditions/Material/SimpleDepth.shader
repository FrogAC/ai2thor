Shader "Hidden/SimpleDepth"
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

                col = tex2D(_DepthMap, fixed2(depth,.5));

                // depth = depth * depth;
                // fixed3 colA = fixed3(0.912,0.713,0.164);
                // fixed3 colB = fixed3(0.000,0.032,1.000);
                // fixed pct = smoothstep(0.3,0.8,depth);
                // col = lerp(colB, colA, pct);
                return fixed4(col,1.0);
            }
            ENDCG
        }
    }
}
