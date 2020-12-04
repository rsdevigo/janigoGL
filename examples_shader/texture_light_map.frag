#version 410

layout(location = 0)in vec3 normal;
layout(location = 1)in vec3 fColor;
layout(location = 2)in vec3 fragPos;
layout(location = 3)in vec2 texCoords;

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
  float outercutoff;
};

struct Material {
  vec3 ambient;
  vec3 diffuse;
  vec3 specular;
  float shininess;
  sampler2D texture_diffuse1;
  sampler2D texture_specular1;
  sampler2D texture_normal1;
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
vec3 calcSpotLight(SpotLight light, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial);
float fallOff(float linear, float quadratic, float constant, vec3 point, vec3 lightPosition);
vec3 calcDirectionalLight(DirectionalLight light, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial);
vec3 calcADS(vec3 ambient, vec3 diffuse, vec3 specular, vec3 lightDir, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial);


void main(void) {
  vec3 saida = vec3(0.0);
  for (int i = 0; i < nr_of_dir_lights; i++) {
    saida += calcDirectionalLight(directionalLights[i], fragPos, normal, viewPos, material);
  }

  for (int i = 0; i < nr_of_point_lights; i++) {
    saida += calcLightPoint(pointLights[i], fragPos, normal, viewPos, material);
  }

  for (int i = 0; i < nr_of_spot_lights; i++) {
    saida += calcSpotLight(spotLights[i], fragPos, normal, viewPos, material);
  }

  FragColor = vec4(saida, 1);
  float gamma = 2.2;
  FragColor.rgb = pow(FragColor.rgb, vec3(1.0/gamma));
}

vec3 calcLightPoint(PointLight light, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial) {

  float f = fallOff(light.linear, light.quadratic, light.constant, point, light.position);

  vec3 lightDir = normalize(light.position - point);

  return calcADS(light.ambient, light.diffuse, light.specular, lightDir, point, point_normal, eyePoint, surfaceMaterial) * f;
}

float fallOff(float linear, float quadratic, float constant, vec3 point, vec3 lightPosition) {
  float dist = length(point - lightPosition);
  float falloff = 1.0/(constant + linear * dist + quadratic * pow(dist,2));

  return falloff;
}

vec3 calcDirectionalLight(DirectionalLight light, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial) {
  vec3 lightDir = normalize(-light.direction);
  return calcADS(light.ambient, light.diffuse, light.specular, lightDir, point, point_normal, eyePoint, surfaceMaterial);
}

vec3 calcSpotLight(SpotLight light, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial) {
  vec3 lightDir = normalize(light.position - point);

  float cosTheta = dot(lightDir, normalize(-light.direction));

  float cosCutoff = cos(radians(light.cutoff));
  float cosOuterCutOff = cos(radians(light.outercutoff));

  float f = fallOff(light.linear, light.quadratic, light.constant, point, light.position);

  float epsilon = cosCutoff - cosOuterCutOff;
  float intensity = clamp((cosTheta - cosOuterCutOff) / epsilon, 0.0, 1.0);
  vec3 ads = calcADS(light.ambient, light.diffuse, light.specular, lightDir, point, point_normal, eyePoint, surfaceMaterial);
  ads = ads - light.ambient * texture(surfaceMaterial.texture_diffuse1, texCoords).xyz;
  return (ads * intensity + light.ambient * texture(surfaceMaterial.texture_diffuse1, texCoords).xyz) * f;
}

vec3 calcADS(vec3 ambient, vec3 diffuse, vec3 specular, vec3 lightDir, vec3 point, vec3 point_normal, vec3 eyePoint, Material surfaceMaterial) {
  
  vec3 norm = normalize(point_normal);
  float diffuseScalar = max(dot(lightDir, norm), 0.0);
  diffuse = diffuse * diffuseScalar;

  vec3 normEyeDir = normalize(eyePoint - point);
  vec3 reflectDir = reflect(-lightDir, norm);

  float specularScalar = max(dot(reflectDir, normEyeDir), 0.0);
  specularScalar = pow(specularScalar, material.shininess);
  specular = specular * specularScalar;

  return ambient * texture(surfaceMaterial.texture_diffuse1, texCoords).rgb + diffuse * texture(surfaceMaterial.texture_diffuse1, texCoords).rgb + specular * texture(surfaceMaterial.texture_specular1, texCoords).rgb;
}