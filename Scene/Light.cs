using OpenTK;
using System;
using opentk_example;
namespace Scene
{
  abstract class Light
  {
    public Vector3 position;
    public Vector3 ambient;
    public Vector3 diffuse;
    public Vector3 specular;
    public float constant = 1;
    public float linear = 0.09f;
    public float quadratic = 0.032f;
    protected LightTypeEnum _type;
    public LightTypeEnum type
    {
      get { return _type; }
    }

    public Light(Vector3 pos, Vector3 amb, Vector3 diff, Vector3 spec)
    {
      position = pos;
      ambient = amb;
      diffuse = diff;
      specular = spec;
    }
    public Light(Vector3 pos, Vector3 amb, Vector3 diff, Vector3 spec, float _constant, float _linear, float _quadratic)
    {
      position = pos;
      ambient = amb;
      diffuse = diff;
      specular = spec;
      constant = _constant;
      linear = _linear;
      quadratic = _quadratic;
    }

  }
}