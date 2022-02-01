Shader "Graph/Point Surface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows
		#pragma target 3.0
        
        float _Smoothness;
        
        struct Input
        {
            // you can't name it "worldPosition", coz worldPos is a special name
            // worldPos is a world position of gameObject with material of this shader 
            float3 worldPos;
        };

        void ConfigureSurface(Input input, inout SurfaceOutputStandard surface)
        {
            surface.Albedo.rg = saturate((input.worldPos.xy + 1) * 0.5);
            surface.Smoothness = _Smoothness;
        }
        
        ENDCG
    }
}