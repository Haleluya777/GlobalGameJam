Shader "UI/HoleShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Aspect ("Aspect Ratio", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            // 최대 구멍 개수 정의 (필요에 따라 늘릴 수 있음)
            #define MAX_HOLES 10

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Aspect;

            // 배열로 파라미터 받기
            int _HoleCount;
            float4 _Holes[MAX_HOLES]; // 각 원의 x, y, radius를 담을 배열

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = i.uv;
                pos.x *= _Aspect; // 화면 비율 보정

                // 모든 구멍에 대해 반복
                for (int j = 0; j < _HoleCount; j++)
                {
                    float2 center = _Holes[j].xy;
                    center.x *= _Aspect; // 중심점도 비율 보정
                    float radius = _Holes[j].z;

                    float dist = distance(pos, center);

                    // 어느 한 구멍 안에라도 포함되면 픽셀을 그리지 않음
                    if (dist < radius)
                    {
                        clip(-1);
                    }
                }

                return i.color;
            }
            ENDCG
        }
    }
}
