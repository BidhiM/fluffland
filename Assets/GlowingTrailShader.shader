Shader "Custom/GlowingTrail"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Texture for the trail
        _TrailColor ("Trail Color", Color) = (1,1,1,1)  // Main trail color
        _GlowIntensity ("Glow Intensity", Range(1, 10)) = 5  // Brightness multiplier
        _Fade ("Fade Amount", Range(0, 1)) = 0.5  // Controls fade-out effect
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha  // Alpha blending for transparency
        ZWrite Off      // Prevents depth sorting issues
        Cull Off        // Renders both sides
        AlphaToMask On  // Ensures alpha works properly

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float4 _TrailColor;
            float _GlowIntensity;
            float _Fade;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Vertex Function
            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            // Fragment Function
            half4 frag(v2f i) : SV_Target
            {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                // Apply glow effect
                float4 glowColor = texColor * _TrailColor * _GlowIntensity;
                
                // Smooth fade effect based on trail lifetime
                float fadeFactor = saturate(1.0 - i.uv.y) * _Fade;
                glowColor.a *= fadeFactor;

                // Ensure the alpha isn't fully zero to avoid disappearing trails
                glowColor.a = max(glowColor.a, 0.05);

                return glowColor;
            }
            ENDHLSL
        }
    }
}