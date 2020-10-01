using OpenTK;
using System;
using System.Collections.Generic;
namespace Scene
{
  class SpotLight : Light
  {
    public float cutoff;
    public Vector3 direction;
    public SpotLight(Vector3 pos, Vector3 amb, Vector3 diff, Vector3 spec, Vector3 dir, float _cutoff) : base(pos, amb, diff, spec)
    {
      _type = LightTypeEnum.SPOTLIGHT;
      direction = dir;
      cutoff = _cutoff;
    }
  }
}