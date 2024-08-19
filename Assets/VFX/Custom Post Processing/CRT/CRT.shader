Shader "Hidden/Custom/CRT"
{
    Properties
    {
        _Curvature("Edge Curvature", float) = 10
        _VingetteWidth("Vingette Width", float) = 5

    }

    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        Texture2D _MainTex;
        SamplerState sampler_MainTex;
        float _Curvature, _VingetteWidth;
        float4 frag (VaryingsDefault i) : SV_Target
        {
            float2 centeredUVs = i.texcoord * 2 - 1;
            float warpAmount = centeredUVs.yx / _Curvature;
            float2 warpedUVs = (centeredUVs + centeredUVs * warpAmount * warpAmount) * .5 + .5;
            float2 vingetteUVs = 1 - abs(warpedUVs * 2 - 1);
            float2 vingetteVector = float2(_VingetteWidth / _ScreenParams.x, _VingetteWidth / _ScreenParams.y);
            float2 finalVingette = saturate(smoothstep(0, vingetteVector, vingetteUVs));

            float4 col = _MainTex.Sample(sampler_MainTex, warpedUVs);
            col.g *= (sin(i.texcoord.y * _ScreenParams.y * 2) + 1) * 0.15 + 1;
            col.rb *= (cos(i.texcoord.y * _ScreenParams.y * 2) + 1) * 0.15 + 1;
            return col * finalVingette.x * finalVingette.y;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            ENDHLSL
        }
    }
}