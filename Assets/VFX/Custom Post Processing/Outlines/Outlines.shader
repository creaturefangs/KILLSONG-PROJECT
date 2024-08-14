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
        Texture2D _MainTex, _CameraDepthTexture, _CameraNormalsTexture;
        SamplerState sampler_MainTex_point_clamp;
        SamplerState sampler_CameraDepthTexture;
        float4 _CameraDepthTexture_TexelSize;
        float _Threshold, _EdgeWidth;
        float Remap(float4 In, float2 InMinMax, float2 OutMinMax)
        {
            return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        struct Varyings
        {
            	float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
        };

        Varyings Vert(AttributesDefault v)
		{
			Varyings o;
			o.vertex = float4(v.vertex.xy, 0.0, 1.0);
			o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
		    #if UNITY_UV_STARTS_AT_TOP
			    o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		    #endif
			o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);
			return o;
		}

        float4 Frag (Varyings i) : SV_Target
        {
            float4 horizontal = 0;
            float4 vertical = 0;
            float samples[9] = 
            {
                _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(-1, -1)).r, _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(0, -1)).r, _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(1, -1)).r,
                _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(-1, 0)).r, _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(0, 0)).r, _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(1, 0)).r,
                _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(-1, 1)).r, _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize * _EdgeWidth * float2(0, 1)).r, _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.texcoord + _CameraDepthTexture_TexelSize *_EdgeWidth *  float2(1, 1)).r
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

            float edge = sqrt(dot(horizontal, horizontal) + dot(vertical, vertical))* 100;
            return(step(lerp(0, 1, edge), _Threshold));
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
}