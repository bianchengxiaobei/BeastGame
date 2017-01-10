Shader "TSF/BaseOutline1" {
Properties {
[MaterialToggle(_OUTL_ON)]  _Outl ("Outline", Float) = 0
[MaterialToggle(_TEX_ON)]  _DetailTex ("Enable Detail texture", Float) = 0
 _MainTex ("Detail", 2D) = "white" {}
 _ToonShade ("Shade", 2D) = "white" {}
[MaterialToggle(_COLOR_ON)]  _TintColor ("Enable Color Tint", Float) = 0
 _Color ("Base Color", Color) = (1,1,1,1)
[MaterialToggle(_VCOLOR_ON)]  _VertexColor ("Enable Vertex Color", Float) = 0
 _Brightness ("Brightness 1 = neutral", Float) = 1
[MaterialToggle(_DS_ON)]  _DS ("Enable DoubleSided", Float) = 0
[Enum(UnityEngine.Rendering.CullMode)]  _Cull ("Cull mode", Float) = 2
 _OutlineColor ("Outline Color", Color) = (0.5,0.5,0.5,1)
 _Outline ("Outline width", Float) = 0.01
[MaterialToggle(_ASYM_ON)]  _Asym ("Enable Asymmetry", Float) = 0
 _Asymmetry ("OutlineAsymmetry", Vector) = (0,0.25,0.5,0)
[MaterialToggle(_TRANSP_ON)]  _Trans ("Enable Transparency", Float) = 0
[Enum(TRANS_OPTIONS)]  _TrOp ("Transparency mode", Float) = 0
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader { 
 LOD 250
 Tags { "RenderType"="Opaque" }
 UsePass "TSF/Base1/BASE"
 Pass {
  Tags { "RenderType"="Opaque" }
  Cull Front
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "normal" Normal
Float 5 [_Outline]
"!!ARBvp1.0
PARAM c[6] = { { 0.0099999998 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
MUL R0.xyz, vertex.normal, c[5].x;
MOV R0.w, vertex.position;
MAD R0.xyz, R0, c[0].x, vertex.position;
DP4 result.position.w, R0, c[4];
DP4 result.position.z, R0, c[3];
DP4 result.position.y, R0, c[2];
DP4 result.position.x, R0, c[1];
END
# 7 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Float 4 [_Outline]
"vs_2_0
def c5, 0.01000000, 0, 0, 0
dcl_position0 v0
dcl_normal0 v1
mul r0.xyz, v1, c4.x
mov r0.w, v0
mad r0.xyz, r0, c5.x, v0
dp4 oPos.w, r0, c3
dp4 oPos.z, r0, c2
dp4 oPos.y, r0, c1
dp4 oPos.x, r0, c0
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "normal" Normal
ConstBuffer "$Globals" 48
Float 16 [_Outline]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedkbjcpjplfhfgenbmceofclhepoocdanpabaaaaaapmabaaaaadaaaaaa
cmaaaaaahmaaaaaalaaaaaaaejfdeheoeiaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaafaepfdejfeejepeoaaeoepfcenebemaaepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfaepfdejfeejepeoaafdeieefceeabaaaaeaaaabaafbaaaaaa
fjaaaaaeegiocaaaaaaaaaaaacaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaadhcbabaaaabaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagiaaaaacacaaaaaadiaaaaaihcaabaaaaaaaaaaaegbcbaaa
abaaaaaaagiacaaaaaaaaaaaabaaaaaadcaaaaamhcaabaaaaaaaaaaaegacbaaa
aaaaaaaaaceaaaaaaknhcddmaknhcddmaknhcddmaaaaaaaaegbcbaaaaaaaaaaa
diaaaaaipcaabaaaabaaaaaafgafbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaa
dcaaaaakpcaabaaaabaaaaaaegiocaaaabaaaaaaaaaaaaaaagaabaaaaaaaaaaa
egaobaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaa
kgakbaaaaaaaaaaaegaobaaaabaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaa
abaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "normal" Normal
ConstBuffer "$Globals" 48
Float 16 [_Outline]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedmmgeahfadklbghlemeonoefjjggkaimeabaaaaaabaadaaaaaeaaaaaa
daaaaaaaeaabaaaaimacaaaanmacaaaaebgpgodjaiabaaaaaiabaaaaaaacpopp
miaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaabaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaknhcddmaaaaaaaaaaaaaaaaaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjaafaaaaadaaaaahiaabaaoejaabaaaakaaeaaaaae
aaaaahiaaaaaoeiaagaaaakaaaaaoejaafaaaaadabaaapiaaaaaffiaadaaoeka
aeaaaaaeabaaapiaacaaoekaaaaaaaiaabaaoeiaaeaaaaaeaaaaapiaaeaaoeka
aaaakkiaabaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaae
aaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiappppaaaa
fdeieefceeabaaaaeaaaabaafbaaaaaafjaaaaaeegiocaaaaaaaaaaaacaaaaaa
fjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaad
hcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagiaaaaacacaaaaaa
diaaaaaihcaabaaaaaaaaaaaegbcbaaaabaaaaaaagiacaaaaaaaaaaaabaaaaaa
dcaaaaamhcaabaaaaaaaaaaaegacbaaaaaaaaaaaaceaaaaaaknhcddmaknhcddm
aknhcddmaaaaaaaaegbcbaaaaaaaaaaadiaaaaaipcaabaaaabaaaaaafgafbaaa
aaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaaabaaaaaaegiocaaa
abaaaaaaaaaaaaaaagaabaaaaaaaaaaaegaobaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaacaaaaaakgakbaaaaaaaaaaaegaobaaaabaaaaaa
dcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaa
egaobaaaaaaaaaaadoaaaaabejfdeheoeiaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaafaepfdejfeejepeoaaeoepfcenebemaaepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfaepfdejfeejepeoaa"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_OutlineColor]
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[1] = { program.local[0] };
MOV result.color, c[0];
END
# 1 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_OutlineColor]
"ps_2_0
mov_pp oC0, c0
"
}
SubProgram "d3d11 " {
ConstBuffer "$Globals" 48
Vector 32 [_OutlineColor]
BindCB  "$Globals" 0
"ps_4_0
eefiecedlnnldmoifgjonaklnecfhepedjagmamkabaaaaaanmaaaaaaadaaaaaa
cmaaaaaagaaaaaaajeaaaaaaejfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfaepfdejfeejepeoaa
epfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
aaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefceaaaaaaaeaaaaaaa
baaaaaaafjaaaaaeegiocaaaaaaaaaaaadaaaaaagfaaaaadpccabaaaaaaaaaaa
dgaaaaagpccabaaaaaaaaaaaegiocaaaaaaaaaaaacaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
ConstBuffer "$Globals" 48
Vector 32 [_OutlineColor]
BindCB  "$Globals" 0
"ps_4_0_level_9_1
eefieceddcbfkcakkmcgjfnedpdbjmamdjcplaeiabaaaaaacmabaaaaaeaaaaaa
daaaaaaahmaaaaaameaaaaaapiaaaaaaebgpgodjeeaaaaaaeeaaaaaaaaacpppp
beaaaaaadaaaaaaaabaaceaaaaaadaaaaaaadaaaaaaaceaaaaaadaaaaaaaacaa
abaaaaaaaaaaaaaaaaacppppabaaaaacaaaicpiaaaaaoekappppaaaafdeieefc
eaaaaaaaeaaaaaaabaaaaaaafjaaaaaeegiocaaaaaaaaaaaadaaaaaagfaaaaad
pccabaaaaaaaaaaadgaaaaagpccabaaaaaaaaaaaegiocaaaaaaaaaaaacaaaaaa
doaaaaabejfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaabaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfaepfdejfeejepeoaaepfdeheocmaaaaaa
abaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaa
fdfgfpfegbhcghgfheaaklkl"
}
}
 }
}
Fallback "Diffuse"
}