// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D
// Upgrade NOTE: replaced tex2D unity_LightmapInd with UNITY_SAMPLE_TEX2D_SAMPLER

// Shader created with Shader Forge v1.01 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.01;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:2,uamb:True,mssp:True,lmpd:True,lprd:True,rprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:False,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:33630,y:32520,varname:node_1,prsc:2|diff-603-OUT,spec-517-OUT,gloss-130-OUT,normal-148-OUT,emission-754-OUT,amdfl-657-OUT,difocc-38-RGB,clip-2-A;n:type:ShaderForge.SFN_Tex2d,id:2,x:30414,y:31578,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_591,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:4,x:30819,y:31504,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_592,prsc:2,glob:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5,x:31229,y:31586,varname:node_5,prsc:2|A-4-RGB,B-39-OUT;n:type:ShaderForge.SFN_Tex2d,id:9,x:30498,y:32259,ptovrint:False,ptlb:specular(rgb)gloss(a),ptin:_specularrgbglossa,varname:node_594,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:31,x:32298,y:31993,ptovrint:False,ptlb:BumpMap,ptin:_BumpMap,varname:node_595,prsc:2,ntxv:3,isnm:True|UVIN-569-OUT;n:type:ShaderForge.SFN_Tex2d,id:33,x:29879,y:32897,ptovrint:False,ptlb:Illum,ptin:_Illum,varname:node_596,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:37,x:30544,y:32090,ptovrint:False,ptlb:ao power,ptin:_aopower,varname:node_597,prsc:2,min:0,cur:0.4796831,max:1;n:type:ShaderForge.SFN_Tex2d,id:38,x:30532,y:31812,ptovrint:False,ptlb:ao (rgb) colormask (a),ptin:_aorgbcolormaska,varname:node_598,prsc:2,tex:d9a5a1001e9272342a3d8e57cda719c5,ntxv:0,isnm:False|UVIN-548-OUT;n:type:ShaderForge.SFN_Multiply,id:39,x:31295,y:31900,varname:node_39,prsc:2|A-2-RGB,B-40-OUT;n:type:ShaderForge.SFN_Lerp,id:40,x:31007,y:31994,varname:node_40,prsc:2|A-192-OUT,B-38-RGB,T-37-OUT;n:type:ShaderForge.SFN_Vector3,id:70,x:31255,y:32692,varname:node_70,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Color,id:113,x:30027,y:33109,ptovrint:False,ptlb:emission color,ptin:_emissioncolor,varname:node_602,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:114,x:30213,y:32969,varname:node_114,prsc:2|A-33-RGB,B-113-RGB;n:type:ShaderForge.SFN_Slider,id:127,x:31013,y:32422,ptovrint:False,ptlb:specular power,ptin:_specularpower,varname:node_604,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:128,x:30423,y:32637,ptovrint:False,ptlb:Shininess,ptin:_Shininess,varname:node_605,prsc:2,min:1,cur:35.00855,max:2048;n:type:ShaderForge.SFN_Lerp,id:129,x:31170,y:32245,varname:node_129,prsc:2|A-70-OUT,B-9-RGB,T-127-OUT;n:type:ShaderForge.SFN_Lerp,id:130,x:30838,y:32590,varname:node_130,prsc:2|A-132-OUT,B-9-A,T-128-OUT;n:type:ShaderForge.SFN_Vector1,id:132,x:30614,y:32473,varname:node_132,prsc:2,v1:0;n:type:ShaderForge.SFN_Color,id:139,x:31361,y:32317,ptovrint:False,ptlb:SpecColor,ptin:_SpecColor,varname:node_609,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:140,x:31534,y:32258,varname:node_140,prsc:2|A-129-OUT,B-139-RGB;n:type:ShaderForge.SFN_Vector3,id:147,x:32298,y:31879,varname:node_147,prsc:2,v1:0.4980392,v2:0.4980392,v3:1;n:type:ShaderForge.SFN_Lerp,id:148,x:32492,y:31972,varname:node_148,prsc:2|A-147-OUT,B-31-RGB,T-149-OUT;n:type:ShaderForge.SFN_Slider,id:149,x:32141,y:32177,ptovrint:False,ptlb:normal intensity,ptin:_normalintensity,varname:node_613,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Lerp,id:151,x:31107,y:32882,varname:node_151,prsc:2|A-70-OUT,B-114-OUT,T-152-OUT;n:type:ShaderForge.SFN_Slider,id:152,x:30452,y:33020,ptovrint:False,ptlb:EmissionLM,ptin:_EmissionLM,varname:node_615,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Vector3,id:192,x:30698,y:31741,varname:node_192,prsc:2,v1:1,v2:1,v3:1;n:type:ShaderForge.SFN_Cubemap,id:200,x:30606,y:33246,ptovrint:False,ptlb:environment,ptin:_environment,varname:node_617,prsc:2|MIP-584-OUT;n:type:ShaderForge.SFN_Fresnel,id:203,x:30826,y:33274,varname:node_203,prsc:2|EXP-206-OUT;n:type:ShaderForge.SFN_Lerp,id:204,x:30881,y:33129,varname:node_204,prsc:2|A-70-OUT,B-200-RGB,T-203-OUT;n:type:ShaderForge.SFN_Slider,id:206,x:30606,y:33503,ptovrint:False,ptlb:environment fresnel,ptin:_environmentfresnel,varname:node_621,prsc:2,min:0,cur:0,max:1.75;n:type:ShaderForge.SFN_Lerp,id:215,x:31107,y:33085,varname:node_215,prsc:2|A-70-OUT,B-204-OUT,T-216-OUT;n:type:ShaderForge.SFN_Slider,id:216,x:31052,y:33318,ptovrint:False,ptlb:environment power,ptin:_environmentpower,varname:node_623,prsc:2,min:0,cur:0,max:2;n:type:ShaderForge.SFN_Power,id:339,x:31483,y:32934,varname:node_339,prsc:2|VAL-215-OUT,EXP-340-OUT;n:type:ShaderForge.SFN_Slider,id:340,x:31274,y:33162,ptovrint:False,ptlb:environment correction,ptin:_environmentcorrection,varname:node_625,prsc:2,min:1,cur:1,max:4;n:type:ShaderForge.SFN_RemapRange,id:427,x:30281,y:33466,varname:node_427,prsc:2,frmn:0,frmx:1,tomn:6,tomx:1|IN-428-OUT;n:type:ShaderForge.SFN_Slider,id:428,x:29879,y:33421,ptovrint:False,ptlb:environment blur,ptin:_environmentblur,varname:node_627,prsc:2,min:1,cur:1,max:0;n:type:ShaderForge.SFN_Lerp,id:442,x:31783,y:31912,varname:node_442,prsc:2|A-39-OUT,B-5-OUT,T-473-OUT;n:type:ShaderForge.SFN_OneMinus,id:473,x:31420,y:32098,varname:node_473,prsc:2|IN-38-A;n:type:ShaderForge.SFN_Multiply,id:482,x:31880,y:32923,varname:node_482,prsc:2|A-140-OUT,B-339-OUT;n:type:ShaderForge.SFN_AmbientLight,id:508,x:32632,y:32882,varname:node_508,prsc:2;n:type:ShaderForge.SFN_Lerp,id:517,x:33177,y:32213,varname:node_517,prsc:2|A-140-OUT,B-519-OUT,T-518-OUT;n:type:ShaderForge.SFN_Slider,id:518,x:32427,y:32337,ptovrint:False,ptlb:Main Brightness Control,ptin:_MainBrightnessControl,varname:node_518,prsc:2,min:1,cur:0,max:0;n:type:ShaderForge.SFN_Vector1,id:519,x:32611,y:32142,varname:node_519,prsc:2,v1:0;n:type:ShaderForge.SFN_Lerp,id:535,x:32813,y:32261,varname:node_535,prsc:2|A-442-OUT,B-519-OUT,T-518-OUT;n:type:ShaderForge.SFN_TexCoord,id:543,x:30109,y:31690,varname:node_543,prsc:2,uv:0;n:type:ShaderForge.SFN_SwitchProperty,id:548,x:30285,y:31854,ptovrint:False,ptlb:[AO] Uv0 / Uv1,ptin:_AOUv0Uv1,varname:node_548,prsc:2,on:False|A-543-UVOUT,B-549-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:549,x:30104,y:32004,varname:node_549,prsc:2,uv:1;n:type:ShaderForge.SFN_TexCoord,id:568,x:31927,y:32068,varname:node_568,prsc:2,uv:1;n:type:ShaderForge.SFN_SwitchProperty,id:569,x:32098,y:31969,ptovrint:False,ptlb:normal uv 0/1,ptin:_normaluv01,varname:node_569,prsc:2,on:False|A-570-UVOUT,B-568-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:570,x:31919,y:31755,varname:node_570,prsc:2,uv:0;n:type:ShaderForge.SFN_SwitchProperty,id:584,x:30426,y:33380,ptovrint:False,ptlb:Slider control /Gloss control,ptin:_SlidercontrolGlosscontrol,varname:node_584,prsc:2,on:False|A-589-OUT,B-427-OUT;n:type:ShaderForge.SFN_RemapRange,id:589,x:30281,y:33231,varname:node_589,prsc:2,frmn:0,frmx:1,tomn:6,tomx:1|IN-9-A;n:type:ShaderForge.SFN_Power,id:603,x:33030,y:32432,varname:node_603,prsc:2|VAL-535-OUT,EXP-604-OUT;n:type:ShaderForge.SFN_Slider,id:604,x:32656,y:32514,ptovrint:False,ptlb:Brigness Levels(contrast),ptin:_BrignessLevelscontrast,varname:node_604,prsc:2,min:2,cur:2,max:0.4;n:type:ShaderForge.SFN_SwitchProperty,id:657,x:32972,y:32965,ptovrint:False,ptlb:Override Ambient,ptin:_OverrideAmbient,varname:node_657,prsc:2,on:False|A-508-RGB,B-661-RGB;n:type:ShaderForge.SFN_Color,id:661,x:32772,y:33069,ptovrint:False,ptlb:Ambient Color,ptin:_AmbientColor,varname:node_661,prsc:2,glob:False,c1:0.05449827,c2:0.2351569,c3:0.3088235,c4:1;n:type:ShaderForge.SFN_Add,id:754,x:31915,y:32709,varname:node_754,prsc:2|A-482-OUT,B-151-OUT;proporder:518-604-2-4-38-37-548-9-139-127-128-31-569-149-33-113-152-200-216-206-340-428-584-657-661;pass:END;sub:END;*/

