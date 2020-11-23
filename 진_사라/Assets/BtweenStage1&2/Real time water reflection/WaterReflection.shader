Shader "Custom/WaterReflection"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_Normal("Normal", 2D) = "bump" {}

		_Height("Height", Range(-1, 1)) = 0
		_DistortionAmount("DistortionAmount", Range(0,1)) = 0.1

		_WaterSpeedX("WaterSpeedX", Range(-5,5)) = 1
		_WaterSpeedY("WaterSpeedY", Range(-5,5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Opaque" = "Transparent"}
    
		GrabPass{}

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf NoLighting //Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 2.0

		sampler2D _GrabTexture;
		sampler2D _Normal;

        struct Input
        {
			fixed4 screenPos;
			fixed2 uv_Normal;
        };

        fixed4 _Color;
		fixed _Height; // 이미지 높이 값
		fixed _DistortionAmount; // 왜곡되는 양

		fixed _WaterSpeedX; // 물이 흐르는 양 X
		fixed _WaterSpeedY; // 물이 흐르는 양 Y

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed3 Normal = (UnpackNormal(tex2D(_Normal, fixed2(IN.uv_Normal.x + _Time.x * _WaterSpeedX, IN.uv_Normal.y + _Time.x * _WaterSpeedY))) *2 + 1.25) * _DistortionAmount;

			fixed3 screenUV = IN.screenPos.xyz / IN.screenPos.w;

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_GrabTexture, fixed2(screenUV.x + Normal.x, 1 - screenUV.y + _Height + Normal.y)) * _Color;
			o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;

			return c;
		}

        ENDCG
    }
    FallBack "Diffuse"
}

//https://www.youtube.com/watch?v=7zR7WBrg0jc