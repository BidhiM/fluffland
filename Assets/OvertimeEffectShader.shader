Shader "Hidden/OvertimeEffect"
{
    Properties
    {
        _Intensity ("Intensity", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "Overtime Effect"
            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            float _Intensity;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Overlay color (red-orange)
                float3 overtimeColor = float3(1.0, 0.3, 0.0);

                // Pulsating effect using sine wave
                float pulse = 0.5 + 0.5 * sin(_Time.y * 5.0); // Adjust speed with multiplier

                // Final color blend
                return half4(overtimeColor, pulse * _Intensity);
            }
            ENDHLSL
        }
    }
}
