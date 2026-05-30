Shader "Triniti/ModelTransparentShow" {
Properties {
 _MainTex ("MainTex", 2D) = "" {}
 clrBase ("Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  Color [clrBase]
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * primary }
 }
}
}