Shader "Unlit/TreeShadowTransparent"
{
    Properties
    {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		 _wind_dir ("Wind Direction", Vector) = (0.5,0.05,0.5,0)
        _wind_size ("Wind Wave Size", range(5,50)) = 15

        _tree_sway_stutter_influence("Tree Sway Stutter Influence", range(0,1)) = 0.2
        _tree_sway_stutter ("Tree Sway Stutter", range(0,10)) = 1.5
        _tree_sway_speed ("Tree Sway Speed", range(0,10)) = 1
        _tree_sway_disp ("Tree Sway Displacement", range(0,1)) = 0.3

        _branches_disp ("Branches Displacement", range(0,0.5)) = 0.3

        _leaves_wiggle_disp ("Leaves Wiggle Displacement", float) = 0.07
        _leaves_wiggle_speed ("Leaves Wiggle Speed", float) = 0.01

        _r_influence ("Red Vertex Influence", range(0,1)) = 1
        _b_influence ("Blue Vertex Influence", range(0,1)) = 1

    }


     SubShader
	 {
	 	 CGPROGRAM
		#pragma surface surf Lambert 


            //Declared Variables
            float4 _wind_dir;
            float _wind_size;
            float _tree_sway_speed;
            float _tree_sway_disp;
            float _leaves_wiggle_disp;
            float _leaves_wiggle_speed;
            float _branches_disp;
            float _tree_sway_stutter;
            float _tree_sway_stutter_influence;
            float _r_influence;
            float _b_influence;

            sampler2D _MainTex;
            fixed4 _Tint;

                //Structs
                struct Input {
                    float2 uv_MainTex;
                };

				// Surface Shader
                void surf (Input IN, inout SurfaceOutput o) {
                    
                }

                


        ENDCG
	 }

        
	

    FallBack "Diffuse"
}