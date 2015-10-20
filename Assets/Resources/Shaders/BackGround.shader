Shader "Custom/Base" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_ScreenWidth("width", Int) = 1
		_ScreenHeight("height", Int) = 1
		_DistanceTravelled("distance", Float) = 0
		_Color0("Color0", Color) = (0.6, 0.3,0.9,1)
		_Color1("Color1", Color) = (0.5, 0.7, 0.2, 1)
		_Color2("Color2", Color) = (0.9, 0, 0.2, 1)
		_Color3("Color3", Color) = (0.6, 0.2, 0.8, 1)
		_Color4("Color4", Color) = (0.6, 0.2, 0.8, 1)
}
SubShader {
		Pass{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM

		// Compiler Directives
		#pragma exclude_renderers xbox360 ps3 flash
		#pragma vertex VS_MAIN
		#pragma fragment FS_MAIN

		// Predefined variables and helper functions (Unity specific)
		#include "UnityCG.cginc"

		// Uniform Variables (Properties)
		uniform fixed4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform int _ScreenWidth;
		uniform int _ScreenHeight;
		uniform float _DistanceTravelled; 
		uniform fixed4 _Color0;
		uniform fixed4 _Color1;
		uniform fixed4 _Color2;
		uniform fixed4 _Color3;
		uniform fixed4 _Color4;


	// Input Structs
	struct FS_INPUT {
		float4 pos		: SV_POSITION;
		half2 uv		: TEXCOORD0;
	};

	// VERTEX FUNCTION
	FS_INPUT VS_MAIN(appdata_base input) {
		FS_INPUT output;

		// Setting FS_MAIN input struct
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
		output.uv = TRANSFORM_TEX(input.texcoord, _MainTex); 

		return output;
	}

	// FRAGMENT FUNCTION
	fixed4 FS_MAIN(FS_INPUT input) : COLOR{
		float4 screenPos = input.pos / input.pos.w;
		screenPos.xy = -0.5*(screenPos.xy + 1.0);

		if (screenPos.y < -100) return fixed4(0,0,0,1);
		else if (screenPos.y < 0.4 + 0.1*cos(screenPos.x + _DistanceTravelled*0.5)) return _Color0;
		else if (screenPos.y < 0.6 + 0.1*cos(screenPos.x + _DistanceTravelled*0.5)) return _Color1;
		else if (screenPos.y < 0.8 + 0.1*cos(screenPos.x + _DistanceTravelled*0.5)) return _Color2;
		else if (screenPos.y < 1 + 0.1*cos(screenPos.x + _DistanceTravelled*0.5)) return   _Color3;

		else return _Color4;
		
		//return tex2D(_MainTex, input.uv) * _Color;
	}

		ENDCG

	}
	}
		//FallBack "Diffuse"
}