// This code was written for the OpenTK library and has been released
// to the Public Domain.
// It is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTkClient
{
	class TextRendering : GameWindow
	{
		TextRenderer renderer;
		Font serif = new Font(FontFamily.GenericSerif, 24);
		Font sans = new Font(FontFamily.GenericSansSerif, 24);
		Font mono = new Font(FontFamily.GenericMonospace, 24);
		float angle;

		public TextRendering()
			: base(800, 600)
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			renderer = new TextRenderer(Width, Height);
			PointF position = PointF.Empty;

			renderer.Clear(Color.Transparent);
			renderer.DrawString("The quick brown fox jumps over the lazy dog", serif, Brushes.White, position);
			position.Y += serif.Height;
			renderer.DrawString("The quick brown fox jumps over the lazy dog", sans, Brushes.White, position);
			position.Y += sans.Height;
			renderer.DrawString("The quick brown fox jumps over the lazy dog", mono, Brushes.White, position);
			position.Y += mono.Height;
		}

		protected override void OnUnload(EventArgs e)
		{
			renderer.Dispose();
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(ClientRectangle);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			if (Keyboard[OpenTK.Input.Key.Escape])
			{
				this.Exit();
			}
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			RenderCube((float)e.Time);
			RenderGui();

			SwapBuffers();
		}

		void RenderCube(float time)
		{
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
				MathHelper.PiOver2, Width / (float)Height, 1.0f, 128.0f);
			Matrix4 modelview = Matrix4.LookAt(0, 3, 3, 0, 0, 0, 0, 1, 0);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref modelview);
			GL.Rotate(angle, 0.0f, 1.0f, 0.0f);
			angle +=  time * 100;

			GL.Enable(EnableCap.DepthTest);

			GL.Begin(BeginMode.Quads);

			GL.Color3(Color.Red);
			GL.Vertex3(-1.0f, -1.0f, -1.0f);
			GL.Vertex3(-1.0f, 1.0f, -1.0f);
			GL.Vertex3(1.0f, 1.0f, -1.0f);
			GL.Vertex3(1.0f, -1.0f, -1.0f);

			GL.Color3(Color.Green);
			GL.Vertex3(-1.0f, -1.0f, -1.0f);
			GL.Vertex3(1.0f, -1.0f, -1.0f);
			GL.Vertex3(1.0f, -1.0f, 1.0f);
			GL.Vertex3(-1.0f, -1.0f, 1.0f);

			GL.Color3(Color.Blue);
			GL.Vertex3(-1.0f, -1.0f, -1.0f);
			GL.Vertex3(-1.0f, -1.0f, 1.0f);
			GL.Vertex3(-1.0f, 1.0f, 1.0f);
			GL.Vertex3(-1.0f, 1.0f, -1.0f);

			GL.Color3(Color.Yellow);
			GL.Vertex3(-1.0f, -1.0f, 1.0f);
			GL.Vertex3(1.0f, -1.0f, 1.0f);
			GL.Vertex3(1.0f, 1.0f, 1.0f);
			GL.Vertex3(-1.0f, 1.0f, 1.0f);

			GL.Color3(Color.Magenta);
			GL.Vertex3(-1.0f, 1.0f, -1.0f);
			GL.Vertex3(-1.0f, 1.0f, 1.0f);
			GL.Vertex3(1.0f, 1.0f, 1.0f);
			GL.Vertex3(1.0f, 1.0f, -1.0f);

			GL.Color3(Color.Violet);
			GL.Vertex3(1.0f, -1.0f, -1.0f);
			GL.Vertex3(1.0f, 1.0f, -1.0f);
			GL.Vertex3(1.0f, 1.0f, 1.0f);
			GL.Vertex3(1.0f, -1.0f, 1.0f);

			GL.End();

			GL.Disable(EnableCap.DepthTest);
		}

		void RenderGui()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BindTexture(TextureTarget.Texture2D, renderer.Texture);

			GL.Begin(BeginMode.Quads);

			GL.Color3(Color.White);
			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1f, -1f);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1f, -1f);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1f, 1f);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1f, 1f);

			GL.End();

			GL.Disable(EnableCap.Texture2D);
			GL.Disable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);
		}

		public static void Main()
		{
			using (TextRendering example = new TextRendering())
			{
				//Utilities.SetWindowTitle(example);
				example.Run(30.0);
			}
		}
	}
}