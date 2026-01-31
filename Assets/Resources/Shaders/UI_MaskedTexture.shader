Shader "UI/MaskedTexture"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _CrackTex ("Crack Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture (A)", 2D) = "white" {}

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            sampler2D _CrackTex;
            sampler2D _MaskTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPosition);

                o.texcoord = v.texcoord;
                
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 금이 간 텍스처에서 색상 값을 가져옵니다.
                fixed4 crackColor = tex2D(_CrackTex, i.texcoord);

                // 눈구멍 마스크 텍스처에서 알파(투명도) 값을 가져옵니다.
                fixed maskAlpha = tex2D(_MaskTex, i.texcoord).a;

                // 최종 색상에 틴트(Tint)와 텍스처 색상을 곱합니다.
                crackColor *= i.color;

                // 금이 간 텍스처의 알파 값을 눈구멍 마스크의 알파 값으로 덮어씁니다.
                crackColor.a = maskAlpha;

                // UI 클리핑 처리
                crackColor.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

                #ifdef UNITY_UI_ALPHACLIP
                clip (crackColor.a - 0.001);
                #endif

                return crackColor;
            }
        ENDCG
        }
    }
}
