//using System;
//using OpenTK;
//using OpenTK.Graphics;
//using OpenTK.Graphics.OpenGL;

//namespace OpenTkClient
//{
//public class Test
//{ 
//	bool	anti=true;											// Antialiasing?
//	bool	active=true;										// Window Active Flag Set To true By Default
//	bool	fullscreen=true;									// Fullscreen Flag Set To Fullscreen Mode By Default

//	int		loop1;												// Generic Loop1
//	int		loop2;												// Generic Loop2
//	int		delay;												// Enemy Delay
//	int		adjust=3;											// Speed Adjustment For Really Slow Video Cards

//	struct	AnObject												// Create A Structure For Our Player
//	{
//		public int		fx, fy;											// Fine Movement Position
//public 		int		x, y;											// Current Player Position
//public 		float	spin;											// Spin Direction
//	};

//	AnObject player;										// Player Information
//	AnObject enemy[9];									// Enemy Information

//	GLuint	texture[2];											// Font Texture Storage Space
//	GLuint	base;												// Base Display List For The Font


//	int LoadGLTextures()										// Load Bitmaps And Convert To Textures
//	{
//		int Status=false;									// Status Indicator
//		AUX_RGBImageRec *TextureImage[2];					// Create Storage Space For The Textures
//		memset(TextureImage,0,sizeof(void *)*2);			// Set The Pointer To null

//		if ((TextureImage[0]=LoadBMP("Data/Font.bmp")) &&	// Load The Font
//			(TextureImage[1]=LoadBMP("Data/Image.bmp")))	// Load Background Image
//		{
//			Status=true;									// Set The Status To true

//			glGenTextures(2, &texture[0]);					// Create The Texture

//			for (loop1=0; loop1<2; loop1++)					// Loop Through 2 Textures
//			{
//				glBindTexture(GL_TEXTURE_2D, texture[loop1]);
//				glTexImage2D(GL_TEXTURE_2D, 0, 3, TextureImage[loop1]->sizeX, TextureImage[loop1]->sizeY, 0, GL_RGB, GL_UNSIGNED_BYTE, TextureImage[loop1]->data);
//				glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);
//				glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
//			}

//			for (loop1=0; loop1<2; loop1++)					// Loop Through 2 Textures
//			{
//				if (TextureImage[loop1])					// If Texture Exists	
//				{
//					if (TextureImage[loop1]->data)			// If Texture Image Exists
//					{
//						free(TextureImage[loop1]->data);	// Free The Texture Image Memory
//					}
//					free(TextureImage[loop1]);				// Free The Image Structure
//				}
//			}
//		}
//		return Status;											// Return The Status
//	}
//	void BuildFont()									// Build Our Font Display List
//	{
//		base=GL.GenLists(256);									// Creating 256 Display Lists
//		GL.BindTexture(GL_TEXTURE_2D, texture[0]);				// Select Our Font Texture
//		for (loop1=0; loop1<256; loop1++)						// Loop Through All 256 Lists
//		{
//			float cx=float(loop1%16)/16.0f;						// X Position Of Current Character
//			float cy=float(loop1/16)/16.0f;						// Y Position Of Current Character

//			GL.NewList(base+loop1,GL_COMPILE);					// Start Building A List
//			GL.Begin(GL_QUADS);								// Use A Quad For Each Character
//			GL.TexCoord2f(cx,1.0f-cy-0.0625f);			// Texture Coord (Bottom Left)
//			GL.Vertex2d(0,16);							// Vertex Coord (Bottom Left)
//			GL.TexCoord2f(cx+0.0625f,1.0f-cy-0.0625f);  // Texture Coord (Bottom Right)
//                GL.Vertex2i(16,16);                         // Vertex Coord (Bottom Right)
//                GL.TexCoord2f(cx+0.0625f,1.0f-cy);          // Texture Coord (Top Right)
//                GL.Vertex2i(16,0);                          // Vertex Coord (Top Right)
//                GL.TexCoord2f(cx,1.0f-cy);                  // Texture Coord (Top Left)
//                GL.Vertex2i(0,0);                           // Vertex Coord (Top Left)
//                GL.End();                                       // Done Building Our Quad (Character)
//                GL.Translated(15,0,0);                          // Move To The Right Of The Character
//                GL.EndList();										// Done Building The Display List
//		}														// Loop Until All 256 Are Built
//	}

