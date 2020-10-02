using OpenTK;
using janigoGL;
namespace Scene
{
  class Material
  {
    private Vector3 _ambient;
    private Vector3 _diffuse;
    private Vector3 _specular;
    private float _shininess;
    public Material(Vector3 amb, Vector3 diff, Vector3 spec, float shininess)
    {
      _ambient = amb;
      _diffuse = diff;
      _specular = spec;
      _shininess = shininess;
    }

    public void LoadMaterial(Shader shader)
    {
      shader.SetVector3(_ambient, "material.ambient");
      shader.SetVector3(_diffuse, "material.diffuse");
      shader.SetVector3(_specular, "material.specular");
      shader.SetFloat(_shininess, "material.shininess");
    }
  }
}