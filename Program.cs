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
using System.Drawing.Imaging;

namespace OpenTkClient
{
	class TextRendering : GameWindow
	{
		TextRenderer renderer;
		Font serif = new Font(FontFamily.GenericSerif, 24);
		Font sans = new Font(FontFamily.GenericSansSerif, 24);
		Font mono = new Font(FontFamily.GenericMonospace, 24);
		float angle;
		int blockTexture;
        int boxListIndex;

		public TextRendering()
			: base(800, 600)
		{
		}

		protected override void OnLoad(EventArgs e)
		{

            GL.ClearColor(Color.CornflowerBlue);
            GL.Ortho(0, 800, 600, 0, -1, 1);
            GL.Viewport(0, 0, 800, 600);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);


			blockTexture = LoadTexture();
			renderer = new TextRenderer(Width, Height);
			PointF position = PointF.Empty;

			renderer.Clear(Color.Transparent);
			renderer.DrawString("The quick brown fox jumps over the lazy dog", serif, Brushes.White, position);
			position.Y += serif.Height;
			renderer.DrawString("The quick brown fox jumps over the lazy dog", sans, Brushes.White, position);
			position.Y += sans.Height;
			renderer.DrawString("The quick brown fox jumps over the lazy dog", mono, Brushes.White, position);
			position.Y += mono.Height;

            boxListIndex = CompileBox();
        }


		private int LoadTexture()
		{
			int texture;
			Bitmap bitmap = new Bitmap("block.png");

			GL.GenTextures(1, out texture);
			GL.BindTexture(TextureTarget.Texture2D, texture);

            bitmap.MakeTransparent(Color.Magenta);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                              ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			return texture;
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

			//RenderCube((float)e.Time);
			RenderGui();
			RenderBox ((float)e.Time);
			//RenderBox2 ();
			SwapBuffers();
		}


		// draw diamond and rectangle
		void RenderBox2() {
			GL.Color3(Color.Blue);
			GL.Begin(PrimitiveType.Polygon);
			GL.Vertex2(0.90, 0.50);
			GL.Vertex2(0.50, 0.90);
			GL.Vertex2(0.10, 0.50);
			GL.Vertex2(0.50, 0.10);
			GL.End();
			GL.Color3(Color.White);
			GL.Rect(0.25, 0.25, 0.75, 0.75);
		}

        void RenderBox(float time)
        {
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			//GL.Enable(EnableCap.Lighting);
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);

			GL.PushMatrix();
			GL.Translate (-0.5, -0.5, 0);
            GL.CallList(boxListIndex);
            GL.PopMatrix ();

			GL.PushMatrix();
			GL.Translate (-0.2, -0.3, 0);
            GL.CallList(boxListIndex);
            GL.PopMatrix ();

			GL.PushMatrix();
			GL.Translate (-0.6, -0.5, 0);
            GL.CallList(boxListIndex);
            GL.PopMatrix ();
        }
		int CompileBox()
        {
            int newList = GL.GenLists(1);
            GL.NewList(newList, ListMode.Compile);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BindTexture(TextureTarget.Texture2D, blockTexture);

			GL.Scale (0.1, 0.1, 0.1);

			//GL.Color3(Color.Red);
			GL.Begin (PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1f, -1f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1f, -1f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1f, 1f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1f, 1f);
            GL.End ();

            GL.EndList();
            return newList;
        }

        void RenderCube(float time)
		{
            GL.LoadIdentity();
            GL.Ortho(0.0f, Width, Height, 0.0f, -1.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

   //         Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, Width / (float)Height, 1.0f, 128.0f);
			//Matrix4 modelview = Matrix4.LookAt(0, 3, 3, 0, 0, 0, 0, 1, 0);

			//GL.MatrixMode(MatrixMode.Projection);
   //         GL.MatrixMode(MatrixMode.Modelview);
			//GL.LoadMatrix(ref projection);

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


	}
}