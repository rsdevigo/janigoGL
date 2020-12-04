#version 410 core
layout (location = 0) in vec3 aPos;

out vec3 TexCoords;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main()
{
    TexCoords = aPos;
    vec4 pos = vec4(aPos,1.0) * viewMatrix * projectionMatrix;
    gl_Position = pos.xyww;
}