using OpenTK;
using OpenTK.Input;
using System;

namespace OGLTest
{
    class Camera
    {
        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ProjectionMatrix { get; private set; }

        private const float _cameraSpeed = 1.0f;

        public Camera(float width, float height)
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), width / height, 0.1f, 100.0f);
        }

        public void UpdateViewMatrix(Vector3 position, Vector3 direction, Vector3 up)
        {
            ViewMatrix = Matrix4.LookAt(position, position + direction, up);
        }
    }
}