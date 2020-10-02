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

void main(void) {
}