Shader "UI/2DGlowEffectWithEmission"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0,1)) = 0.5
        _Speed ("Speed", Range(0.1,5)) = 1
        _EmissionTex ("Emission Texture", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionIntensity ("Emission Intensity", Range(0,5)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowIntensity;
            float _Speed;

            sampler2D _EmissionTex;
            float4 _EmissionColor;
            float _EmissionIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                // Main texture with glow effect
                float2 panning = uv + float2(sin(_Time.y * _Speed) * 0.5, cos(_Time.y * _Speed) * 0.5);
                fixed4 tex = tex2D(_MainTex, panning);
                float glow = tex.a * _GlowIntensity;
                fixed4 finalColor = tex + (_GlowColor * glow);

                // Emission channel
                fixed4 emission = tex2D(_EmissionTex, uv) * _EmissionColor * _EmissionIntensity;
                finalColor += emission;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}

