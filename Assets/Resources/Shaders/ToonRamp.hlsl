#ifdef SHADERGRAPH_PREVIEW
ToonRampOutput = float3(0.5,0.5,0);
Direction = float3(0.5,0.5,0);
#else
#if SHADOWS_SCREEN
half4 shadowCoord = ComputeScreenPos(ClipSpacePos);
#else
half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif 
#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
Light light = GetMainLight(shadowCoord);
#else
Light light = GetMainLight();
#endif