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
		_Color5("Color5", Color) = (0.6, 0.2, 0.8, 1)

}

CGINCLUDE
///reusable stuff here
float Curve(float x, float distance) {
			x += distance;
			return 0.1*cos(0.2*(x));
		}


		ENDCG

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
		uniform fixed4 _Color5;


	// Input Structs
	struct FS_INPUT {
		float4 pos		: SV_POSITION;
		//half2 uv		: TEXCOORD0;
		float4 screenVert : TEXCOORD1;
	};

	// VERTEX FUNCTION
	FS_INPUT VS_MAIN(appdata_base input) {
		FS_INPUT output;

		float4 clipVert = mul(UNITY_MATRIX_MVP, input.vertex);

		// Setting FS_MAIN input struct
		output.pos = clipVert;
		// output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);

		float4 screenVert = clipVert / clipVert.w; // perspective divide
		screenVert = 0.5 * (screenVert + 1.0); // 

		output.screenVert = screenVert;

		return output;
	}

	// FRAGMENT FUNCTION
	fixed4 FS_MAIN(FS_INPUT input) : COLOR{
		//input.screenVert is the pixel in the range 0-1
		float y = input.screenVert.y;
		float x = input.screenVert.x;
		float fadeWidth = 0.1;
		//if (y < 0.2 + 0 * cos(_DistanceTravelled)) return _Color0;
		// lerp(color1, color2, alpha) where alpha 0..1, 0 is color 1 1 is color2
		if (y < Curve(x, _DistanceTravelled + 0.5)) return float4(0, 0, 0, 1);;

			if (y < 0.2 + Curve(x, _DistanceTravelled + 8)) return _Color0;
			//second if checks if it's in the previous range plus a "margin of blur"
			else if (y < 0.2 + fadeWidth + Curve(x, _DistanceTravelled + 8)) return lerp(_Color0, _Color1, (y - (0.2 + Curve(x, _DistanceTravelled + 8)))/fadeWidth);

			else if (y < 0.4 + Curve(x, _DistanceTravelled))	return _Color1;
			else if (y < 0.4 + Curve(x, _DistanceTravelled) + fadeWidth) return lerp(_Color1, _Color2, (y - (0.4 + Curve(x, _DistanceTravelled))) / fadeWidth);

			else if (y < 0.6 + Curve(x, _DistanceTravelled + 3))	return _Color2;
			else if (y < 0.6 + Curve(x, _DistanceTravelled + 3) + fadeWidth) return lerp(_Color2, _Color3, (y - (0.6 + Curve(x, _DistanceTravelled + 3))) / fadeWidth);

			else if (y < 0.8 + Curve(x, _DistanceTravelled + 1))	return _Color3;
			else if (y < 0.8 + Curve(x, _DistanceTravelled + 1) + fadeWidth) return lerp(_Color3, _Color4, (y - (0.8 + Curve(x, _DistanceTravelled + 1))) / fadeWidth);

			else if (y < 1 + Curve(x, _DistanceTravelled + 7))		return   _Color4;
			else if (y < 1 + Curve(x, _DistanceTravelled + 7) + fadeWidth) return lerp(_Color4, _Color5, (y - (1.0 + Curve(x, _DistanceTravelled + 7))) / fadeWidth);
		
			else return _Color5;
		
			//return tex2D(_MainTex, input.uv) * _Color;
	}
	
		
		ENDCG

	}
	}
		//FallBack "Diffuse"
}