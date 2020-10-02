#version 410

layout(location = 0)in vec3 normal;
layout(location = 1)in vec3 fColor;
layout(location = 2)in vec3 fragPos;

#define MAX_LIGHT_SIZE 20

struct DirectionalLight {
  vec3 direction;
  vec3 ambient;
  vec3 diffuse;
  vec3 specular;
};

struct PointLight {
  vec3 position;
  vec3 ambient;
  vec3 diffuse;
  vec3 specular;
  float constant;
  float linear;
  float quadratic;
};

struct SpotLight {
  vec3 direction;
  vec3 position;
  vec3 ambient;
  vec3 diffuse;
  vec3 specular;
  float constant;
  float linear;
  float quadratic;
  float cutoff;
};

struct Material {
  vec3 ambient;
  vec3 diffuse;
  vec3 specular;
  float shininess;
};

uniform int nr_of_dir_lights;
uniform int nr_of_point_lights;
uniform int nr_of_spot_lights;
uniform vec3 viewPos;

uniform DirectionalLight directionalLights[MAX_LIGHT_SIZE];
uniform PointLight pointLights[MAX_LIGHT_SIZE];
uniform SpotLight spotLights[MAX_LIGHT_SIZE];

uniform Material material;

out vec4 FragColor;

vec3 calcLightPoint(PointLight light, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial);

void main(void) {
  vec3 saida = vec3(0.0);
  for (int i = 0; i < nr_of_point_lights; i++) {
    saida += calcLightPoint(pointLights[i], fragPos, normal, viewPos, material);
  }
  FragColor = vec4(saida, 1);
}

vec3 calcLightPoint(PointLight light, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial) {
  //TODO: modelo de iluminação ADS usando os atributos do material e da luz.
  //Onde a normal já está normalizada
  return vec3(0.0);
}