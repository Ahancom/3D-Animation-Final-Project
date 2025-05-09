Shader "Custom/Blink"
{
    Properties
    {
        _Radius("视窗半径", Range(0,1)) = 1
        _Softness("边缘软化", Range(0,0.5)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            // 不做深度测试、写入，也不剪裁
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };
            struct Varyings
            {
                float4 positionH : SV_POSITION;
                float2 uv        : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionH = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _Radius;
            float _Softness;

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                // 计算到屏幕中心 (0.5,0.5) 的距离
                float dist = distance(uv, float2(0.5, 0.5));
                // smoothstep: 当 dist 从 (radius−softness)→radius 时，mask 从 0→1
                float mask = smoothstep(_Radius - _Softness, _Radius, dist);

                // 采样场景画面
                float4 sceneCol = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv);

                // mask = 0 → 内部显示场景，mask = 1 → 外部黑色
                return lerp(sceneCol, float4(0,0,0,1), mask);
            }
            ENDHLSL
        }
    }
}
