Shader "Custom/AlphaExample"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}   // 텍스처 프로퍼티
        _Color ("Color", Color) = (1,1,1,1)          // 색상 프로퍼티 (RGBA)
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0      // 알파값 프로퍼티 (0~1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" } // 투명 설정
        Blend SrcAlpha OneMinusSrcAlpha  // 알파 블렌딩
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 유니티에서 제공하는 기본 헤더 포함
            #include "UnityCG.cginc"

            // 프로퍼티
            sampler2D _MainTex;
            float4 _Color;
            float _Alpha;

            // 입력 구조체
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // 출력 구조체
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            // 버텍스 셰이더
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // 월드 -> 클립 좌표 변환
                o.uv = v.uv;
                return o;
            }

            // 프래그먼트 셰이더
            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv); // 텍스처 샘플링
                texColor.a = texColor.a * _Alpha;       // 알파값 조정
                return texColor * _Color;
            }
            ENDCG
        }
    }
    Fallback "Transparent/Diffuse"
}
