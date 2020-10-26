#version 410

layout(location = 0)in vec3 normal;
layout(location = 2)in vec3 fragPos;

const vec3 lightPosition = vec3(10,5,10);

out vec4 FragColor;

vec3 calcColor(float intensity);

void main(void) {
  vec3 lightDir = normalize(lightPosition - fragPos);
  float intensity = dot(lightDir, normalize(normal));
  FragColor = vec4(calcColor(intensity), 1);
}

vec3 calcColor(float intensity) {
  vec3 color = vec3(0);
  if (intensity > 0.95) {
    color = vec3(0.71, 0.14, 0.98);
  } else if(intensity > 0.5) {
    color = vec3(0.53, 0.10, 0.72);
  } else if (intensity > 0.25) {
    color = vec3(0.34, 0.07, 0.47);
  } else {
    color = vec3(0.16, 0.03, 0.23);
  }

  return color;
}