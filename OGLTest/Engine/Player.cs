using System;
using System.Diagnostics;
using OGLTest.DebugUtilities;
using OGLTest.Engine;
using OGLTest.Utilities;
using OpenTK;
using OpenTK.Input;

namespace OGLTest
{
  internal class Player
  {
    private readonly CollisionController _collisionController;
    private readonly BlockController _blockController;

    private const bool DoCollision = false;
    private const float PlayerSpeed = 20.0f;
    private const float MouseSensitivity = 0.2f;

    private readonly Vector3 GravityAcceleration = -Vector3.UnitY * 9.81f;

    private readonly float CameraHeight = 0.8f;
    private readonly float PlayerHeight = 0.9f;
    private readonly float PlayerWidth = 0.8f;

    private Vector2 _angles = Vector2.Zero;

    private int _lastX;
    private int _lastY;
    private Vector3 _physicsVelocity = Vector3.Zero;

    private PlayerState _playerState;
    private readonly Vector3 _up = Vector3.UnitY;
    private Vector3 _viewDirection;

    private Stopwatch _blockDestoryCooldown = new Stopwatch();

    public Player(Camera camera, CollisionController collisionController, BlockController blockController)
    {
      _collisionController = collisionController;
      _blockController = blockController;
      Camera = camera;
    }

    public Camera Camera { get; }
    public Vector3 Position { get; private set; } = new Vector3(-5, 28, 8);

    public void Update(float frameTime)
    {
      // ApplyGravity(frameTime);
      ApplyMovement(frameTime);

      var ray = new Ray(Position + new Vector3(0, CameraHeight, 0), _viewDirection);
      if (_collisionController.CollidesWithWorld(ray, 5.0f, out var hit))
      {
        var worldPos = new WorldPosition(hit);
        DebugDrawManager.AddElement(new DebugDrawCube(worldPos.BBox(Vector3.One)), 100);

        if (_playerState.Attack1)
        {
          if (!_blockDestoryCooldown.IsRunning)
          {
            _blockController.DestroyBlock(worldPos);
            _blockDestoryCooldown.Start();
          }
          else
          {
            if (_blockDestoryCooldown.ElapsedMilliseconds > 200)
              _blockDestoryCooldown.Reset();
          }
        }
      }

      UpdateCamera();
    }

    public void HandleKeyboard(KeyboardState keyState)
    {
      _playerState.WalkForward = keyState.IsKeyDown(Key.W);
      _playerState.WalkBackward = keyState.IsKeyDown(Key.S);
      _playerState.StrafeLeft = keyState.IsKeyDown(Key.A);
      _playerState.StrafeRight = keyState.IsKeyDown(Key.D);
    }

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

      _playerState.Attack1 = (mouseState.LeftButton == ButtonState.Pressed);
    }

    private void ApplyMovement(float frameTime)
    {
      var newPosition = Position + frameTime * GetMovingVelocity();
      var bBox = BuildBBox(newPosition);

      if (!DoCollision || !_collisionController.CollidesWithWorld(bBox))
        Position = newPosition;
    }

    private void ApplyGravity(float frameTime)
    {
      _physicsVelocity += GravityAcceleration * frameTime;

      var newPosition = Position + frameTime * _physicsVelocity;
      var bBox = BuildBBox(newPosition);

      if (DoCollision && _collisionController.CollidesWithWorld(bBox))
        _physicsVelocity = Vector3.Zero;
      else
        Position = newPosition;
    }

    private BBox BuildBBox(Vector3 position)
    {
      var halfPlayerWidth = PlayerWidth * 0.5f;
      var halfPlayerWidthVec = new Vector3(halfPlayerWidth, 0, halfPlayerWidth);

      var min = new Vector3(position - halfPlayerWidthVec);
      var max = new Vector3(position + halfPlayerWidthVec + new Vector3(0, PlayerHeight, 0));

      return new BBox(min, max);
    }

    private Vector3 GetMovingVelocity()
    {
      var velocity = Vector3.Zero;
      if (_playerState.WalkForward)
        velocity += _viewDirection * PlayerSpeed;
      if (_playerState.WalkBackward)
        velocity -= _viewDirection * PlayerSpeed;
      if (_playerState.StrafeLeft)
        velocity -= Vector3.Normalize(Vector3.Cross(_viewDirection, _up)) * PlayerSpeed;
      if (_playerState.StrafeRight)
        velocity += Vector3.Normalize(Vector3.Cross(_viewDirection, _up)) * PlayerSpeed;

      // TODO: Do this the right way!
      // velocity.Y = 0;

      return velocity;
    }

    private void UpdateCamera()
    {
      Console.WriteLine($"Player position: {Position}, angles: {_angles}");

      var CameraPosition = Position + new Vector3(0, CameraHeight, 0);

      _viewDirection.X = Utils.Cos(_angles.Y) * Utils.Cos(_angles.X);
      _viewDirection.Y = Utils.Sin(_angles.Y);
      _viewDirection.Z = Utils.Cos(_angles.Y) * Utils.Sin(_angles.X);
      _viewDirection.Normalize();

      Camera.UpdateViewMatrix(CameraPosition, _viewDirection, _up);
    }
  }
}