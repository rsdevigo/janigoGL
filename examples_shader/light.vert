#version 410

layout(location = 0)in vec3 aPosition;
layout(location = 1)in vec4 aColor;
layout(location = 2)in vec3 aNormal;

layout(location = 0)out vec3 fColor;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 viewPos;

void main(void) {
  
}