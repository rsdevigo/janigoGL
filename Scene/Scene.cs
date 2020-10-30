using System;
using System.Collections.Generic;
using OpenTK;
using Newtonsoft.Json.Linq;
using janigoGL;


namespace Scene
{
  class Scene
  {
    public Camera _camera;
    public LightSourceManager lightSourceManager = new LightSourceManager();
    public List<Actor> _actors = new List<Actor>();
    public Shader _shader;

    public Scene(String scenePath = "")
    {
      string sceneJsonSource;
      using (System.IO.StreamReader reader = new System.IO.StreamReader(scenePath))
      {
        sceneJsonSource = reader.ReadToEnd();
      }
      JObject sceneJson = JObject.Parse(sceneJsonSource);
      Vector3 _cameraPosition = sceneJson["camera"]["position"].ToObject<Vector3>();
      Vector3 _cameraTarget = sceneJson["camera"]["target"].ToObject<Vector3>();
      LoadActors((JArray)sceneJson["actors"]);
      LoadLights((JArray)sceneJson["lights"]);
      _camera = new Camera(_cameraPosition, _cameraTarget, (int)sceneJson["width"], (int)sceneJson["height"]);
      _shader = new Shader((string)sceneJson["vertexShaderPath"], (string)sceneJson["fragmentShaderPath"]);
      _shader.SetVector3(_camera._position, "viewPos");
      _shader.SetMatrix4(_camera._viewMatrix, "viewMatrix");
      _shader.SetMatrix4(_camera._projectionMatrix, "projectionMatrix");
      lightSourceManager.LoadLights(_shader);
    }

    private void LoadActors(JArray actors)
    {
      JToken actor;
      for (int i = 0; i < actors.Count; i++)
      {
        actor = actors[i];
        
        if (actor["objPath"] == null) {
          continue;
        }
        Actor actorObj = new Actor((string)(actor["objPath"]));
        Transform t = new Transform();
        t.scale = actor["scale"] != null ? actor["scale"].ToObject<Vector3>() : Vector3.One;
        t.translate = actor["translate"] != null ? actor["translate"].ToObject<Vector3>() : Vector3.Zero;
        t.axis = Vector3.UnitX;
        t.angle = 0.0f;

        if (actor["rotate"] != null) {
          t.axis = actor["rotate"]["axis"] != null ? actor["rotate"]["axis"].ToObject<Vector3>() : Vector3.UnitX;
          t.angle = actor["rotate"]["angle"] != null ? (float)actor["rotate"]["angle"] : 0.0f;
        }

        Material m = new Material(Vector3.One, Vector3.One, Vector3.One, 32.0f);

        if (actor["material"] != null) {
          Vector3 ambient = actor["material"]["ambient"] != null ? actor["material"]["ambient"].ToObject<Vector3>() : Vector3.One;
          Vector3 diffuse = actor["material"]["diffuse"] != null ? actor["material"]["diffuse"].ToObject<Vector3>() : Vector3.One;
          Vector3 specular = actor["material"]["specular"] != null ? actor["material"]["specular"].ToObject<Vector3>() : Vector3.One;
          float shininess = actor["material"]["shininess"] != null ? (float)actor["material"]["shininess"] : 32.0f;
          m = new Material(ambient, diffuse, specular, shininess);
        }
        
        actorObj.transform = t;
        actorObj.material = m;
        _actors.Add(actorObj);
      }
    }

    private void LoadLights(JArray lights)
    {
      JToken light;
      for (int i = 0; i < lights.Count; i++)
      {
        light = lights[i];
        string type = (string)light["type"];
        Vector3 position = light["position"].ToObject<Vector3>();
        Vector3 ambient = light["ambient"].ToObject<Vector3>();
        Vector3 diffuse = light["diffuse"].ToObject<Vector3>();
        Vector3 specular = light["specular"].ToObject<Vector3>();

        if (type.CompareTo("Point Light") == 0)
        {
          PointLight lightObj = new PointLight(position, ambient, diffuse, specular);
          lightObj.constant = (float)light["constant"];
          lightObj.linear = (float)light["linear"];
          lightObj.quadratic = (float)light["quadratic"];
          lightSourceManager.AddLight(lightObj);
        }
        if (type.CompareTo("Directional Light") == 0)
        {
          DirectionalLight lightObj = new DirectionalLight(position, ambient, diffuse, specular);
          lightSourceManager.AddLight(lightObj);
        }
        if (type.CompareTo("Spot Light") == 0)
        {
          Vector3 direction = light["direction"].ToObject<Vector3>();
          float cutoff = (float)light["cutoff"];
          float outercutoff = (float)light["outercutoff"];
          SpotLight lightObj = new SpotLight(position, ambient, diffuse, specular, direction, cutoff, outercutoff);
          lightObj.constant = (float)light["constant"];
          lightObj.linear = (float)light["linear"];
          lightObj.quadratic = (float)light["quadratic"];
          lightSourceManager.AddLight(lightObj);
        }
      }
    }

    public void Draw()
    {
      _shader.SetVector3(_camera._position, "viewPos");
      _shader.SetMatrix4(_camera._viewMatrix, "viewMatrix");
      _shader.SetMatrix4(_camera._projectionMatrix, "projectionMatrix");
      _actors.ForEach(actor =>
      {
        actor.Draw(_shader);
      });
    }

    public void UnLoad()
    {
      _actors.ForEach(actor =>
      {
        actor.UnLoad();
      });
      _shader.Dispose();
    }

    ~Scene()
    {
      _actors.ForEach(actor =>
      {
        actor.UnLoad();
      });
      _shader.Dispose();
    }

    public void SwapProjection()
    {
      _camera.SwapProjection();
      _shader.SetMatrix4(_camera._viewMatrix, "viewMatrix");
      _shader.SetMatrix4(_camera._projectionMatrix, "projectionMatrix");
    }
  }
}