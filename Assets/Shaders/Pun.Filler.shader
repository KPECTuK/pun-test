Shader "Pun/Filler"
{
	Properties
	{
		[PerRendererData] _MainTex("Texture", 2D) = "white" {}
		_FillColor("_FillColor", Color) = (255, 255, 255, 255)
		_BorderColor("_BorderColor", Color) = (0, 0, 0, 0)
		_Progress("_Progress", Range(0.001, 1.0)) = 1.0
		_StencilComp("Stencil Comparison (warn remove)", Float) = 8
		_Stencil("Stencil ID (warn remove)", Float) = 0
		_StencilOp("Stencil Operation (warn remove)", Float) = 0
		_StencilWriteMask("Stencil Write Mask (warn remove)", Float) = 255
		_StencilReadMask("Stencil Read Mask (warn remove)", Float) = 255
		_ColorMask("Color Mask (warn remove)", Float) = 15 
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent+1"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		// filler pass

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _FillColor;
			float _Progress;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 data = tex2D(_MainTex, i.uv);
				float progress = _Progress / _MainTex_ST.x + _MainTex_ST.z;
				fixed4 fill = _FillColor * data.g * ceil(progress - i.uv.x);
				fixed4 color = fixed4(fill.r, fill.g, fill.b, data.a);
				return color;
			}

			ENDCG
		}

		// border pass

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _BorderColor;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 data = tex2D(_MainTex, i.uv);
				return data.r * _BorderColor;
			}

			ENDCG
		}
	}
}
