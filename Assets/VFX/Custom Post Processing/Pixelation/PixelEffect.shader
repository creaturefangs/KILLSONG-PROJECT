Shader "Hidden/Custom/Pixel Effect"
{
    Properties
    {
        _SampleAmount("Sample Amount", int) = 100
        _DitherSpread("Dither Spread", float) = .1
        _QuantizationAmounts("Quantization Amounts", vector) = (255, 255, 255, 255)
    }

    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        Texture2D _MainTex;
        SamplerState sampler_MainTex_point_clamp;
        int _SampleAmount;
        float4 _QuantizationAmounts;
        float _DitherSpread;

        static const int BayerDitherPattern[2*2] = 
        {
            0,2,
            3,1
        };
        static const int BayerPatternSize = 2;


        float GetBayer2(int x, int y)
        {
            return float(BayerDitherPattern[(x % 2) + (y % 2) *2]) * ((1.0 / 4.0) - 0.5);
        }
        float CalculateQuatization(float currentAmount, float desiredSamples)
        {
            return floor(currentAmount * (desiredSamples - 1) + 0.5) / (desiredSamples - 1);
        }
        float4 frag (VaryingsDefault i) : SV_Target
        {
            float2 pixelRatio = float2(_SampleAmount, _SampleAmount * (_ScreenParams.y / _ScreenParams.x));
            float2 newTexUVs = i.texcoord * pixelRatio;
            newTexUVs = floor(newTexUVs);
            newTexUVs /= pixelRatio;

            float2 BayerDitherCoords = newTexUVs * pixelRatio;
            // sample the texture
            float4 col = _MainTex.Sample(sampler_MainTex_point_clamp, newTexUVs) + ( _DitherSpread * GetBayer2(BayerDitherCoords.x, BayerDitherCoords.y));
            col = float4(CalculateQuatization(col.x, _QuantizationAmounts.x), CalculateQuatization(col.y, _QuantizationAmounts.y), CalculateQuatization(col.z, _QuantizationAmounts.z), CalculateQuatization(col.w, _QuantizationAmounts.w));
            return col;
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