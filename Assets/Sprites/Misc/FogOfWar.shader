Shader "Unlit/FogOfWar"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1) // Колір туману
        _Center ("Center", Vector) = (0,0,0,0) // Центр поля зору
        _Radius ("Radius", Float) = 30.0 // Радіус видимості
        _Angle ("Angle", Float) = 90.0 // Кут огляду
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 worldPos : TEXCOORD1;
            };

            float4 _Color;
            float4 _Center;
            float _Radius;
            float _Angle;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 dir = normalize(i.worldPos - _Center.xy);
                float dist = distance(i.worldPos, _Center.xy);

                float angle = atan2(dir.y, dir.x) * 57.2958; // Переводимо в градуси
                float halfAngle = _Angle * 0.5;

                bool inAngle = abs(angle) < halfAngle;
                bool inRadius = dist < _Radius;

                if (inAngle && inRadius)
                    return fixed4(1, 1, 1, 0); // Прозорість у зоні видимості

                return _Color; // Темний туман
            }
            ENDCG
        }
    }
}
