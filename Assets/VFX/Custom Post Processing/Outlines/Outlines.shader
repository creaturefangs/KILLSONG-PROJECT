Shader "Hidden/Custom/Outlines"
{
    Properties
    {
        _EdgeColor("Edge Color", color) = (0,0,0,0)
        _DepthThreshold("Depth Threshold", float) = .5
        _NormalThreshold("Normal Threshold", float) = .5
        _EdgeWidth("Edge Width", float) = 2
        _Power("Fresnel Edge Power", float) = 1
        _GrazingAngleMaskPower("Grazing Angle Mask Power", float) = 1
        _GrazingAngleMaskHardness("Grazing Angle Mask Hardness", float) = 1
    }
    HLSLINCLUDE
        float4x4 unity_CameraToWorld;
        float4x4 unity_CameraInvProjection;
        float4x4 unity_MatrixITMV;
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
        Texture2D _MainTex, _CameraDepthNormalsTexture, _CameraDepthTexture;
        SamplerState sampler_MainTex, sampler_CameraDepthNormalsTexture, sampler_CameraDepthTexture;
        float4 _CameraDepthNormalsTexture_TexelSize, _EdgeColor;
        float _DepthThreshold,_NormalThreshold, _EdgeWidth, _Power, _GrazingAngleMaskPower, _GrazingAngleMaskHardness;
        struct DepthAndNormalInfo
        {
            float depth;
            float3 normal;
        };

        float Remap(float4 In, float2 InMinMax, float2 OutMinMax)
        {
            return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        DepthAndNormalInfo DecodeDepthNormal(float4 normalsDepthTextureSample, float4 depthTextureSample)
        {
            DepthAndNormalInfo o;
            o.depth = LinearEyeDepth(depthTextureSample.r);
            o.normal = DecodeViewNormalStereo(normalsDepthTextureSample);
            return o;
        }

        struct Varyings
        {
            	float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
        };
        //Vertex Shader
        Varyings Vert(AttributesDefault v)
		{
			Varyings o;
			o.vertex = float4(v.vertex.xy, 0.0, 1.0);
			o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
		    #if UNITY_UV_STARTS_AT_TOP
			    o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
		    #endif
			o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);
            o.worldPos = mul(unity_CameraInvProjection, v.vertex);
			return o;
		}
        //Fragment Shader
        float4 Frag (Varyings i) : SV_Target
        {

            float3 cameraFacingDirection = normalize(_WorldSpaceCameraPos - i.worldPos);
            float4 col = _MainTex.Sample(sampler_MainTex, i.texcoord);
            float horizontalDepth = 0;
            float verticalDepth = 0;
            float3 horizontalNormal = 0;
            float3 verticalNormal = 0;
            float2 offsets[9] = 
            {
                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(-1, 1),
                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(0, 1),
                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(1, 1),

                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(-1, 0),
                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(0, 0),
                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(1, 0),

                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(-1, -1),
                i.texcoord + _CameraDepthNormalsTexture_TexelSize * _EdgeWidth * float2(0, -1),
                i.texcoord + _CameraDepthNormalsTexture_TexelSize *_EdgeWidth *  float2(1, -1),
            };
            DepthAndNormalInfo depthSamples[9] = 
            {
                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[0]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[0])),
                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[1]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[1])),
                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[2]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[2])),

                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[3]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[3])),
                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[4]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[4])),
                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[5]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[5])),

                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[6]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[6])),
                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[7]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[7])),
                DecodeDepthNormal(_CameraDepthNormalsTexture.Sample(sampler_CameraDepthNormalsTexture, offsets[8]), _CameraDepthTexture.Sample(sampler_CameraDepthTexture, offsets[8]))
            };
            verticalDepth += depthSamples[0].depth * SobelX[0]; // top left (factor +1)
            verticalDepth += depthSamples[2].depth * SobelX[2]; // top right (factor -1)
            verticalDepth += depthSamples[3].depth * SobelX[3]; // center left (factor +2)
            verticalDepth += depthSamples[5].depth * SobelX[5]; // center right (factor -2)
            verticalDepth += depthSamples[6].depth * SobelX[6]; // bottom left (factor +1)
            verticalDepth += depthSamples[8].depth * SobelX[8]; // bottom right (factor -1)

            horizontalDepth += depthSamples[0].depth * SobelY[0]; // top left (factor +1)
            horizontalDepth += depthSamples[1].depth * SobelY[1]; // top center (factor +2)
            horizontalDepth += depthSamples[2].depth * SobelY[2]; // top right (factor +1)
            horizontalDepth += depthSamples[6].depth * SobelY[6]; // bottom left (factor -1)
            horizontalDepth += depthSamples[7].depth * SobelY[7]; // bottom center (factor -2)
            horizontalDepth += depthSamples[8].depth * SobelY[8]; // bottom right (factor -1)

            verticalNormal += depthSamples[0].normal * SobelX[0]; // top left (factor +1)
            verticalNormal += depthSamples[2].normal * SobelX[2]; // top right (factor -1)
            verticalNormal += depthSamples[3].normal * SobelX[3]; // center left (factor +2)
            verticalNormal += depthSamples[5].normal * SobelX[5]; // center right (factor -2)
            verticalNormal += depthSamples[6].normal * SobelX[6]; // bottom left (factor +1)
            verticalNormal += depthSamples[8].normal * SobelX[8]; // bottom right (factor -1)

            horizontalNormal += depthSamples[0].normal * SobelY[0]; // top left (factor +1)
            horizontalNormal += depthSamples[1].normal * SobelY[1]; // top center (factor +2)
            horizontalNormal += depthSamples[2].normal * SobelY[2]; // top right (factor +1)
            horizontalNormal += depthSamples[6].normal * SobelY[6]; // bottom left (factor -1)
            horizontalNormal += depthSamples[7].normal * SobelY[7]; // bottom center (factor -2)
            horizontalNormal += depthSamples[8].normal * SobelY[8]; // bottom right (factor -1)

            float edgeDepth = sqrt(dot(horizontalDepth, horizontalDepth) + dot(verticalDepth, verticalDepth));

            float edgeNormal = sqrt(dot(horizontalNormal, horizontalNormal) + dot(verticalNormal, verticalNormal));

            float fresnel = pow(1 - saturate(dot(normalize(mul(unity_CameraToWorld, depthSamples[4].normal)), normalize(mul((float3x3)unity_CameraToWorld, float3(0,0,1))))), _Power);
            float grazingAngleMask = saturate((fresnel + _GrazingAngleMaskPower - 1) / _GrazingAngleMaskPower);
            float modulatedDepthThreshold = (_DepthThreshold * depthSamples[4].depth) *  (1 + smoothstep(0, 1 - _GrazingAngleMaskHardness, grazingAngleMask));
            float modulatedNormalThreshold = _NormalThreshold * (1 + smoothstep(0, 1 - _GrazingAngleMaskHardness, grazingAngleMask));
            float4 finalColor = lerp(_EdgeColor, col, min(step(edgeNormal, modulatedNormalThreshold), step(edgeDepth, modulatedDepthThreshold))); 
            return finalColor;
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