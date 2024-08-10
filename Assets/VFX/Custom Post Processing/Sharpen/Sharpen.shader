Shader "Hidden/Custom/Sharpen"
{
    Properties
    {
        _SharpnessAmount("Sharpness Amount", float) = 1
    }

    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        float _SharpnessAmount;
        float4 frag (VaryingsDefault i) : SV_Target
        {
            float4 col = saturate(tex2D(_MainTex, i.texcoord));
            float neighboringPixelMultiplier = _SharpnessAmount * -1;
            float centerPixelMultiplier = _SharpnessAmount * 4 + 1;
            float4 upPixel = tex2D(_MainTex, i.texcoord + _MainTex_TexelSize * float2(0, 1));
            float4 downPixel = tex2D(_MainTex, i.texcoord + _MainTex_TexelSize * float2(0, -1));
            float4 leftPixel = tex2D(_MainTex, i.texcoord + _MainTex_TexelSize * float2(0, -1));
            float4 rightPixel = tex2D(_MainTex, i.texcoord + _MainTex_TexelSize * float2(1, 0));
            float4 output = upPixel * neighboringPixelMultiplier + downPixel * neighboringPixelMultiplier + leftPixel * neighboringPixelMultiplier + rightPixel * neighboringPixelMultiplier + col * centerPixelMultiplier;
            return saturate(output);
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