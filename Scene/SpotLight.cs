using OpenTK;
using System;
using System.Collections.Generic;
namespace Scene
{
  class SpotLight : Light
  {
    public float cutoff;
    public float outercutoff;
    public Vector3 direction;
    public SpotLight(Vector3 pos, Vector3 amb, Vector3 diff, Vector3 spec, Vector3 dir, float _cutoff, float _outercutoff) : base(pos, amb, diff, spec)
    {
      _type = LightTypeEnum.SPOTLIGHT;
      direction = dir;
      cutoff = _cutoff;
      outercutoff = _outercutoff;
    }
  }
}