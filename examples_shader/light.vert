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
uniform vec3 viewPos;

void main(void) {
  fragPos = vec3(vec4(aPosition,1) * modelMatrix);
  fColor = aColor.rgb;
  normal = aNormal * mat3(transpose(inverse(modelMatrix)));
  gl_Position = vec4(aPosition,1) * modelMatrix * viewMatrix * projectionMatrix;
}