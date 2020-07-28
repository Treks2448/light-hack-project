Shader "Unlit/SimpleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    // Where the shader logic starts
    // Contains vertex and fragment shaders
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            // CG code start (Unity shader code)
            CGPROGRAM

            // Precompiler that defindes what the vertex 
            // and fragment shaders will be called int he program
            #pragma vertex vert
            #pragma fragment frag

            // Used for Unity specific features
            #include "UnityCG.cginc"

            // Mesh data: vertex position, vertex normal, UVs, tangents, vertex colors
            struct VertexInput // This is called appdata by default (but I changed it to VertexInput)
            {
                // (very limited) set of properties that you can get from the mesh 
                float4 vertex : POSITION;
                // normal that points out of the vertex.
                float3 normal : NORMAL; 
                // UV coordinates (that are mapped onto XYZ)
                float2 uv0 : TEXCOORD0; 
                //float4 color : COLOR;
                //float2 uv1 : TEXTCOORD1;
                // there are more...
            };

            // Output of vertex shader that is fed into the fragment shader (usually v2f)
            struct VertexOutput
            {
                // heard something about interpolators... not sure what that means
                float4 clipSpacePos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            // Vertex shader function
            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                // convert local vertex position to clip position and assign to our vertex structure
                o.uv0 = v.uv0;
                o.normal = v.normal;
                o.clipSpacePos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // float - highest precision 
            // half 
            // fixed -1 to 1

            // fragment shader
            fixed4 frag(VertexOutput o) : SV_Target
            {
                float2 uv = o.uv0;
                float3 normal = o.normal;
                return float4(normal,0);
            }
            ENDCG
        }
    }
}