Shader "RaginBear/Specular/Self-Illumin/rb_std_01" {
    Properties {
        _MainBrightnessControl ("Main Brightness Control", Range(1, 0)) = 0
        _BrignessLevelscontrast ("Brigness Levels(contrast)", Range(2, 0.4)) = 2
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,0,0,1)
        _aorgbcolormaska ("ao (rgb) colormask (a)", 2D) = "white" {}
        _aopower ("ao power", Range(0, 1)) = 0.4796831
        [MaterialToggle] _AOUv0Uv1 ("[AO] Uv0 / Uv1", Float ) = 0
        _specularrgbglossa ("specular(rgb)gloss(a)", 2D) = "white" {}
        _SpecColor ("SpecColor", Color) = (0.5,0.5,0.5,1)
        _specularpower ("specular power", Range(0, 1)) = 0
        _Shininess ("Shininess", Range(1, 2048)) = 35.00855
        _BumpMap ("BumpMap", 2D) = "bump" {}
        [MaterialToggle] _normaluv01 ("normal uv 0/1", Float ) = 0
        _normalintensity ("normal intensity", Range(0, 1)) = 0
        _Illum ("Illum", 2D) = "white" {}
        _emissioncolor ("emission color", Color) = (0.5,0.5,0.5,1)
        _EmissionLM ("EmissionLM", Range(0, 1)) = 0
        _environment ("environment", Cube) = "_Skybox" {}
        _environmentpower ("environment power", Range(0, 2)) = 0
        _environmentfresnel ("environment fresnel", Range(0, 1.75)) = 0
        _environmentcorrection ("environment correction", Range(1, 4)) = 1
        _environmentblur ("environment blur", Range(1, 0)) = 1
        [MaterialToggle] _SlidercontrolGlosscontrol ("Slider control /Gloss control", Float ) = 1
        [MaterialToggle] _OverrideAmbient ("Override Ambient", Float ) = 0.2
        _AmbientColor ("Ambient Color", Color) = (0.05449827,0.2351569,0.3088235,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH_PROBE ( defined (LIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            #ifndef LIGHTMAP_OFF
                // float4 unity_LightmapST;
                // sampler2D unity_Lightmap;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform sampler2D _specularrgbglossa; uniform float4 _specularrgbglossa_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform sampler2D _Illum; uniform float4 _Illum_ST;
            uniform float _aopower;
            uniform sampler2D _aorgbcolormaska; uniform float4 _aorgbcolormaska_ST;
            uniform float4 _emissioncolor;
            uniform float _specularpower;
            uniform float _Shininess;
            uniform float _normalintensity;
            uniform float _EmissionLM;
            uniform samplerCUBE _environment;
            uniform float _environmentfresnel;
            uniform float _environmentpower;
            uniform float _environmentcorrection;
            uniform float _environmentblur;
            uniform float _MainBrightnessControl;
            uniform fixed _AOUv0Uv1;
            uniform fixed _normaluv01;
            uniform fixed _SlidercontrolGlosscontrol;
            uniform float _BrignessLevelscontrast;
            uniform fixed _OverrideAmbient;
            uniform float4 _AmbientColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD8;
                #elif SHOULD_SAMPLE_SH_PROBE
                    float3 shLight : TEXCOORD8;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                #if SHOULD_SAMPLE_SH_PROBE
                    o.shLight = ShadeSH9(float4(mul(unity_ObjectToWorld, float4(v.normal,0)).xyz * 1.0,1));
                #endif
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float2 _normaluv01_var = lerp( i.uv0, i.uv1, _normaluv01 );
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(_normaluv01_var, _BumpMap)));
                float3 normalLocal = lerp(float3(0.4980392,0.4980392,1),_BumpMap_var.rgb,_normalintensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - 0.5);
                #ifndef LIGHTMAP_OFF
                    float4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap,i.uvLM);
                    #ifndef DIRLIGHTMAP_OFF
                        float3 lightmap = DecodeLightmap(lmtex);
                        float3 scalePerBasisVector = DecodeLightmap(UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd,unity_Lightmap,i.uvLM));
                        UNITY_DIRBASIS
                        half3 normalInRnmBasis = saturate (mul (unity_DirBasis, normalLocal));
                        lightmap *= dot (normalInRnmBasis, scalePerBasisVector);
                    #else
                        float3 lightmap = DecodeLightmap(lmtex);
                    #endif
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef DIRLIGHTMAP_OFF
                        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    #else
                        float3 lightDirection = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
                        lightDirection = mul(lightDirection,tangentTransform); // Tangent to world
                    #endif
                #else
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                #endif
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float4 _specularrgbglossa_var = tex2D(_specularrgbglossa,TRANSFORM_TEX(i.uv0, _specularrgbglossa));
                float gloss = lerp(0.0,_specularrgbglossa_var.a,_Shininess);
                float specPow = gloss;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 node_70 = float3(0,0,0);
                float3 node_140 = (lerp(node_70,_specularrgbglossa_var.rgb,_specularpower)*_SpecColor.rgb);
                float node_519 = 0.0;
                float3 specularColor = lerp(node_140,float3(node_519,node_519,node_519),_MainBrightnessControl);
                #if !defined(LIGHTMAP_OFF) && defined(DIRLIGHTMAP_OFF)
                    float3 directSpecular = float3(0,0,0);
                #else
                    float3 directSpecular = 1 * pow(max(0,dot(reflect(-lightDirection, normalDirection),viewDirection)),specPow);
                #endif
                float3 specular = directSpecular * specularColor;
                #ifndef LIGHTMAP_OFF
                    #ifndef DIRLIGHTMAP_OFF
                        specular *= lightmap;
                    #else
                        specular *= (floor(attenuation) * _LightColor0.xyz);
                    #endif
                #else
                    specular *= (floor(attenuation) * _LightColor0.xyz);
                #endif
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                #ifndef LIGHTMAP_OFF
                    float3 directDiffuse = float3(0,0,0);
                #else
                    float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef SHADOWS_SCREEN
                        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
                            directDiffuse += min(lightmap.rgb, attenuation);
                        #else
                            directDiffuse += max(min(lightmap.rgb,attenuation*lmtex.rgb), lightmap.rgb*attenuation*0.5);
                        #endif
                    #else
                        directDiffuse += lightmap.rgb;
                    #endif
                #endif
                indirectDiffuse += lerp( UNITY_LIGHTMODEL_AMBIENT.rgb, _AmbientColor.rgb, _OverrideAmbient ); // Diffuse Ambient Light
                #if SHOULD_SAMPLE_SH_PROBE
                    indirectDiffuse += i.shLight; // Per-Vertex Light Probes / Spherical harmonics
                #endif
                float2 _AOUv0Uv1_var = lerp( i.uv0, i.uv1, _AOUv0Uv1 );
                float4 _aorgbcolormaska_var = tex2D(_aorgbcolormaska,TRANSFORM_TEX(_AOUv0Uv1_var, _aorgbcolormaska));
                indirectDiffuse *= _aorgbcolormaska_var.rgb; // Diffuse AO
                float3 node_39 = (_MainTex_var.rgb*lerp(float3(1,1,1),_aorgbcolormaska_var.rgb,_aopower));
                float3 diffuse = (directDiffuse + indirectDiffuse) * pow(lerp(lerp(node_39,(_Color.rgb*node_39),(1.0 - _aorgbcolormaska_var.a)),float3(node_519,node_519,node_519),_MainBrightnessControl),_BrignessLevelscontrast);
