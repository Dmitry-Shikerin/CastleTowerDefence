#ifndef CUSTOM_VERTEX_TRANSFORM_INCLUDED
#define CUSTOM_VERTEX_TRANSFORM_INCLUDED


float GetEyeDepth(float rawDepth) {
    float persp = LinearEyeDepth(rawDepth);
    #if UNITY_REVERSED_Z
        rawDepth = 1.0 - rawDepth;
    #endif
    float ortho = (_ProjectionParams.z - _ProjectionParams.y) * rawDepth + _ProjectionParams.y;
    return lerp(persp,ortho,unity_OrthoParams.w);
}



float4 ComputeVertexPosition(float4 v) {
    // Add here any custom vertex transform
    float4 pos = UnityObjectToClipPos(v);
    return pos;
}
		
#endif
