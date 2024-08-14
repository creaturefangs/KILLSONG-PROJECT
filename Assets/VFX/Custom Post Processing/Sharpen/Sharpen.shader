Shader "Hidden/Custom/Outlines"
{
    Properties
    {
        _Threshold("Edge Threshold", float) = .5
        _EdgeWidth("Edge Width", float) = 2
    }

    HLSLINCLUDE
        static const int SobelX[9] = 
        {
            1, 0, -1,
            2, 0, -2,
            1, 0, -1
        };

        static const int SobelY[9] = 
        {
            1, 2, 1,
            0, 0, 0,
            -1, -2, -1
        };


        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl" 
        Texture2D _MainTex;
        sampler2D _CameraDepthTexture;
        SamplerState sampler_MainTex_point_clamp;
        float4 _MainTex_TexelSize;
        float _Threshold, _EdgeWidth, _DepthDistanceModulation;
        float Remap(float4 In, float2 InMinMax, float2 OutMinMax)
        {
            return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        float Sobel(Texture2D textureToSample, SamplerState samplerToUse, float2 uv, float texelSize)
        {
            float4 horizontal = 0;
            float4 vertical = 0;
            float4 samples[9] = 
            {
                textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(-1, -1)), textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(0, -1)), textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(1, -1)),
                textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(-1, 0)), textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(0, 0)), textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(1, 0)),
                textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(-1, 1)), textureToSample.Sample(samplerToUse, uv + texelSize * _EdgeWidth * float2(0, 1)), textureToSample.Sample(samplerToUse, uv + texelSize *_EdgeWidth *  float2(1, 1))
            };
            horizontal += samples[0] * SobelX[0]; // top left (factor +1)
            horizontal += samples[2] * SobelX[2]; // top right (factor -1)
            horizontal += samples[3] * SobelX[3]; // center left (factor +2)
            horizontal += samples[4] * SobelX[4]; // center right (factor -2)
            horizontal += samples[5] * SobelX[5]; // bottom left (factor +1)
            horizontal += samples[7] * SobelX[7]; // bottom right (factor -1)

            vertical += samples[0] * SobelY[0]; // top left (factor +1)
            vertical += samples[1] * SobelY[1]; // top center (factor +2)
            vertical += samples[2] * SobelY[2]; // top right (factor +1)
            vertical += samples[5] * SobelY[5]; // bottom left (factor -1)
            vertical += samples[6] * SobelY[6]; // bottom center (factor -2)
            vertical += samples[7] * SobelY[7]; // bottom right (factor -1)

            return sqrt(dot(horizontal, horizontal) + dot(vertical, vertical));
        }
        float4 Frag (VaryingsDefault i) : SV_Target
        {
            float depth;
            float3 worldNormals;
    
            float edge = Sobel(_MainTex, sampler_MainTex_point_clamp, i.texcoord, _MainTex_TexelSize);
            return(step(lerp(0, 1, edge), _Threshold));
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