Shader "Custom/Stencil/Diffuse NotEqualOne Clone"
{

Properties
{
	[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader
{
	Tags { "Queue" = "Transparent"
	"IgnoreProjector" = "True"
	"RenderType" = "Transparent"
	"PreviewType" = "Plane"
	"CanUseSpriteAtlas" = "True" }
	LOD 200

	Cull Off
	Lighting Off
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

	Stencil
	{
		Ref 1
		Comp notequal
		Pass keep
	}

	


	CGPROGRAM
	#pragma surface surf Lambert vertex:vert nofog alpha
	#pragma multi_compile _ PIXELSNAP_ON
	#pragma shader_feature ETC1_EXTERNAL_ALPHA

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

	void surf (Input IN, inout SurfaceOutput o)
	{
		fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	
	ENDCG
}

Fallback "VertexLit"
}