//	void KillFont()										// Delete The Font From Memory
//	{
//            GL.DeleteLists(base,256);								// Delete All 256 Display Lists
//	}

//	void GLPrint(GLint x, GLint y, int set, const char *fmt, ...)	// Where The Printing Happens
//	{
//		char		text[256];									// Holds Our String
//		va_list		ap;											// Pointer To List Of Arguments

//		if (fmt == null)										// If There's No Text
//			return;												// Do Nothing

//		va_start(ap, fmt);										// Parses The String For Variables
//		vsprintf(text, fmt, ap);							// And Converts Symbols To Actual Numbers
//		va_end(ap);												// Results Are Stored In Text

//		if (set>1)												// Did User Choose An Invalid Character Set?
//		{
//			set=1;												// If So, Select Set 1 (Italic)
//		}
//    GL.Enable(GL_TEXTURE_2D);								// Enable Texture Mapping
//		GL.LoadIdentity();										// Reset The Modelview Matrix
//		GL.Translated(x,y,0);									// Position The Text (0,0 - Bottom Left)
//		GL.ListBase(base-32+(128*set));							// Choose The Font Set (0 or 1)

//		if (set==0)												// If Set 0 Is Being Used Enlarge Font
//		{
//			glScalef(1.5f,2.0f,1.0f);							// Enlarge Font Width And Height
//		}

//GL.CallLists(strlen(text),GL_UNSIGNED_BYTE, text);		// Write The Text To The Screen
//		GL.Disable(GL_TEXTURE_2D);								// Disable Texture Mapping
//	}

//	void ReSizeGLScene(GLsizei width, GLsizei height)			// Resize And Initialize The GL Window
//	{
//		if (height==0)											// Prevent A Divide By Zero By
//		{
//			height=1;											// Making Height Equal One
//		}

//		GL.Viewport(0,0,width,height);							// Reset The Current Viewport

//		GL.MatrixMode(GL_PROJECTION);                           // Select The Projection Matrix
//    GL.LoadIdentity();                                      // Reset The Projection Matrix

//    GL.Ortho(0.0f,width,height,0.0f,-1.0f,1.0f);                // Create Ortho 640x480 View (0,0 At Top Left)

//    GL.MatrixMode(GL_MODELVIEW);                                // Select The Modelview Matrix
//    GL.LoadIdentity();										// Reset The Modelview Matrix
//	}

//	int InitGL()											// All Setup For OpenGL Goes Here
//	{
//		if (!LoadGLTextures())									// Jump To Texture Loading Routine
//		{
//			return false;										// If Texture Didn't Load Return false
//		}

//		BuildFont();											// Build The Font

//		GL.ShadeModel(GL_SMOOTH);                               // Enable Smooth Shading
//    GL.ClearColor(0.0f, 0.0f, 0.0f, 0.5f);                  // Black Background
//    GL.ClearDepth(1.0f);                                        // Depth Buffer Setup
//    GL.Hint(GL_LINE_SMOOTH_HINT, GL_NICEST);                    // Set Line Antialiasing
//    GL.Enable(GL_BLEND);                                        // Enable Blending
//    GL.BlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);		// Type Of Blending To Use
//		return true;											// Initialization Went OK
//	}

//	int DrawGLScene()										// Here's Where We Do All The Drawing
//	{
//    GL.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);        // Clear Screen And Depth Buffer
//    GL.BindTexture(GL_TEXTURE_2D, texture[0]);              // Select Our Font Texture
//    GL.Color3f(1.0f,0.5f,1.0f);                             // Set Color To Purple
//    GLPrint(207,24,0,"GRID CRAZY");                            // Write GRID CRAZY On The Screen
//    GL.Color3f(1.0f,1.0f,0.0f);                             // Set Color To Yellow
//    GLPrint(20,20,1,"Level:%2i",level2);                   // Write Actual Level Stats
//    GLPrint(20,40,1,"Stage:%2i",stage);						// Write Stage Stats

