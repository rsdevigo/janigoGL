using OpenTK;
using OpenTK.Graphics;
using System;

namespace Scene
{
  class Camera
  {
    private float _fov = MathHelper.PiOver4;
    public float fov {
      get {return MathHelper.RadiansToDegrees(_fov);}
      set {
        var angle = MathHelper.Clamp(value, 1f, 45f);
        _fov = MathHelper.DegreesToRadians(angle);
        UpdateMatrices();
      }
    }
    public float _near = 0.5f;
    public float _far = 100f;
    public float _sensitivity = 0.2f;
    private float _width;
    private float _height;
    public Vector3 _position;
    private Vector3 _target;
    private Vector3 _direction;
    private Vector3 _up;
    private Vector3 _right;
    private Vector3 _front;
    private float _pitch;
    public float pitch {
      get{return MathHelper.RadiansToDegrees(_pitch);}
      set
      {
        var angle = MathHelper.Clamp(value, -89f, 89f);
        _pitch = MathHelper.DegreesToRadians(angle);
        UpdateVectors();
      }
    }
    private float _yaw = -MathHelper.PiOver2;
    public float yaw {
      get{return MathHelper.RadiansToDegrees(_yaw);}
      set{
        _yaw = MathHelper.DegreesToRadians(value);
        UpdateVectors();
      }
    }
    public Matrix4 _viewMatrix;
    public Matrix4 _projectionMatrix;
    private bool isOrtho = false;
    

    public Camera(Vector3 position, Vector3 target, float width, float height)
    {
      SetupCamera(position, target, width, height);
    }

    public Camera(Vector3 position, Vector3 target, float width, float height, float fov, float near, float far)
    {
      _fov = fov;
      _far = far;
      _near = near;
      SetupCamera(position, target, width, height);
    }

    private void SetupCamera(Vector3 position, Vector3 target, float width, float height)
    {
      _position = position;
      _target = target;
      _width = width;
      _height = height;
      CalculateDirection();
      UpdateVectors();
    }

    private void CalculateDirection()
    {
      _direction = Vector3.Normalize(_position - _target);
    }

    private void CalculateRight()
    {
      _right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _front));
    }

    private void CalculateUp()
    {
      _up = Vector3.Normalize(Vector3.Cross(_front, _right));
    }

    private void CalculateFront()
    {
      _front = -_direction;
    }

    private void UpdateMatrices()
    {
      _viewMatrix = Matrix4.LookAt(_position, _position + _front, _up);
      if (!isOrtho)
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(_fov, (float)_width / _height, _near, _far);
      else
        _projectionMatrix = Matrix4.CreateOrthographic(_width,_height, 0.1f, 10f);
    }

    public void Resize(float w, float h)
    {
      _width = w;
      _height = h;
      UpdateMatrices();
    }

    public void SwapProjection()
    {
      isOrtho = !isOrtho;
      UpdateMatrices();
    }

    public void Forward(double time) 
    {
      _position += _front * (float)time * 1.5f;
      UpdateMatrices();
    }

    public void Backward(double time) 
    {
      _position -= _front * (float)time * 1.5f;
      UpdateMatrices();
    }

    public void Right(double time) 
    {
      _position += Vector3.Normalize(Vector3.Cross(_front, _up)) * (float)time * 1.5f;
      UpdateMatrices();
    }

    public void Left(double time) 
    {
      _position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * (float)time * 1.5f;
      UpdateMatrices();
    }

    public void Up(double time) 
    {
      _position += _up * (float)time * 1.5f;
      UpdateMatrices();
    }

    public void Down(double time) 
    {
      _position -= _up * (float)time * 1.5f;
      UpdateMatrices();
    }

    public void UpdateVectors() {
      _front.Y = (float)Math.Sin(_pitch);
      _front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
      _front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);
      _front = Vector3.Normalize(_front);

      CalculateRight();
      CalculateUp();
      UpdateMatrices();
    }

    public void PrintCameraInfo() {
      Console.WriteLine("Position: "+_position);
      Console.WriteLine("Front: "+_front);
    }
  }
}