Shader "Replace Color"
{
    Properties
    {
        [NoScaleOffset]Texture2D_7B811119("Texture2D", 2D) = "white" {}
        CellColor("Color", Color) = (0.1058824, 0.2509804, 0.3568628, 0)
        Vector3position("position", Vector) = (0, 0, 0, 0)
        DarkenColor("Darken", Color) = (0, 0, 0, 0.3568628)
        DarkenAmount("DarkenAmount", Float) = 0
        [ToggleUI]ShouldFlash("ShouldFlash", Float) = 0
        FlashColor("FlashColor", Color) = (1, 0, 0.01956749, 0)
        FlashSpeed("FlashSpeed", Float) = 15
        [ToggleUI]DarkCell("DarkCell", Float) = 0
        Color_B1808535("Inactive", Color) = (0.2941177, 0.509804, 0.5921569, 0)
        Metallic("Metallic", Float) = 0
        Smoothness("Smoothness", Float) = 0
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
    #define SHADERGRAPH_PREVIEW 1

    CBUFFER_START(UnityPerMaterial)
    float4 CellColor;
    float3 Vector3position;
    float4 DarkenColor;
    float DarkenAmount;
    float ShouldFlash;
    float4 FlashColor;
    float FlashSpeed;
    float DarkCell;
    float4 Color_B1808535;
    float Metallic;
    float Smoothness;
    CBUFFER_END
    TEXTURE2D(Texture2D_7B811119); SAMPLER(samplerTexture2D_7B811119); float4 Texture2D_7B811119_TexelSize;

    struct SurfaceDescriptionInputs
    {
        float3 TimeParameters;
    };


    void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
    {
        Out = Predicate ? True : False;
    }

    void Unity_Multiply_float(float A, float B, out float Out)
    {
        Out = A * B;
    }

    void Unity_Sine_float(float In, out float Out)
    {
        Out = sin(In);
    }

    void Unity_ReplaceColor_float(float3 In, float3 From, float3 To, float Range, out float3 Out, float Fuzziness)
    {
        float Distance = distance(From, In);
        Out = lerp(To, In, saturate((Distance - Range) / max(Fuzziness, 1e-5f)));
    }

    struct SurfaceDescription
    {
        float3 Out_4;
    };

    SurfaceDescription PopulateSurfaceData(SurfaceDescriptionInputs IN)
    {
        SurfaceDescription surface = (SurfaceDescription)0;
        float _Property_EC528BA7_Out_0 = DarkCell;
        float4 _Property_747C6F7D_Out_0 = Color_B1808535;
        float4 _Property_1B74F66B_Out_0 = CellColor;
        float4 _Branch_1921020F_Out_3;
        Unity_Branch_float4(_Property_EC528BA7_Out_0, _Property_747C6F7D_Out_0, _Property_1B74F66B_Out_0, _Branch_1921020F_Out_3);
        float4 _Property_EEA2FC6B_Out_0 = FlashColor;
        float _Property_148A370A_Out_0 = FlashSpeed;
        float _Multiply_72B1F9EC_Out_2;
        Unity_Multiply_float(_Property_148A370A_Out_0, IN.TimeParameters.x, _Multiply_72B1F9EC_Out_2);
        float _Sine_2A378237_Out_1;
        Unity_Sine_float(_Multiply_72B1F9EC_Out_2, _Sine_2A378237_Out_1);
        float3 _ReplaceColor_C7585EE5_Out_4;
        Unity_ReplaceColor_float((_Branch_1921020F_Out_3.xyz), (_Branch_1921020F_Out_3.xyz), (_Property_EEA2FC6B_Out_0.xyz), _Sine_2A378237_Out_1, _ReplaceColor_C7585EE5_Out_4, 0);
        surface.Out_4 = _ReplaceColor_C7585EE5_Out_4;
        return surface;
    }

    struct GraphVertexInput
    {
        float4 vertex : POSITION;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    GraphVertexInput PopulateVertexData(GraphVertexInput v)
    {
        return v;
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct GraphVertexOutput
            {
                float4 position : POSITION;
        
            };

            GraphVertexOutput vert (GraphVertexInput v)
            {
                v = PopulateVertexData(v);

                GraphVertexOutput o;
                float3 positionWS = TransformObjectToWorld(v.vertex);
                o.position = TransformWorldToHClip(positionWS);
        
                return o;
            }

            float4 frag (GraphVertexOutput IN ) : SV_Target
            {
        
                SurfaceDescriptionInputs surfaceInput = (SurfaceDescriptionInputs)0;
                surfaceInput.TimeParameters = _TimeParameters.xyz;

                SurfaceDescription surf = PopulateSurfaceData(surfaceInput);
                return all(isfinite(surf.Out_4)) ? half4(surf.Out_4.x, surf.Out_4.y, surf.Out_4.z, 1.0) : float4(1.0f, 0.0f, 1.0f, 1.0f);

            }
            ENDHLSL
        }
    }
}
