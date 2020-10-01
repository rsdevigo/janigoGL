#version 410

layout(location = 0)in vec3 aPosition;
layout(location = 1)in vec4 aColor;
layout(location = 2)in vec3 aNormal;

layout(location = 0)out vec4 fColor;
vec3 retornaVec3(vec3 vector);

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

void main(void) {
  fColor = aColor;
  gl_Position = vec4(aPosition, 1) * modelMatrix * viewMatrix * projectionMatrix;
}

vec3 retornaVec3(vec3 vector) {
  return vector;
}