Shader "iPhone/Additive3" {
Properties {
 _TintColor ("Tint Color", Color) = (1,1,1,1)
 _MainTex ("Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent+3" "IGNOREPROJECTOR"="True" "RenderType"="Transparent+3" }
 Pass {
  Tags { "QUEUE"="Transparent+3" "IGNOREPROJECTOR"="True" "RenderType"="Transparent+3" }
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend SrcAlpha One
  SetTexture [_MainTex] { ConstantColor [_TintColor] combine texture * constant }
 }
}
}