using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEngine
{
    public class Sprite
    {
        public Sprite(Texture texture, float width, float height)
        {
            this.texture = texture;
            Width = width;
            Height = height;
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture2D);
        }

        private readonly Texture texture;
        public float Width { get; private set; }
        public float Height { get; private set; }

        private float[] vertices;
        private float[] uvs = new float[] { 0.0f, 0.0f,   0.0f, 1.0f,   1.0f, 1.0f,   1.0f, 0.0f };
        private short[] indices = new short[] { 0, 1, 2, 0, 2, 3 };

        public void Draw(float x = 0.0f, float y = 0.0f)
        {
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
            var w2 = Width / 2;
            var h2 = Height / 2;
            vertices = new float[]
            {
                x-w2, y+h2, 0,
                x-w2, y-h2, 0,
                x+w2, y-h2, 0,
                x+w2, y+h2, 0
            };
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, uvs);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, vertices);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length,
                DrawElementsType.UnsignedShort, indices);
        }
    }
}