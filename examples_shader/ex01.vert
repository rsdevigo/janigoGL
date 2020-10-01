#version 410

layout(location = 0)in vec3 aPosition;
layout(location = 1)in vec4 aColor;
layout(location = 2)in vec3 aNormal;

layout(location = 0)out vec3 normal;
layout(location = 1)out vec3 fColor;
layout(location = 2)out vec3 fragPos;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;


void main() {
  fColor = vec3(aColor);
  normal = aNormal * mat3(transpose(inverse(modelMatrix)));
  fragPos = vec3(vec4(aPosition,1) * modelMatrix);
  gl_Position = vec4(aPosition, 1) * modelMatrix * viewMatrix * projectionMatrix;
}