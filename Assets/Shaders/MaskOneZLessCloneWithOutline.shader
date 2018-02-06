// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Stencil/MaskOneZLessCloneWithOutline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_AlphaCutoff("Alpha Cutoff", Range(0.01, 1.0)) = 0.1
	}


		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "TransparentCutout"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}
		ColorMask 0
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _MainTex;

	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	v2f vert(appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		return o;
	}

	fixed4 _Color;
	float4 _MainTex_TexelSize;

	fixed4 frag(v2f i) : COLOR
	{
		half4 c = tex2D(_MainTex, i.uv);
		c.rgb *= c.a;
		half4 outlineC = _Color;
		outlineC.a *= ceil(c.a);
		outlineC.rgb *= outlineC.a;

		fixed alpha_up = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
		fixed alpha_down = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
		fixed alpha_right = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
		fixed alpha_left = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;

		return lerp(outlineC, c, ceil(alpha_up * alpha_down * alpha_right * alpha_left));
	}

		ENDCG

		Stencil
	{
		Ref 4
		Comp Always
		Pass Replace
	}
	}
		Pass
	{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	fixed4 _Color;
	fixed _AlphaCutoff;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;


	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
	c.rgb *= c.a;

	// here we discard pixels below our _AlphaCutoff so the stencil buffer only gets written to
	// where there are actual pixels returned. If the occluders are all tight meshes (such as solid rectangles)
	// this is not necessary and a non-transparent shader would be a better fit.
	clip(c.a - _AlphaCutoff);

	return c;
	}
		ENDCG
	}
	}
}