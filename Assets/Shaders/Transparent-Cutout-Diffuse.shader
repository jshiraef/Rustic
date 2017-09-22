Shader "Transparent/Cutout/Diffuse" {
 Properties {
     _Color ("Main Color", Color) = (1,1,1,1)
     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
     _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 }
 
	 SubShader{
		 Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
			 ColorMask 0
			 ZWrite off

			 Stencil
		 {
			 Ref 1
			 Comp always
			 Pass replace
		 }

		 /*Pass
		{*/
			Cull Back
			ZTest Less

			CGPROGRAM

			#pragma surface surf Lambert alphatest:_Cutoff

			 //#pragma vertex vert
			 //#pragma fragment frag

				  /*struct appdata
				  {
					  float4 vertex : POSITION;
				  };
				  struct v2f
				  {
					  float4 pos : SV_POSITION;
				  };
				  v2f vert(appdata v)
				  {
					  v2f o;
					  o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					  return o;
				  }
				  half4 frag(v2f i) : COLOR
				  {
					  return half4(1,1,0,1);
				  }*/

			  sampler2D _MainTex;
			  fixed4 _Color;

			  struct Input {
				  float2 uv_MainTex;
							};

	  void surf(Input IN, inout SurfaceOutput o) {
		  fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		  o.Albedo = c.rgb;
		  o.Alpha = c.a;
			}
	  ENDCG
	  }

//	 }
 
 Fallback "Transparent/Cutout/VertexLit"
 }