////// Emissive:
                float4 _Illum_var = tex2D(_Illum,TRANSFORM_TEX(i.uv0, _Illum));
                float3 emissive = ((node_140*pow(lerp(node_70,lerp(node_70,texCUBElod(_environment,float4(viewReflectDirection,lerp( (_specularrgbglossa_var.a*-5.0+6.0), (_environmentblur*-5.0+6.0), _SlidercontrolGlosscontrol ))).rgb,pow(1.0-max(0,dot(normalDirection, viewDirection)),_environmentfresnel)),_environmentpower),_environmentcorrection))+lerp(node_70,(_Illum_var.rgb*_emissioncolor.rgb),_EmissionLM));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH_PROBE ( defined (LIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform sampler2D _specularrgbglossa; uniform float4 _specularrgbglossa_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform sampler2D _Illum; uniform float4 _Illum_ST;
            uniform float _aopower;
            uniform sampler2D _aorgbcolormaska; uniform float4 _aorgbcolormaska_ST;
            uniform float4 _emissioncolor;
            uniform float _specularpower;
            uniform float _Shininess;
            uniform float _normalintensity;
            uniform float _EmissionLM;
            uniform samplerCUBE _environment;
            uniform float _environmentfresnel;
            uniform float _environmentpower;
            uniform float _environmentcorrection;
            uniform float _environmentblur;
            uniform float _MainBrightnessControl;
            uniform fixed _AOUv0Uv1;
            uniform fixed _normaluv01;
            uniform fixed _SlidercontrolGlosscontrol;
            uniform float _BrignessLevelscontrast;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 binormalDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float2 _normaluv01_var = lerp( i.uv0, i.uv1, _normaluv01 );
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(_normaluv01_var, _BumpMap)));
                float3 normalLocal = lerp(float3(0.4980392,0.4980392,1),_BumpMap_var.rgb,_normalintensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float4 _specularrgbglossa_var = tex2D(_specularrgbglossa,TRANSFORM_TEX(i.uv0, _specularrgbglossa));
                float gloss = lerp(0.0,_specularrgbglossa_var.a,_Shininess);
                float specPow = gloss;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 node_70 = float3(0,0,0);
                float3 node_140 = (lerp(node_70,_specularrgbglossa_var.rgb,_specularpower)*_SpecColor.rgb);
                float node_519 = 0.0;
                float3 specularColor = lerp(node_140,float3(node_519,node_519,node_519),_MainBrightnessControl);
                float3 directSpecular = attenColor * pow(max(0,dot(reflect(-lightDirection, normalDirection),viewDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float2 _AOUv0Uv1_var = lerp( i.uv0, i.uv1, _AOUv0Uv1 );
                float4 _aorgbcolormaska_var = tex2D(_aorgbcolormaska,TRANSFORM_TEX(_AOUv0Uv1_var, _aorgbcolormaska));
                float3 node_39 = (_MainTex_var.rgb*lerp(float3(1,1,1),_aorgbcolormaska_var.rgb,_aopower));
                float3 diffuse = directDiffuse * pow(lerp(lerp(node_39,(_Color.rgb*node_39),(1.0 - _aorgbcolormaska_var.a)),float3(node_519,node_519,node_519),_MainBrightnessControl),_BrignessLevelscontrast);
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #define SHOULD_SAMPLE_SH_PROBE ( defined (LIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - 0.5);
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Cull Off
            Offset 1, 1
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH_PROBE ( defined (LIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
