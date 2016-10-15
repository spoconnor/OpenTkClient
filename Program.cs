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
using Sean.Shared;
using OpenTK.Input;

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
            //GL.MatrixMode(MatrixMode.Projection);        // Select the Projection matrix for operation
            //GL.LoadIdentity();                           // Reset Projection matrix
            //GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0); // Set clipping area's left, right, bottom, top
            //GL.Ortho(0, 800, 600, 0, -1, 1);             // Set clipping area's left, right, bottom, top
            //GL.MatrixMode(MatrixMode.Modelview);         // Select the ModelView for operation
            //GL.LoadIdentity();                           // Reset the Model View Matrix

            //background color
            GL.ClearColor(Color.CornflowerBlue);
            //GL.Ortho(0, 800, 600, 0, -1, 1);
            //GL.Viewport(0, 0, 800, 600);

            //set the view area
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(-2, 2, -2, 2, -2, 2);

            //now back to 'scene editing' mode
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            //make things look nice
            GL.ShadeModel(ShadingModel.Smooth);

            //set up our z-rendering logic
            //GL.ClearDepth(1.0f);
            GL.Disable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Lequal);
            //GL.DepthMask(true);
            //GL.ClearColor(Color.Black);

            //other improvements to quality
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

            //initialize our scene data
            //CreateVertexBuffer();

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

        //void CreateVertexBuffer()
        //{
        //    vertices = new Vector3[2, 3];
        //    vertices[0, 0] = new Vector3(-1f, -1f, (float)Math.Sin(time));
        //    vertices[0, 1] = new Vector3(0.5f, -1f, (float)Math.Sin(time));
        //    vertices[0, 2] = new Vector3(-0.25f, 1f, -(float)Math.Sin(time));
        //    vertices[1, 0] = new Vector3(-0.5f, -1f, (float)Math.Cos(time));
        //    vertices[1, 1] = new Vector3(1f, -1f, (float)Math.Cos(time));
        //    vertices[1, 2] = new Vector3(0.25f, 1f, -(float)Math.Cos(time));

        //    //MessageBox.Show("Length: " + vertices.Length.ToString());

        //    GL.GenBuffers(1, out vbo);
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        //    GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
        //                           new IntPtr(vertices.Length * Vector3.SizeInBytes),
        //                           vertices, BufferUsageHint.StaticDraw);
        //}

        private int LoadTexture()
		{
			int texture;
			Bitmap bitmap = new Bitmap("block.png");

			GL.GenTextures(1, out texture);
			GL.BindTexture(TextureTarget.Texture2D, texture);

			//bitmap.MakeTransparent(Color.Black);

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

			KeyboardState keyState = Keyboard.GetState();

			if (keyState.IsKeyDown (Key.A)) {
				Global.LookingAt.X--;
			} else if (keyState.IsKeyDown (Key.D)) {
				Global.LookingAt.X++;
			} else if (keyState.IsKeyDown (Key.W)) {
				Global.LookingAt.Z++;
			} else if (keyState.IsKeyDown (Key.S)) {
				Global.LookingAt.Z--;
			}
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{            
            //RenderGui();
            GL.MatrixMode(MatrixMode.Projection);        // Select the Projection matrix for operation
            GL.LoadIdentity();                           // Reset Projection matrix
            //GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0); // Set clipping area's left, right, bottom, top
			GL.Ortho(0, this.Width * Global.Scale, 0, this.Height * Global.Scale, -1.0, 1.0);             // Set clipping area's left, right, bottom, top
            GL.MatrixMode(MatrixMode.Modelview);         // Select the ModelView for operation
            GL.LoadIdentity();                           // Reset the Model View Matrix

            //GL.Enable(EnableCap.Lighting);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //RenderBlock((float)e.Time, new Block(Block.BlockType.Dirt), 100,100,0.1f);
			int midWidth = (int)(this.Width * Global.Scale / 2);
			int midHeight = (int)(this.Height * Global.Scale / 2);
            int sprXOffset = 32;
            int sprYOffset = 15;
            int sprHeight = 21;
            foreach (var blockInfo in MapManager.GetBlocks())
            {
				var pos = blockInfo.Item1;
				var blockType = blockInfo.Item2;
					
				float x1 = pos.X - Global.LookingAt.X;
				float y1 = pos.Y - Global.LookingAt.Y;
				float z1 = pos.Z - Global.LookingAt.Z;

                //if (y1 > 1 || x1 > 5 || z1 > 5) continue;

				var x2 = midWidth + (x1 - z1) * sprXOffset;
				var y2 = midHeight + (y1 * sprHeight) + (x1 + z1) * sprYOffset;
				var z2 = 0.0f;//(y1 + (x1 + z1) * 128.0f) / (64.0f * 128.0f);

                //Console.WriteLine($"{x1},{y1},{z1}=>{x2},{y2},{z2}");
                RenderBlock((float)e.Time, blockType, x2, y2, z2);
            }
			SwapBuffers();
		}

		void RenderBlock(float time, Block.BlockType blockType, float x, float y, float z)
        {
            GL.PushMatrix();
            GL.Translate(x,y,z);
            GL.CallList(boxListIndex);
            GL.PopMatrix();
        }

		int CompileBox()
        {
            int newList = GL.GenLists(1);
            GL.NewList(newList, ListMode.Compile);

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BindTexture(TextureTarget.Texture2D, blockTexture);

            //GL.Scale (0.1, 0.1, 1.0);
            float sprSize = 32;
			GL.Begin (PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-sprSize, -sprSize);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(sprSize, -sprSize);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(sprSize, sprSize);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-sprSize, sprSize);
            GL.End ();

            GL.EndList();
            return newList;
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