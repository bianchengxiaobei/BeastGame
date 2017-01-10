Shader "Character/InTree_Normal" {
Properties {
 _RimColor ("Rim Color", Color) = (0,0,0,3)
 _RimParam ("Rim Parameter", Vector) = (3,1,1,1)
 _InnerColor ("Inner Color", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("Diffuse Texture", 2D) = "white" {}
 _BrightColor ("Brighten Color", Color) = (0,0,0,1)
 _BrightColorFactor ("Brighten Scale", Float) = 1
 _OutlineColor ("Outline Color", Color) = (0.5,0.5,0.5,1)
 _Outline ("Outline width", Float) = 0.01
}
SubShader { 
 UsePass "Character/Unit/Transparent0/COMPONENT"
 UsePass "Character/Unit/Transparent1/COMPONENT"
 UsePass "Character/Unit/Transparent2/COMPONENT"
 UsePass "Character/Unit/Outline/COMPONENT"
}
Fallback "Diffuse"
}