// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.01 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.01;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.7647059,fgcg:0.8636917,fgcb:1,fgca:1,fgde:0.02,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32717,y:32765,varname:node_1,prsc:2|spec-5-OUT,gloss-6-OUT,normal-7-RGB,emission-21-OUT,alpha-38-OUT;n:type:ShaderForge.SFN_Cubemap,id:3,x:32030,y:32573,ptovrint:False,ptlb:environment,ptin:_environment,varname:node_3,prsc:2,cube:f9382e5e7ba1b87498b521f1e9c4ac0c,pvfc:0|MIP-17-OUT;n:type:ShaderForge.SFN_Slider,id:4,x:31972,y:33466,ptovrint:False,ptlb:transparency,ptin:_transparency,varname:node_4,prsc:2,min:0,cur:0.5384616,max:1;n:type:ShaderForge.SFN_Slider,id:5,x:32049,y:33261,ptovrint:False,ptlb:specular,ptin:_specular,varname:node_5,prsc:2,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:6,x:32049,y:33152,ptovrint:False,ptlb:gloss,ptin:_gloss,varname:node_6,prsc:2,min:0,cur:0.6581197,max:1;n:type:ShaderForge.SFN_Tex2d,id:7,x:31929,y:32899,ptovrint:False,ptlb:normal,ptin:_normal,varname:node_7,prsc:2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:16,x:31382,y:32581,ptovrint:False,ptlb:env blur,ptin:_envblur,varname:node_16,prsc:2,min:0,cur:0.8205128,max:1;n:type:ShaderForge.SFN_RemapRange,id:17,x:31748,y:32597,varname:node_17,prsc:2,frmn:0,frmx:1,tomn:1,tomx:6|IN-16-OUT;n:type:ShaderForge.SFN_Fresnel,id:20,x:32207,y:32445,varname:node_20,prsc:2|EXP-23-OUT;n:type:ShaderForge.SFN_Lerp,id:21,x:32404,y:32642,varname:node_21,prsc:2|A-3-RGB,B-22-OUT,T-30-OUT;n:type:ShaderForge.SFN_Vector1,id:22,x:32139,y:32795,varname:node_22,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:23,x:31786,y:32429,ptovrint:False,ptlb:env fresnel,ptin:_envfresnel,varname:node_23,prsc:2,min:0,cur:0.961876,max:1;n:type:ShaderForge.SFN_OneMinus,id:30,x:32431,y:32468,varname:node_30,prsc:2|IN-20-OUT;n:type:ShaderForge.SFN_Fresnel,id:37,x:32305,y:33533,varname:node_37,prsc:2|EXP-39-OUT;n:type:ShaderForge.SFN_Lerp,id:38,x:32483,y:33326,varname:node_38,prsc:2|A-22-OUT,B-4-OUT,T-37-OUT;n:type:ShaderForge.SFN_Slider,id:39,x:31986,y:33640,ptovrint:False,ptlb:trans fresnel,ptin:_transfresnel,varname:node_39,prsc:2,min:0,cur:1,max:1;proporder:3-16-23-5-6-7-4-39;pass:END;sub:END;*/

Shader "RaginBear/rb_glass_01" {
    Properties {
        _environment ("environment", Cube) = "_Skybox" {}
        _envblur ("env blur", Range(0, 1)) = 0.8205128
        _envfresnel ("env fresnel", Range(0, 1)) = 0.961876
        _specular ("specular", Range(0, 1)) = 1
        _gloss ("gloss", Range(0, 1)) = 0.6581197
        _normal ("normal", 2D) = "bump" {}
        _transparency ("transparency", Range(0, 1)) = 0.5384616
        _transfresnel ("trans fresnel", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform samplerCUBE _environment;
            uniform float _transparency;
            uniform float _specular;
            uniform float _gloss;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform float _envblur;
            uniform float _envfresnel;
            uniform float _transfresnel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _normal_var = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(i.uv0, _normal)));
                float3 normalLocal = _normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_specular,_specular,_specular);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
////// Emissive:
                float node_22 = 0.0;
                float3 emissive = lerp(texCUBElod(_environment,float4(viewReflectDirection,(_envblur*5.0+1.0))).rgb,float3(node_22,node_22,node_22),(1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_envfresnel)));
/// Final Color:
                float3 finalColor = specular + emissive;
                return fixed4(finalColor,lerp(node_22,_transparency,pow(1.0-max(0,dot(normalDirection, viewDirection)),_transfresnel)));
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform samplerCUBE _environment;
            uniform float _transparency;
            uniform float _specular;
            uniform float _gloss;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform float _envblur;
            uniform float _envfresnel;
            uniform float _transfresnel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _normal_var = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(i.uv0, _normal)));
                float3 normalLocal = _normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_specular,_specular,_specular);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/// Final Color:
                float3 finalColor = specular;
                float node_22 = 0.0;
                return fixed4(finalColor * lerp(node_22,_transparency,pow(1.0-max(0,dot(normalDirection, viewDirection)),_transfresnel)),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