//		if (gameover)											// Is The Game Over?
//		{
//        GL.Color3ub(rand()%255,rand()%255,rand()%255);      // Pick A Random Color
//        GLPrint(472,20,1,"GAME OVER");                     // Write GAME OVER To The Screen
//        GLPrint(456,40,1,"PRESS SPACE");					// Write PRESS SPACE To The Screen
//		}

//		for (loop1=0; loop1<lives-1; loop1++)					// Loop Through Lives Minus Current Life
//		{
//        GL.LoadIdentity();                                  // Reset The View
//        GL.Translatef(490+(loop1*40.0f),40.0f,0.0f);            // Move To The Right Of Our Title Text
//        GL.Rotatef(-player.spin,0.0f,0.0f,1.0f);                // Rotate Counter Clockwise
//        GL.Color3f(0.0f,1.0f,0.0f);                         // Set Player Color To Light Green
//        GL.Begin(GL_LINES);                                 // Start Drawing Our Player Using Lines
//        GL.Vertex2d(-5,-5);                             // Top Left Of Player
//        GL.Vertex2d( 5, 5);								// Bottom Right Of Player
//			GL.Vertex2d( 5,-5);                             // Top Right Of Player
//        GL.Vertex2d(-5, 5);                             // Bottom Left Of Player
//        GL.End();                                           // Done Drawing The Player
//        GL.Rotatef(-player.spin*0.5f,0.0f,0.0f,1.0f);       // Rotate Counter Clockwise
//        GL.Color3f(0.0f,0.75f,0.0f);                            // Set Player Color To Dark Green
//        GL.Begin(GL_LINES);                                 // Start Drawing Our Player Using Lines
//        GL.Vertex2d(-7, 0);                             // Left Center Of Player
//        GL.Vertex2d( 7, 0);                             // Right Center Of Player
//        GL.Vertex2d( 0,-7);                             // Top Center Of Player
//        GL.Vertex2d( 0, 7);                             // Bottom Center Of Player
//        GL.End();											// Done Drawing The Player
//		}

//		filled=true;                                            // Set Filled To True Before Testing
//    GL.LineWidth(2.0f);                                     // Set Line Width For Cells To 2.0f
//    GL.Disable(GL_LINE_SMOOTH);                             // Disable Antialiasing
//    GL.LoadIdentity();										// Reset The Current Modelview Matrix
//		for (loop1=0; loop1<11; loop1++)						// Loop From Left To Right
//		{
//			for (loop2=0; loop2<11; loop2++)					// Loop From Top To Bottom
//			{
//            GL.Color3f(0.0f,0.5f,1.0f);						// Set Line Color To Blue
//				if (hline[loop1][loop2])						// Has The Horizontal Line Been Traced
//				{
//                GL.Color3f(1.0f,1.0f,1.0f);					// If So, Set Line Color To White
//				}

//				if (loop1<10)									// Dont Draw To Far Right
//				{
//					if (!hline[loop1][loop2])					// If A Horizontal Line Isn't Filled
//					{
//						filled=false;							// filled Becomes False
//					}
//                GL.Begin(GL_LINES);                         // Start Drawing Horizontal Cell Borders
//                GL.Vertex2d(20+(loop1*60),70+(loop2*40));// Left Side Of Horizontal Line
//                GL.Vertex2d(80+(loop1*60),70+(loop2*40));// Right Side Of Horizontal Line
//                GL.End();									// Done Drawing Horizontal Cell Borders
//				}

