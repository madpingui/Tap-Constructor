Shader "Custom/BuildingsNormalShader"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Color3("Color3", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

     
        fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;

    

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
            o.Albedo = c.g * _Color1 + c.b * _Color2 + c.r * _Color3;
          
          
        }
        ENDCG
    }
    FallBack "Diffuse"
}
