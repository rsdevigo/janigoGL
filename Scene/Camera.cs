using OpenTK;
using OpenTK.Graphics;
using System;

namespace Scene
{
  class Camera
  {
    public float _fov = 1f;
    public float _near = 0.5f;
    public float _far = 100f;
    private float _width;
    private float _height;
    public Vector3 _position;
    private Vector3 _target;
    private Vector3 _direction;
    private Vector3 _up;
    private Vector3 _right;
    private Vector3 _front;
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
      CalculateRight();
      CalculateUp();
      CalculateFront();
      UpdateMatrices();
    }

    private void CalculateDirection()
    {
      _direction = Vector3.Normalize(_position - _target);
    }

    private void CalculateRight()
    {
      _right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _direction));
    }

    private void CalculateUp()
    {
      _up = Vector3.Normalize(Vector3.Cross(_direction, _right));
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
        _projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, 0, _width, _height, 0.1f, 100f);
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
  }
}