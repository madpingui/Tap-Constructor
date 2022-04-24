Shader "Custom/AlphaDistance"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MinAlpha("Alpha min", Float) = 0.25
		_MaxAlpha("Alpha max", Float) = 1.
		_MinDistance("Distance min", Float) = 100.
		_MaxDistance("Distance max", Float) = 200.
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "LightMode" = "ForwardBase"}

			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
			
				 #pragma multi_compile_fwdbase 
    			 #pragma vertex vert
    			 #pragma fragment frag
			
				#include "UnityCG.cginc"
				uniform float4 _LightColor0;

				struct Input {
				  float2 uv_MainTex;
							 };
				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 worldPos : TEXCOORD1;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				v2f vert(appdata_base v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				float _MinAlpha;
				float _MaxAlpha;
				float _MinDistance;
				float _MaxDistance;
				//float4 _Target;

				fixed4 frag(v2f i) : SV_Target 
				{

					fixed4 col = tex2D(_MainTex, i.uv);
					
				//Compute distance
				float dist = distance(i.worldPos.xyz, _WorldSpaceCameraPos.xyz);
				//float dist = distance(i.worldPos, _Target);

				//Clamp distance
				//Compute distance ratio
				dist = saturate((dist - _MinDistance) / (_MaxDistance - _MinDistance));

				//Compute Alpha
				col.a = _MinAlpha + (_MaxAlpha - _MinAlpha) * dist;

				return col;
			}


			ENDCG
		}
		}
	
}