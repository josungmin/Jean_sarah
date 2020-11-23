Shader "Custom/Water" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Strength("Strength", Range(0,2)) = 1.0
		_Speed("Speed", Range(-200, 200)) = 100
		_Opacity("Opacity", Range(0, 1)) = 0.1
	}
		SubShader{
			//렌더링 타입을 투명성이 있는 타입으로 설정
			Tags { "RenderType" = "Transparent" }
			Pass
			{
				LOD 200

				Cull Off
			//깊이 버퍼 연산 X -> 뒤쪽 오브젝트가 있는 그대로 렌더링됨
			ZWrite Off
			//블렌딩 모드를 float4(1,1,1,1)- 알파 로 변경-> 투명 구현
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;

			uniform float4 _Color;
			uniform float _Strength;
			uniform float _Speed;
			uniform float _Opacity;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata i)
			{
				v2f o;

				float4 worldPos = mul(unity_ObjectToWorld, i.vertex);

				float displacement = (cos(worldPos.y) + cos(worldPos.x + (_Speed * _Time.x * 0.5f)));
				worldPos.y = worldPos.y + (displacement * _Strength);

				o.vertex = mul(UNITY_MATRIX_VP, worldPos);
				o.uv = i.uv;

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				//메인 텍스*컬러로 아웃풋 설정
				float4 col = tex2D(_MainTex, i.uv) * _Color;

				//아웃풋.rgb+오퍼시티(알파) 리턴
				return float4(col.rgb , _Opacity);
			}

			ENDCG

		}
		}
			FallBack "Diffuse"
}
