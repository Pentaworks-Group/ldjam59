Shader"Hidden/ConnectionLoss"
{
    SubShader
    {
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _StaticIntensity, _StaticSpeed;
            float _ScanlineIntensity, _ScanlineCount;
            float _JitterAmount, _RollSpeed;
            float _VignetteStrength;
            float _Time2;

            float hash(float2 p)
            {
                p = frac(p * float2(443.8975, 397.3973));
                p += dot(p.xy, p.yx + 19.19);
                return frac(p.x * p.y);
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;

                            // Rolling bar / vertical jitter
                float roll = frac(_Time2 * _RollSpeed);
                float barNoise = abs(uv.y - roll);
                float jitter = (hash(float2(_Time2 * 20.0, uv.y * 50.0)) - 0.5)
                                            * _JitterAmount
                                            * smoothstep(0.05, 0.0, barNoise);
                uv.x += jitter;

                half4 col = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                            // Scanlines
                float scan = sin(uv.y * _ScanlineCount * 3.14159) * 0.5 + 0.5;
                col.rgb *= lerp(1.0, scan, _ScanlineIntensity);

                            // Static noise
                float t = floor(_Time2 * _StaticSpeed * 24.0);
                float noise = hash(uv * float2(1920, 1080) + t);
                col.rgb = lerp(col.rgb, half3(noise, noise, noise), _StaticIntensity);

                            // Vignette
                float2 vig = uv * (1.0 - uv.yx);
                float v = pow(vig.x * vig.y * 15.0, _VignetteStrength);
                col.rgb *= saturate(v);

                return col;
            }
            ENDHLSL
        }
    }
}