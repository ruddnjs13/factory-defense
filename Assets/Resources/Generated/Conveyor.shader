Shader "Custom/BeltScrollShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 0.5
        _BeltMin ("Belt UV Min", Vector) = (0.25, 0.0, 0, 0)
        _BeltMax ("Belt UV Max", Vector) = (0.5, 0.25, 0, 0)
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalRenderPipeline"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "Unlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.5
            // #pragma prefer_hlslcc gles  // 빌드 대상에 따라 불필요하여 주석 처리
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MaskTex_ST;
                float _ScrollSpeed;
                float4 _BeltMin;
                float4 _BeltMax;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings o;
                // 기본 변환만 수행 (Unlit)
                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                o.positionHCS = posInputs.positionCS;
                // ST 적용 (필요 시)
                o.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // 기본 컬러 샘플
                float2 uv = i.uv;

                // 마스크 UV도 ST 적용
                float2 maskUV = i.uv * _MaskTex_ST.xy + _MaskTex_ST.zw;

                // 벨트 영역 계산
                float2 beltMin = _BeltMin.xy;
                float2 beltMax = _BeltMax.xy;
                float2 beltSize = beltMax - beltMin;

                // 로컬 UV (영역 내부를 0~1로 정규화)
                float2 localUV = (uv - beltMin) / max(beltSize, float2(1e-5, 1e-5));

                // 시간 기반 스크롤 (Y축 이동)
                // Unity 전역 시간은 광범위 호환을 위해 _Time.y 사용 (초 단위 시간)
                float t = _Time.y * _ScrollSpeed;
                localUV.y = frac(localUV.y + t);

                // 다시 원래 UV 공간으로 복원
                float2 scrolledUV = beltMin + localUV * beltSize;

                // 텍스처 샘플링
                half4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                half4 beltCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrolledUV);
                half mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, maskUV).r;

                // 마스크로 스크롤 결과 블렌드 (Unlit)
                half4 color = lerp(baseCol, beltCol, mask);
                return color;
            }
            ENDHLSL
        }
    }
}
