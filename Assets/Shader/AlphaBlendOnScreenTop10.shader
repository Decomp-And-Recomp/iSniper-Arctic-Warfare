Shader "iPhone/AlphaBlendOnScreenTop10" {
Properties {
 _MainTex ("Texture", 2D) = "white" {}
 _TintColor ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent+10" "IGNOREPROJECTOR"="True" }
 Pass {
  Tags { "QUEUE"="Transparent+10" "IGNOREPROJECTOR"="True" }
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_TintColor] combine texture }
 }
}
}