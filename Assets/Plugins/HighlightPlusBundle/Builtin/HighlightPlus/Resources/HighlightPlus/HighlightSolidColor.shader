Shader "HighlightPlus/Geometry/SolidColor" {
Properties {
    _MainTex ("Texture", Any) = "white" {}
    _Color ("Color", Color) = (1,1,1) // not used; dummy property to avoid inspector warning "material has no _Color property"
    _CutOff("CutOff", Float ) = 0.5
    _Cull ("Cull Mode", Int) = 2
	_ZTest("ZTest", Int) = 4
    _ZShift("ZShift", Float) = 0.0001
    _EdgeThreshold("Edge Threshold", Float) = 0.995
    _Padding("Padding", Float) = 0
}
    SubShader
    {
        Tags { "Queue"="Transparent+100" "RenderType"="Transparent" }

        // Compose effect on camera target
        Pass
        {
            ZWrite Off
            Cull [_Cull]
			ZTest [_ZTest]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ HP_ALPHACLIP
            #pragma multi_compile_local _ HP_DEPTHCLIP
            #pragma multi_compile_local _ HP_ALL_EDGES

            #include "UnityCG.cginc"
            #include "CustomVertexTransform.cginc"

            sampler2D _MainTex;
      		float4 _MainTex_ST;
            float _ZShift;
            float _Padding;
            fixed _CutOff;
            fixed4 _Color;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float4 _CameraDepthTexture_TexelSize;;
            float _EdgeThreshold;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
				float4 pos: SV_POSITION;
                float2 uv     : TEXCOORD0;
                float4 scrPos : TEXCOORD1;
                #if HP_DEPTHCLIP
                    float  depth  : TEXCOORD2;
                #endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                v.vertex.xyz *= 1.0 + _Padding;
				o.pos = ComputeVertexPosition(v.vertex);
				o.uv = TRANSFORM_TEX (v.uv, _MainTex);
                o.scrPos = ComputeScreenPos(o.pos);
                #if HP_DEPTHCLIP
                    COMPUTE_EYEDEPTH(o.depth);
                #endif
                #if UNITY_REVERSED_Z
                    o.pos.z += _ZShift;
                #else
                    o.pos.z -= _ZShift;
                #endif
				return o;
            }

	        float3 GetNormal(float depth, float depth1, float depth2, float2 offset1, float2 offset2) {
  		        float3 p1 = float3(offset1, depth1 - depth);
  		        float3 p2 = float3(offset2, depth2 - depth);
  		        float3 norm = cross(p1, p2);
	  	        return normalize(norm);
	        }

            fixed ComputeDepthOutline(float2 uv) {
		        float3 uvInc      = float3(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y, 0);
		        float  depthS     = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv - uvInc.zy));
		        float  depthW     = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv - uvInc.xz));
		        float  depthE     = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv + uvInc.xz));		
		        float  depthN     = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv + uvInc.zy));
                float  depth      = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
			    float3  normalNW  = GetNormal(depth, depthN, depthW, uvInc.zy, float2(-uvInc.x, 0));
   				float3 normalSE   = GetNormal(depth, depthS, depthE, float2(0, -uvInc.y),  uvInc.xz);
				float  dnorm      = dot(normalNW, normalSE);
   				fixed outline = (fixed)(dnorm  < _EdgeThreshold);
                return outline;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

            	#if HP_ALPHACLIP
            	    fixed4 col = tex2D(_MainTex, i.uv);
            	    clip(col.a - _CutOff);
            	#endif
                float2 uv = UnityStereoTransformScreenSpaceTex(i.scrPos.xy / i.scrPos.w);
                #if HP_DEPTHCLIP
                    float sceneZ = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
                    float sceneDepth = GetEyeDepth(sceneZ);
                    clip(sceneDepth - i.depth * 0.999);
                #endif
                #if HP_ALL_EDGES
                    return fixed4(1.0, ComputeDepthOutline(uv), 1.0, 1.0);
                #else
                	return fixed4(1.0, 1.0, 1.0, 1.0);
                #endif
            }
            ENDCG
        }

    }
}