using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace OGLTest
{
    struct PlayerState
    {
        public bool OnGround;

        public bool WalkForward;
        public bool WalkBackward;

        public bool StrafeLeft;
        public bool StrafeRight;
    }

    class Player
    {
        private Vector3 GravityAcceleration = -Vector3.UnitY * 9.81f * 0f;

        private const float PlayerSpeed = 20.0f;
        private const float MouseSensitivity = 0.2f;

        public Camera Camera { get; }
        public Vector3 Position { get; private set; } = new Vector3(-5, 28, 8);

        private PlayerState _playerState;
        private Vector3 _physicsVelocity = Vector3.Zero;

        private Vector2 _angles = Vector2.Zero;
        private Vector3 _viewDirection;
        private Vector3 _up = Vector3.UnitY;

        public Player(Camera camera)
        {
            Camera = camera;
        }

        public void Update(double frameTime)
        {
            if (_playerState.OnGround)
                _physicsVelocity = Vector3.Zero;
            else
                _physicsVelocity += GravityAcceleration * (float) frameTime;

            Vector3 velocity = GetMovingVelocity() + _physicsVelocity;
            Position += velocity * (float) frameTime;

            UpdateCamera();
        }

        public void HandleKeyboard(KeyboardState keyState)
        {
            _playerState.WalkForward = keyState.IsKeyDown(Key.W);
            _playerState.WalkBackward = keyState.IsKeyDown(Key.S);
            _playerState.StrafeLeft = keyState.IsKeyDown(Key.A);
            _playerState.StrafeRight = keyState.IsKeyDown(Key.D);
        }

        private int _lastX = 0;
        private int _lastY = 0;
        public void HandleMouse(MouseState mouseState)
        {
            var deltaX = mouseState.X - _lastX;
            var deltaY = _lastY - mouseState.Y;

            var deltaYaw = deltaX * MouseSensitivity;
            var deltaPitch = deltaY * MouseSensitivity;

            _angles.X += deltaYaw;
            _angles.Y += deltaPitch;
            _angles.Y = MathHelper.Clamp(_angles.Y, -89.0f, 89.0f);

            _lastX = mouseState.X;
            _lastY = mouseState.Y;
        }

        private Vector3 GetMovingVelocity()
        {
            Vector3 velocity = Vector3.Zero;
            if (_playerState.WalkForward)
            {
                velocity += _viewDirection * PlayerSpeed;
            }
            if (_playerState.WalkBackward)
            {
                velocity -= _viewDirection * PlayerSpeed;
            }
            if (_playerState.StrafeLeft)
            {
                velocity -= Vector3.Normalize(Vector3.Cross(_viewDirection, _up)) * PlayerSpeed;
            }
            if (_playerState.StrafeRight)
            {
                velocity += Vector3.Normalize(Vector3.Cross(_viewDirection, _up)) * PlayerSpeed;
            }
            return velocity;
        }

        private void UpdateCamera()
        {
            Console.WriteLine($"Player position: {Position}, angles: {_angles}");

            _viewDirection.X = Utils.Cos(_angles.Y) * Utils.Cos(_angles.X);
            _viewDirection.Y = Utils.Sin(_angles.Y);
            _viewDirection.Z = Utils.Cos(_angles.Y) * Utils.Sin(_angles.X);
            _viewDirection.Normalize();

            Camera.UpdateViewMatrix(Position, _viewDirection, _up);
        }
    }
}
