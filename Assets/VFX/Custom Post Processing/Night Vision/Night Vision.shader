Shader "Hidden/Custom/Night Vision"
{
    Properties
    {
        _ViewDistance("View Distance", float) = 10
        _NightVisionColorLow("Vision Color Low", color) = (1, 1, 1, 1)
        _NightVisionColorHigh("Vision Color High", color) = (1, 1, 1, 1)
        _GrainAmount("Grain Amount", float) = 1
    }

    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        sampler2D _CameraDepthTexture;
        sampler2D _MainTex;
        float _ViewDistance, _GrainAmount;
        float4 _NightVisionColorLow, _NightVisionColorHigh;

        float HashRange(float2 seed)
        {
            return frac(sin(dot(seed, float2(12.9898, 78.233)))*43758.5453);
        }
        float CalculateLuminosity(float3 incomingColor)
        {
            return (0.299*incomingColor.x + 0.587*incomingColor.y + 0.114*incomingColor.z);
        }
        float4 Frag (VaryingsDefault i) : SV_Target
        {
            float depth = tex2D(_CameraDepthTexture, i.texcoord).r;
            depth = Linear01Depth(depth);
            depth *= _ProjectionParams.z;
            float visibility = 1 - (depth / _ViewDistance);
            float luminosity = CalculateLuminosity(tex2D(_MainTex, i.texcoord));
            float4 colorForVision = lerp(_NightVisionColorLow, _NightVisionColorHigh,HashRange(i.texcoord * _Time.x /20));
            return lerp(float4(_NightVisionColorLow), colorForVision, _GrainAmount) * luminosity * visibility;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            ENDHLSL
        }
    }
}