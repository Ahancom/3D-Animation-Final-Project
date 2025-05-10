Shader "Custom/Blink"
{
    Properties
    {
        _Radius ("视窗半径",    Range(0,1) ) = 1
        _Softness("羽化厚度",    Float      ) = 2 // 默认 0.8，可调到 1.5、2.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings   { float4 positionH : SV_POSITION; float2 uv : TEXCOORD0; };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionH = TransformObjectToHClip(IN.positionOS);
                OUT.uv        = IN.uv;
                return OUT;
            }

            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _Radius;
            float _Softness;

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv   = IN.uv;
                float  dist = distance(uv, float2(0.5,0.5));

                // 对称羽化：从 (radius-softness) → (radius+softness) 之内做平滑过渡
                float mask = smoothstep(_Radius - _Softness,
                                        _Radius + _Softness,
                                        dist);

                float4 sceneCol = SAMPLE_TEXTURE2D(_CameraOpaqueTexture,
                                                   sampler_CameraOpaqueTexture,
                                                   uv);
                // mask=0 → 完全显场景，mask=1 → 完全黑
                return lerp(sceneCol, float4(0,0,0,1), mask);
            }
            ENDHLSL
        }
    }
}
