#version 410

layout(location = 0) in vec4 fColor;

layout(location = 0) out vec4 FragColor;

void main(void) {
  FragColor = fColor;
}