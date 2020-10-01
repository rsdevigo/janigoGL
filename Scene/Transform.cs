using OpenTK;
using opentk_example;
namespace Scene
{
  class Transform
  {
    Vector3 _translate = Vector3.Zero;
    Vector3 _scale = new Vector3(1.0f);
    Vector3 _axis = Vector3.UnitZ;
    float _angle = 0;
    public Vector3 translate { set { _translate = value; } }
    public Vector3 scale { set { _scale = value; } }
    public Vector3 axis { set { _axis = value; } }
    public float angle { set { _angle = value; } }
    public Matrix4 LocalToWorldMatrix()
    {
      Quaternion q = Quaternion.FromAxisAngle(_axis, _angle);
      return Matrix4.CreateTranslation(_translate) * Matrix4.CreateScale(_scale) * Matrix4.CreateFromQuaternion(q);
    }
  }
}