#version 410

layout(location = 0)in vec3 normal;
layout(location = 1)in vec3 fColor;
layout(location = 2)in vec3 fragPos;
layout(location = 3)in vec2 texCoords;

out vec4 FragColor;

struct Material {
  vec3 ambient;
  vec3 diffuse;
  vec3 specular;
  float shininess;
  sampler2D texture_diffuse1;
  sampler2D texture_specular1;
};


uniform Material material;

void main()
{
    FragColor = texture(material.texture_diffuse1, texCoords);
}


