using System;
namespace OpenTkClient
{
    public static class MainApp
    {
        public static void Main()
        {
            using (TextRendering example = new TextRendering())
            {
                //Utilities.SetWindowTitle(example);
                Comms.Run();
                example.Run(30.0);
            }
        }

    }
}

