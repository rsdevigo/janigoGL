using OpenTK;
using System;
using System.Collections.Generic;
namespace Scene
{
  class DirectionalLight : Light
  {

    public DirectionalLight(Vector3 pos, Vector3 amb, Vector3 diff, Vector3 spec) : base(pos, amb, diff, spec)
    {
      _type = LightTypeEnum.DIRECTIONAL_LIGHT;
    }
  }
}