Shader "Custom/AlphaExample"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}   // �ؽ�ó ������Ƽ
        _Color ("Color", Color) = (1,1,1,1)          // ���� ������Ƽ (RGBA)
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0      // ���İ� ������Ƽ (0~1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" } // ���� ����
        Blend SrcAlpha OneMinusSrcAlpha  // ���� ����
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // ����Ƽ���� �����ϴ� �⺻ ��� ����
            #include "UnityCG.cginc"

            // ������Ƽ
            sampler2D _MainTex;
            float4 _Color;
            float _Alpha;

            // �Է� ����ü
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // ��� ����ü
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            // ���ؽ� ���̴�
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // ���� -> Ŭ�� ��ǥ ��ȯ
                o.uv = v.uv;
                return o;
            }

            // �����׸�Ʈ ���̴�
            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv); // �ؽ�ó ���ø�
                texColor.a = texColor.a * _Alpha;       // ���İ� ����
                return texColor * _Color;
            }
            ENDCG
        }
    }
    Fallback "Transparent/Diffuse"
}
