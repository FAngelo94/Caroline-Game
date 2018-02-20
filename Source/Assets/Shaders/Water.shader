Shader "Unlit/Water"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed("Wave Speed", Range(0,100)) = 0.5
        _Amount("Wave Amount", Range(0,100)) = 0.5
        _Height("Wave Height", Range(0,100)) = 0.5
        _Color1("Dark Tint", Color) = (1, 1, 1, .5)
        _Color2("Light Tint", Color) = (1, 1, 1, .5)
        _ColorSpeed("Change Color Speed", Range(0,2)) = 1 

	}
	SubShader
	{
		Tags 
        { 
         "Queue"="Transparent" 
         "IgnoreProjector"="True" 
         "RenderType"="Transparent" 
         "PreviewType"="Plane"
         "CanUseSpriteAtlas"="True"
            
        }
		LOD 100

		Pass
		{
            Cull Off
            Lighting Off
            ZWrite Off
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

            
            struct Vertex
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
                 float2 uv2 : TEXCOORD1;
             };
             
             struct Fragment
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
                 float2 uv2 : TEXCOORD1;
             };
            
            
            float4 _Color1, _Color2;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Speed, _Amount, _Height,_ColorSpeed;
			
			Fragment vert (Vertex v)
			{
				Fragment o;
				
				v.vertex.y += sin( _Time.z * _Speed + (v.vertex.x * _Amount)) * _Height;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);
				o.uv2 = v.uv2;
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			
			float4 frag (Fragment i) : COLOR
			{
                float4 o = float4(0,0,0,1);
                 o.rgb= lerp (_Color1, _Color2,_ColorSpeed* i.uv_MainTex.y);
                 return o; 

			}
			

			ENDCG
		}
	}
}
