#version 410

layout(location = 0)in vec3 aPosition;
layout(location = 1)in vec4 aColor;
layout(location = 2)in vec3 aNormal;

layout(location = 0)out vec3 normal;
layout(location = 1)out vec3 fColor;
layout(location = 2)out vec3 fragPos;
layout(location = 3)out float vX;
layout(location = 4)out float vY;
layout(location = 5)out float vLightIntensity;


uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

const vec3 LIGHTPOS = vec3( 0., 0., 10. );
const float uAmp = 3.0;
const float uFreq = 5.0;

void main() {
  fColor = vec3(aColor);
  normal = aNormal * mat3(transpose(inverse(modelMatrix)));
  vec3 ECPosition = (vec4(aPosition,1) * modelMatrix * viewMatrix).xyz;
  vLightIntensity = abs(dot( normalize(LIGHTPOS - ECPosition), normal));
  vX = aPosition.x;
  vY = aPosition.y;
  vX = vX + uAmp * sin(uFreq * vY);
  fragPos = vec3(vec4(aPosition,1) * modelMatrix);
  gl_Position = vec4(aPosition, 1) * modelMatrix * viewMatrix * projectionMatrix;
}