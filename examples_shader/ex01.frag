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

vec3 calculatePointLight(PointLight light, vec3 fragPos, vec3 normal, vec3 viewPos, Material material);
float fallOff(PointLight light, vec3 fragPos);

void main() {
  vec3 saida = vec3(0.0);
  for(int i = 0; i < nr_of_point_lights; i++) {
      saida += calculatePointLight(pointLights[i], fragPos, normal, viewPos, material);
  }

  FragColor = vec4(saida, 1.0);
  //FragColor = vec4(fColor, 1.0);
}


vec3 calculatePointLight(PointLight light, vec3 fragPos, vec3 normal, vec3 viewPos, Material material) {
  vec3 lightDirection = normalize(light.position - fragPos);
  vec3 norm = normalize(normal);
  float _fallOff = fallOff(light, fragPos);

  //ambient
  vec3 ambient = light.ambient;

  //diffuse
  vec3 diffuse = max(dot(lightDirection, norm), 0.0) * light.diffuse * _fallOff;
  
  //specular
  vec3 reflectDir = reflect(-lightDirection, norm);
  vec3 viewDir = normalize(viewPos - fragPos);
  float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256.0);
  vec3 specular = light.specular * spec * _fallOff;
  
  return ambient * material.ambient + diffuse * material.diffuse + specular * material.specular;
}

float fallOff(PointLight light, vec3 fragPos) {
  float distance = length(light.position - fragPos);
  float _fallOff = 1 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
  return _fallOff;
}