//            GL.Color3f(0.0f,0.5f,1.0f);						// Set Line Color To Blue
//				if (vline[loop1][loop2])						// Has The Horizontal Line Been Traced
//				{
//                GL.Color3f(1.0f,1.0f,1.0f);					// If So, Set Line Color To White
//				}
//				if (loop2<10)									// Dont Draw To Far Down
//				{
//					if (!vline[loop1][loop2])					// If A Verticle Line Isn't Filled
//					{
//						filled=false;							// filled Becomes False
//					}
//                GL.Begin(GL_LINES);                         // Start Drawing Verticle Cell Borders
//                GL.Vertex2d(20+(loop1*60),70+(loop2*40));// Left Side Of Horizontal Line
//                GL.Vertex2d(20+(loop1*60),110+(loop2*40));// Right Side Of Horizontal Line
//                GL.End();									// Done Drawing Verticle Cell Borders
//				}

//            GL.Enable(GL_TEXTURE_2D);                       // Enable Texture Mapping
//            GL.Color3f(1.0f,1.0f,1.0f);                     // Bright White Color
//            GL.BindTexture(GL_TEXTURE_2D, texture[1]);		// Select The Tile Image
//				if ((loop1<10) && (loop2<10))					// If In Bounds, Fill In Traced Boxes
//				{
//					// Are All Sides Of The Box Traced?
//					if (hline[loop1][loop2] && hline[loop1][loop2+1] && vline[loop1][loop2] && vline[loop1+1][loop2])
//					{
//                    GL.Begin(GL_QUADS);                     // Draw A Textured Quad
//                    GL.TexCoord2f(float(loop1/10.0f)+0.1f,1.0f-(float(loop2/10.0f)));
//                    GL.Vertex2d(20+(loop1*60)+59,(70+loop2*40+1));  // Top Right
//                    GL.TexCoord2f(float(loop1/10.0f),1.0f-(float(loop2/10.0f)));
//                    GL.Vertex2d(20+(loop1*60)+1,(70+loop2*40+1));   // Top Left
//                    GL.TexCoord2f(float(loop1/10.0f),1.0f-(float(loop2/10.0f)+0.1f));
//                    GL.Vertex2d(20+(loop1*60)+1,(70+loop2*40)+39);  // Bottom Left
//                    GL.TexCoord2f(float(loop1/10.0f)+0.1f,1.0f-(float(loop2/10.0f)+0.1f));
//                    GL.Vertex2d(20+(loop1*60)+59,(70+loop2*40)+39); // Bottom Right
//                    GL.End();								// Done Texturing The Box
//					}
//				}
//            GL.Disable(GL_TEXTURE_2D);						// Disable Texture Mapping
//			}
//		}
//    GL.LineWidth(1.0f);										// Set The Line Width To 1.0f

//		if (anti)												// Is Anti true?
//		{
//        GL.Enable(GL_LINE_SMOOTH);							// If So, Enable Antialiasing
//		}

//		//if (hourglass.fx==1)									// If fx=1 Draw The Hourglass
//		//{
//  //      GL.LoadIdentity();                                  // Reset The Modelview Matrix
//  //      GL.Translatef(20.0f+(hourglass.x*60),70.0f+(hourglass.y*40),0.0f);  // Move To The Fine Hourglass Position
//  //      GL.Rotatef(hourglass.spin,0.0f,0.0f,1.0f);          // Rotate Clockwise
//  //      GL.Color3ub(rand()%255,rand()%255,rand()%255);      // Set Hourglass Color To Random Color
//  //      GL.Begin(GL_LINES);                                 // Start Drawing Our Hourglass Using Lines
//  //      GL.Vertex2d(-5,-5);                             // Top Left Of Hourglass
//  //      GL.Vertex2d( 5, 5);                             // Bottom Right Of Hourglass
//  //      GL.Vertex2d( 5,-5);                             // Top Right Of Hourglass
//  //      GL.Vertex2d(-5, 5);                             // Bottom Left Of Hourglass
//  //      GL.Vertex2d(-5, 5);                             // Bottom Left Of Hourglass
//  //      GL.Vertex2d( 5, 5);                             // Bottom Right Of Hourglass
//  //      GL.Vertex2d(-5,-5);                             // Top Left Of Hourglass
//  //      GL.Vertex2d( 5,-5);                             // Top Right Of Hourglass
//  //      GL.End();											// Done Drawing The Hourglass
//		//}

