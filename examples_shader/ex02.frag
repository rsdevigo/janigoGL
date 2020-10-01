#version 410

layout(location = 0)in vec3 normal;
layout(location = 1)in vec3 fColor;
layout(location = 2)in vec3 fragPos;
layout(location = 3)in float vX;
layout(location = 4)in float vY;
layout(location = 5)in float vLightIntensity;

out vec4 FragColor;

const vec3 WHITE = vec3(1.,1.,1.);
const float uA = 1.0;
const float uP = 0.25;
const float uTol = 0.25;

void main() {
  vec3 outcolor = vec3(1., 0, 0);
  float f = fract(uA * vX);
  float t = smoothstep( 0.5 - uP - uTol, 0.5 - uP + uTol, f) - 
  smoothstep( 0.5 + uP - uTol, 0.5 + uP + uTol, f);
  vec3 rgb = vLightIntensity * mix( WHITE, outcolor, t);
  FragColor = vec4(rgb, 1.0);
}