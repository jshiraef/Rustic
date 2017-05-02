Shader "Custom/Stencil/Mask OneZLess Clone"
{

	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

    SubShader
    {
        Tags { "Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True" }
        ColorMask 0
        ZWrite off
        
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One One


        Stencil
        {
            Ref 1
            Comp always
            Pass replace
        }
        
        Pass
		{
			Cull Back
			ZTest Less

			CGPROGRAM
			#pragma vertex vert nofog alpha
			#pragma fragment frag nofog alpha

	struct appdata
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
			}

				 ENDCG
	}
			CGPROGRAM
			#pragma surface surf Lambert vertex:vert nofog alpha
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
            
            
            /*struct appdata
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 pos : SV_POSITION;
            };*/

			sampler2D _MainTex;
			fixed4 _Color;
			sampler2D _AlphaTex;

			struct Input
			{
				float2 uv_MainTex;
				fixed4 color;
			};

			void vert(inout appdata_full v, out Input o)
			{
#if defined(PIXELSNAP_ON)
				v.vertex = UnityPixelSnap(v.vertex);
#endif

				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.color = v.color * _Color;
			}


				fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

#if ETC_EXTERNAL_ALPHA
				color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
            
            ENDCG
 //       }
    } 
}