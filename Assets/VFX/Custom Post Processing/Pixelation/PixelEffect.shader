Shader "Hidden/Custom/Pixel Effect"
{
    Properties
    {
        _SampleAmount("Sample Amount", int) = 100
        _DitherSpread("Dither Spread", float) = .1
        _QuantizationAmounts("Quantization Amounts", int) = 255
    }

    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        Texture2D _MainTex;
        SamplerState sampler_MainTex_point_clamp;
        int _SampleAmount, _QuantizationAmounts;
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

        float4 frag (VaryingsDefault i) : SV_Target
        {
            float2 pixelRatio = float2(_SampleAmount, _SampleAmount * (_ScreenParams.y / _ScreenParams.x));
            float2 newTexUVs = i.texcoord * pixelRatio;
            newTexUVs = floor(newTexUVs);
            newTexUVs /= pixelRatio;

            float2 BayerDitherCoords = newTexUVs * pixelRatio;
            // sample the texture
            float4 col = _MainTex.Sample(sampler_MainTex_point_clamp, newTexUVs) + ( _DitherSpread * GetBayer2(BayerDitherCoords.x, BayerDitherCoords.y));
            col = floor(col * (_QuantizationAmounts - 1) + 0.5) / (_QuantizationAmounts - 1);
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