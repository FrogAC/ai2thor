Shader "Hidden/ExpPoints"
{
    Properties
    {
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = mul(UNITY_MATRIX_MV, v.normal);
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col;
                half nml = max(max(i.normal.x, i.normal.y), i.normal.z);
                //col = fixed3(nml, nml, nml);
                col = i.normal;
                return fixed4(col, 1.);
            }
            ENDCG
        }
    }
}