//    GL.LoadIdentity();                                      // Reset The Modelview Matrix
//    GL.Translatef(player.fx+20.0f,player.fy+70.0f,0.0f);        // Move To The Fine Player Position
//    GL.Rotatef(player.spin,0.0f,0.0f,1.0f);                 // Rotate Clockwise
//    GL.Color3f(0.0f,1.0f,0.0f);                             // Set Player Color To Light Green
//    GL.Begin(GL_LINES);                                     // Start Drawing Our Player Using Lines
//    GL.Vertex2d(-5,-5);                                 // Top Left Of Player
//    GL.Vertex2d( 5, 5);                                 // Bottom Right Of Player
//    GL.Vertex2d( 5,-5);                                 // Top Right Of Player
//    GL.Vertex2d(-5, 5);                                 // Bottom Left Of Player
//    GL.End();                                               // Done Drawing The Player
//    GL.Rotatef(player.spin*0.5f,0.0f,0.0f,1.0f);                // Rotate Clockwise
//    GL.Color3f(0.0f,0.75f,0.0f);                                // Set Player Color To Dark Green
//    GL.Begin(GL_LINES);                                     // Start Drawing Our Player Using Lines
//    GL.Vertex2d(-7, 0);                                 // Left Center Of Player
//    GL.Vertex2d( 7, 0);                                 // Right Center Of Player
//    GL.Vertex2d( 0,-7);                                 // Top Center Of Player
//    GL.Vertex2d( 0, 7);                                 // Bottom Center Of Player
//    GL.End();												// Done Drawing The Player

//		for (loop1=0; loop1<(stage*level); loop1++)				// Loop To Draw Enemies
//		{
//        GL.LoadIdentity();                                  // Reset The Modelview Matrix
//        GL.Translatef(enemy[loop1].fx+20.0f,enemy[loop1].fy+70.0f,0.0f);
//        GL.Color3f(1.0f,0.5f,0.5f);                         // Make Enemy Body Pink
//        GL.Begin(GL_LINES);                                 // Start Drawing Enemy
//        GL.Vertex2d( 0,-7);                             // Top Point Of Body
//        GL.Vertex2d(-7, 0);                             // Left Point Of Body
//        GL.Vertex2d(-7, 0);                             // Left Point Of Body
//        GL.Vertex2d( 0, 7);                             // Bottom Point Of Body
//        GL.Vertex2d( 0, 7);                             // Bottom Point Of Body
//        GL.Vertex2d( 7, 0);                             // Right Point Of Body
//        GL.Vertex2d( 7, 0);                             // Right Point Of Body
//        GL.Vertex2d( 0,-7);                             // Top Point Of Body
//        GL.End();                                           // Done Drawing Enemy Body
//        GL.Rotatef(enemy[loop1].spin,0.0f,0.0f,1.0f);       // Rotate The Enemy Blade
//        GL.Color3f(1.0f,0.0f,0.0f);                         // Make Enemy Blade Red
//        GL.Begin(GL_LINES);                                 // Start Drawing Enemy Blade
//        GL.Vertex2d(-7,-7);                             // Top Left Of Enemy
//        GL.Vertex2d( 7, 7);                             // Bottom Right Of Enemy
//        GL.Vertex2d(-7, 7);                             // Bottom Left Of Enemy
//        GL.Vertex2d( 7,-7);                             // Top Right Of Enemy
//        GL.End();											// Done Drawing Enemy Blade
//		}
//		return true;											// Everything Went OK
//	}

///*	This Code Creates Our OpenGL Window.  Parameters Are:					*
// *	title			- Title To Appear At The Top Of The Window				*
// *	width			- Width Of The GL Window Or Fullscreen Mode				*
// *	height			- Height Of The GL Window Or Fullscreen Mode			*
// *	bits			- Number Of Bits To Use For Color (8/16/24/32)			*
// *	fullscreenflag	- Use Fullscreen Mode (true) Or Windowed Mode (false)	*/

