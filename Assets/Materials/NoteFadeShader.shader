Shader "Custom/NoteTransparent" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { 
            "Queue"="Transparent+100" 
            "RenderType"="Transparent" 
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        sampler2D _MainTex;
        float4 _Color;
        
        struct Input { float2 uv_MainTex; };
        void surf (Input IN, inout SurfaceOutputStandard o) {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
}