#version 410

layout(location = 0)in vec3 aPosition;
layout(location = 1)in vec4 aColor;
layout(location = 2)in vec3 aNormal;
layout(location = 3)in vec2 aTexCoords;
layout(location = 4)in vec3 aTangentCoords;
layout(location = 5)in vec3 biTangentCoords;

layout(location = 0)out vec3 normal;
layout(location = 1)out vec3 fColor;
layout(location = 2)out vec3 fragPos;
layout(location = 3)out vec2 texCoords;
layout(location = 4)out mat3 TBN;
layout(location = 7)out vec3 tangentViewPos;
layout(location = 8)out vec3 tangentFragPos;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 viewPos;

void main(void) {
  fragPos = vec3(vec4(aPosition,1) * modelMatrix);
  fColor = aColor.rgb;
  texCoords = aTexCoords;

  mat3 normalMatrix = mat3(transpose(inverse(modelMatrix)));
  vec3 T = normalize(vec3(normalMatrix * aTangentCoords));
  vec3 N = normalize(vec3(normalMatrix * aNormal));
  T = normalize(T - dot(T, N) * N);
  vec3 B = cross(N, T);
  TBN = transpose(mat3(T, B, N));
  tangentViewPos = TBN * viewPos;
  tangentFragPos = TBN * fragPos;


  gl_Position = vec4(aPosition,1) * modelMatrix * viewMatrix * projectionMatrix;
}