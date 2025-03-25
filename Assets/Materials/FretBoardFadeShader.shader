Shader "Custom/DistanceFade" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _FadeStart ("Fade Start", Float) = 5
        _FadeEnd ("Fade End", Float) = 2
        _HitZ ("Hit Z Position", Float) = 0
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        
        sampler2D _MainTex;
        fixed4 _Color;
        float _FadeStart;
        float _FadeEnd;
        float _HitZ;
        
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            // Use Z position relative to hit plane
            float zDist = IN.worldPos.z - _HitZ;
            
            // Fade in as approaching (zDist decreasing)
            if(zDist > _FadeStart) {
                o.Alpha = 0;
            }
            else if(zDist > _FadeEnd) {
                o.Alpha = 1 - (zDist - _FadeEnd)/(_FadeStart - _FadeEnd);
            }
            else {
                o.Alpha = 1;
            }
        }
        ENDCG
    }
    FallBack "Transparent"
}