//	bool CreateGLWindow(char* title, int width, int height, int bits, bool fullscreenflag)
//	{
//		GLuint		PixelFormat;									// Holds The Results After Searching For A Match
//		WNDCLASS	wc;												// Windows Class Structure
//		DWORD		dwExStyle;										// Window Extended Style
//		DWORD		dwStyle;										// Window Style
//		RECT		WindowRect;										// Grabs Rectangle Upper Left / Lower Right Values
//		WindowRect.left=(long)0;									// Set Left Value To 0
//		WindowRect.right=(long)width;								// Set Right Value To Requested Width
//		WindowRect.top=(long)0;										// Set Top Value To 0
//		WindowRect.bottom=(long)height;								// Set Bottom Value To Requested Height

//		fullscreen=fullscreenflag;									// Set The Global Fullscreen Flag

//		wc.style			= CS_HREDRAW | CS_VREDRAW | CS_OWNDC;	// Redraw On Size, And Own DC For Window
//		wc.lpfnWndProc		= (WNDPROC) WndProc;					// WndProc Handles Messages
//		wc.cbClsExtra		= 0;									// No Extra Window Data
//		wc.cbWndExtra		= 0;									// No Extra Window Data
//		wc.hInstance		= hInstance;							// Set The Instance
//		wc.hIcon			= LoadIcon(null, IDI_WINLOGO);			// Load The Default Icon
//		wc.hCursor			= LoadCursor(null, IDC_ARROW);			// Load The Arrow Pointer
//		wc.hbrBackground	= null;									// No Background Required For GL
//		wc.lpszMenuName		= null;									// We Don't Want A Menu
//		wc.lpszClassName	= "OpenGL";								// Set The Class Name

//		if (!RegisterClass(&wc))									// Attempt To Register The Window Class
//		{
//			Console.WriteLine("ERROR - Failed To Register The Window Class.");
//			return false;											// Return false
//		}

//		if (fullscreen)												// Attempt Fullscreen Mode?
//		{
//			DEVMODE dmScreenSettings;								// Device Mode
//			memset(&dmScreenSettings,0,sizeof(dmScreenSettings));	// Makes Sure Memory's Cleared
//			dmScreenSettings.dmSize=sizeof(dmScreenSettings);		// Size Of The Devmode Structure
//			dmScreenSettings.dmPelsWidth	= width;				// Selected Screen Width
//			dmScreenSettings.dmPelsHeight	= height;				// Selected Screen Height
//			dmScreenSettings.dmBitsPerPel	= bits;					// Selected Bits Per Pixel
//			dmScreenSettings.dmFields=DM_BITSPERPEL|DM_PELSWIDTH|DM_PELSHEIGHT;

//			// Try To Set Selected Mode And Get Results.  NOTE: CDS_FULLSCREEN Gets Rid Of Start Bar.
//			if (ChangeDisplaySettings(&dmScreenSettings,CDS_FULLSCREEN)!=DISP_CHANGE_SUCCESSFUL)
//			{
//            // If The Mode Fails, Offer Two Options.  Quit Or Use Windowed Mode.
//            Console.WriteLine("NeHe GL - The Requested Fullscreen Mode Is Not Supported By\nYour Video Card. Use Windowed Mode Instead?", "NeHe GL");
//    		fullscreen=false;								// Windowed Mode Selected.  Fullscreen = false
//			}
//		}

//		if (fullscreen)												// Are We Still In Fullscreen Mode?
//		{
//			dwExStyle=WS_EX_APPWINDOW;								// Window Extended Style
//			dwStyle=WS_POPUP;										// Windows Style
//			ShowCursor(false);										// Hide Mouse Pointer
//		}
//		else
//		{
//			dwExStyle=WS_EX_APPWINDOW | WS_EX_WINDOWEDGE;			// Window Extended Style
//			dwStyle=WS_OVERLAPPEDWINDOW;							// Windows Style
//		}

//		AdjustWindowRectEx(&WindowRect, dwStyle, false, dwExStyle);	// Adjust Window To True Requested Size

