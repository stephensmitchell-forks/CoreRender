﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace CoreRender
{
    public class GLWindow4 : GameWindow
    {
        public Action Draw { get; set; }
        public bool DepthTesting
        {
            set
            {
                if (value)
                    GL.Enable(EnableCap.DepthTest);
                else
                    GL.Disable(EnableCap.DepthTest);
            }
        }

        /// <summary>
        /// Enables writing on the depth buffer
        /// </summary>
        public bool DepthWrite
        {
            set
            {
                GL.DepthMask(value);
            }
        }

        //public bool StencilWrite
        //{
        //    set
        //    {
        //        GL.StencilFunc(StencilFunction.Always, 1, 0xff);
        //        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
        //        GL.StencilMask(0xff);
        //    }
        //}

        public GLWindow4() : base()
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);

            // transparency
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // antialiasing
            GL.Enable(EnableCap.PolygonSmooth);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Draw?.Invoke();

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle);
        }

        public int DemoTriangle()
        {
            var vertices = new float[]
            {
                0.0f, 0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f
            };

            //int vao = GL.GenVertexArray();

            //GL.BindVertexArray(vao);

            int buffer = LoadGeometry(vertices);
            
            Draw = () =>
            {
                //GL.BindVertexArray(vao);
                GL.PointSize(5f);
                GL.DrawArrays(PrimitiveType.Points, 0, 3);
                GL.BindVertexArray(0);
            };

            return buffer;
        }
        
        public int LoadGeometry(float[] vertices)
        {
            int buffer = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 4, vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            return buffer;
        }
    }
}
