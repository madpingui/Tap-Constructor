Shader "Custom/ShaderBuildings"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
		  _Color2("Color2", Color) = (1,1,1,1)
		  _ColorWindow("ColorWindow", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
      
		[PerRendererData]_ScaleX("ScaleX", Range(0,5)) = 0.0
			[PerRendererData]_ScaleZ("ScaleZ", Range(0,5)) = 0.0
    }
    SubShader
    {
		Tags { "RenderType" = "Opaque"  "Queue" = "Transparent"}
		
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_MainTex2;
			float3 worldNormal;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _ColorWindow;
		half _ScaleX;
		half _ScaleZ;
   

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 c;

			if (IN.worldNormal.x < -0.5 || IN.worldNormal.x > 0.5)
			{
				IN.uv_MainTex.x = IN.uv_MainTex * _ScaleX ;
				
			}
			if (IN.worldNormal.z > 0.5 || IN.worldNormal.z < -0.5)
			{
				IN.uv_MainTex.x = IN.uv_MainTex * _ScaleZ ;
				
			}
			c = tex2D(_MainTex, IN.uv_MainTex);
			
			//IN.uv_MainTex.x = IN.uv_MainTex / _Scale ;
            // Albedo comes from a texture tinted by color
			
           
            o.Albedo = c.g * _Color1 + c.b * _Color2 + c.r * _ColorWindow;
            // Metallic and smoothness come from slider variables

         
            o.Alpha =  (_ColorWindow.a * c.r) + c.b + c.g ;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
