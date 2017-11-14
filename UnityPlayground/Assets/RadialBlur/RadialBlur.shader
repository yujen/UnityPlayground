
Shader "Hidden/RadialBlur"
{
	Properties
	{
		_MainTex("Input", 2D) = "white" {}
		_BlurStrengthWidthPosition("BlurStrengthWidthPosition", Vector) = (10, 0.25, 0.5, 0.5)
	}

	SubShader{
	Pass{

	ZTest Always Cull Off ZWrite Off
	Fog{ Mode off }
			
	CGPROGRAM

#pragma vertex vert_img
#pragma fragment frag
#pragma target 2.0
#pragma fragmentoption ARB_precision_hint_fastest
			
#include "UnityCG.cginc"

	// sample positions
	static const fixed samples[10] = { -0.08,-0.05,-0.03,-0.02,-0.01,0.01,0.02,0.03,0.05,0.08 };

	uniform sampler2D _MainTex;
	uniform fixed4 _BlurStrengthWidthPosition;



	fixed4 frag(v2f_img i) : COLOR
	{
		fixed4 color = tex2D(_MainTex, i.uv);

		// vector to the middle of the screen
		fixed2 dir = _BlurStrengthWidthPosition.zw - i.uv;

		// distance to center
		fixed dist = sqrt((dir.x * dir.x) + (dir.y * dir.y));
		
		// normalize direction
		dir = dir / dist;


		// additional samples towards center of screen
		fixed4 sum = color;
		for (int n = 0; n < 10; n++)
		{
			sum += tex2D(_MainTex, i.uv + dir * samples[n] * _BlurStrengthWidthPosition.y);
		}

		// eleven samples
		sum *= (1.0 / 11.0);

		// weighten blur depending on distance to screen center
		fixed t = dist * _BlurStrengthWidthPosition.x;
		t = clamp(t, 0.0, 1.0);

		// blend original with blur
		return lerp(color, sum, t);

	}

	ENDCG
	}
	}
}
