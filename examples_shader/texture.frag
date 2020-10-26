#version 410

layout(location = 0)in vec3 normal;
layout(location = 1)in vec3 fColor;
layout(location = 2)in vec3 fragPos;
layout(location = 3)in vec2 texCoords;

out vec4 FragColor;


uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;

void main()
{
    FragColor = texture(texture_diffuse1, texCoords);
}


