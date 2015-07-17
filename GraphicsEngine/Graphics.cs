using System;

namespace GraphicsEngine
{
    public interface Graphics : IDisposable
    {
        void Init3D();
        void Render();
        void Draw3DCube();
        void Present();
    }
}