using OpenTK;
using OpenTK.Input;
using System;

namespace OGLTest
{
    class Camera
    {
        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ProjectionMatrix { get; private set; }

        private Vector3 _position = new Vector3(0, 0, 0);

        private float _yaw = 0.0f;
        private float _pitch = 0.0f;
        private Vector3 _direction = Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private const float _cameraSpeed = 1.0f;

        public Camera(float width, float height)
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), width / height, 0.1f, 100.0f);
            updateViewMatrix();
        }

        public void updateViewMatrix()
        {
            var newDirection = new Vector3
            {
                X = f_cos(_pitch) * f_cos(_yaw),
                Y = f_sin(_pitch),
                Z = f_cos(_pitch) * f_sin(_yaw)
            };
            _direction = Vector3.Normalize(newDirection);

            ViewMatrix = Matrix4.LookAt(_position, _position + _direction, _up);
        }

        public void HandleKeyboard(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Key.W))
            {
                _position += _direction * _cameraSpeed;
            }
            if (keyState.IsKeyDown(Key.S))
            {
                _position -= _direction * _cameraSpeed;
            }
            if (keyState.IsKeyDown(Key.A))
            {
                _position -= Vector3.Normalize(Vector3.Cross(_direction, _up)) * _cameraSpeed;
            }
            if (keyState.IsKeyDown(Key.D))
            {
                _position += Vector3.Normalize(Vector3.Cross(_direction, _up)) * _cameraSpeed;
            }
            updateViewMatrix();
        }

        private int _lastX = 0;
        private int _lastY = 0;
        public void HandleMouse(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var deltaX =  mouseState.X - _lastX;
                var deltaY = _lastY - mouseState.Y;

                var sensitivity = 0.10f;
                var deltaYaw = deltaX * sensitivity;
                var deltaPitch = deltaY * sensitivity;

                _yaw += deltaYaw;
                _pitch += deltaPitch;
            }

            _lastX = mouseState.X;
            _lastY = mouseState.Y;
        }

        private static float f_sin(float degrees)
        {
            return (float) Math.Sin(MathHelper.DegreesToRadians(degrees));
        }
        private static float f_cos(float degrees)
        {
            return (float) Math.Cos(MathHelper.DegreesToRadians(degrees));
        }
    }
}