using OpenTK;
using janigoGL;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Scene
{
  class LightSourceManager
  {
    private List<Light> _lights = new List<Light>();

    public LightSourceManager()
    {
    }

    public void AddLight(Light light)
    {
      _lights.Add(light);
    }

    public void LoadLights(Shader shader)
    {
      SetNumberOfLights(shader);
      int p = 0;
      int d = 0;
      int s = 0;
      int j;
      string uniformName = "";
      _lights.ForEach(light =>
      {
        if (light.type == LightTypeEnum.DIRECTIONAL_LIGHT)
        {
          uniformName = "directionalLights";
          j = d++;
          shader.SetVector3(light.position, $"{uniformName}[{j}].direction");
        }
        else if (light.type == LightTypeEnum.POINT_LIGHT)
        {
          uniformName = "pointLights";
          j = p++;
          shader.SetVector3(light.position, $"{uniformName}[{j}].position");
        }
        else
        {
          uniformName = "spotLights";
          j = s++;
          SpotLight l = (SpotLight)light;
          shader.SetVector3(((SpotLight)light).direction, $"{uniformName}[{j}].direction");
          shader.SetVector3(((SpotLight)light).position, $"{uniformName}[{j}].position");
          shader.SetFloat(((SpotLight)light).cutoff, $"{uniformName}[{j}].cutoff");
          shader.SetFloat(((SpotLight)light).outercutoff, $"{uniformName}[{j}].outercutoff");
        }
        shader.SetFloat(light.constant, $"{uniformName}[{j}].constant");
        shader.SetFloat(light.linear, $"{uniformName}[{j}].linear");
        shader.SetFloat(light.quadratic, $"{uniformName}[{j}].quadratic");
        shader.SetVector3(light.diffuse, $"{uniformName}[{j}].diffuse");
        shader.SetVector3(light.ambient, $"{uniformName}[{j}].ambient");
        shader.SetVector3(light.specular, $"{uniformName}[{j}].specular");
      });
    }

    public void SetNumberOfLights(Shader shader)
    {
      int nrDirectionLights = 0;
      int nrSpotLights = 0;
      int nrPointLights = 0;
      _lights.ForEach(light =>
      {
        if (light.type == LightTypeEnum.DIRECTIONAL_LIGHT)
          nrDirectionLights++;
        if (light.type == LightTypeEnum.POINT_LIGHT)
          nrPointLights++;
        if (light.type == LightTypeEnum.SPOTLIGHT)
          nrSpotLights++;
      });

      shader.SetInt(nrDirectionLights, "nr_of_dir_lights");
      shader.SetInt(nrPointLights, "nr_of_point_lights");
      shader.SetInt(nrSpotLights, "nr_of_spot_lights");
    }
  }
}