//		// Create The Window
//		if (!(hWnd=CreateWindowEx(	dwExStyle,						// Extended Style For The Window
//			"OpenGL",						// Class Name
//			title,							// Window Title
//			dwStyle |						// Defined Window Style
//			WS_CLIPSIBLINGS |				// Required Window Style
//			WS_CLIPCHILDREN,				// Required Window Style
//			0, 0,							// Window Position
//			WindowRect.right-WindowRect.left,	// Calculate Window Width
//			WindowRect.bottom-WindowRect.top,	// Calculate Window Height
//			null,							// No Parent Window
//			null,							// No Menu
//			hInstance,						// Instance
//			null)))							// Dont Pass Anything To WM_CREATE
//		{
//			KillGLWindow();                                         // Reset The Display
//        Console.WriteLine("Error - Window Creation Error.");
//			return false;											// Return false
//		}

//		static	PIXELFORMATDESCRIPTOR pfd=							// pfd Tells Windows How We Want Things To Be
//		{
//			sizeof(PIXELFORMATDESCRIPTOR),							// Size Of This Pixel Format Descriptor
//			1,														// Version Number
//			PFD_DRAW_TO_WINDOW |									// Format Must Support Window
//			PFD_SUPPORT_OPENGL |									// Format Must Support OpenGL
//			PFD_DOUBLEBUFFER,										// Must Support Double Buffering
//			PFD_TYPE_RGBA,											// Request An RGBA Format
//			bits,													// Select Our Color Depth
//			0, 0, 0, 0, 0, 0,										// Color Bits Ignored
//			0,														// No Alpha Buffer
//			0,														// Shift Bit Ignored
//			0,														// No Accumulation Buffer
//			0, 0, 0, 0,												// Accumulation Bits Ignored
//			16,														// 16Bit Z-Buffer (Depth Buffer)  
//			0,														// No Stencil Buffer
//			0,														// No Auxiliary Buffer
//			PFD_MAIN_PLANE,											// Main Drawing Layer
//			0,														// Reserved
//			0, 0, 0													// Layer Masks Ignored
//		};

//		if (!(hDC=GetDC(hWnd)))										// Did We Get A Device Context?
//		{
//			KillGLWindow();                                         // Reset The Display
//        Console.WriteLine("ERROR - Can't Create A GL Device Context.");
//			return false;											// Return false
//		}

//		if (!(PixelFormat=ChoosePixelFormat(hDC,&pfd)))				// Did Windows Find A Matching Pixel Format?
//		{
//			KillGLWindow();                                         // Reset The Display
//        Console.WriteLine("ERROR - Can't Find A Suitable PixelFormat.");
//			return false;											// Return false
//		}

//		if(!SetPixelFormat(hDC,PixelFormat,&pfd))					// Are We Able To Set The Pixel Format?
//		{
//			KillGLWindow();											// Reset The Display
//			Console.WriteLine("ERROR - Can't Set The PixelFormat.");
//        return false;											// Return false
//		}

//		if (!(hRC=wglCreateContext(hDC)))							// Are We Able To Get A Rendering Context?
//		{
//			KillGLWindow();                                         // Reset The Display
//        Console.WriteLine("ERROR - Can't Create A GL Rendering Context.");

//        return false;											// Return false
//		}	

//		if(!wglMakeCurrent(hDC,hRC))								// Try To Activate The Rendering Context
//		{
//			KillGLWindow();                                         // Reset The Display
//        Console.WriteLine("ERROR - Can't Activate The GL Rendering Context.");

//        return false;											// Return false
//		}

//		ShowWindow(hWnd,SW_SHOW);									// Show The Window
//		SetForegroundWindow(hWnd);									// Slightly Higher Priority
//		SetFocus(hWnd);												// Sets Keyboard Focus To The Window
//		ReSizeGLScene(width, height);								// Set Up Our Perspective GL Screen

//		if (!InitGL())												// Initialize Our Newly Created GL Window
//		{
//			KillGLWindow();                                         // Reset The Display
//        Console.WriteLine("ERROR - Initialization Failed.");
//        return false;											// Return false
//		}

//		return true;												// Success
//	}


//}
//}

