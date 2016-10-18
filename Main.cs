﻿using System;
namespace OpenTkClient
{
    public static class MainApp
    {
        public static void Main()
        {
            using (GameRenderer example = new GameRenderer())
            {
                //Utilities.SetWindowTitle(example);
                Comms.Run();
                example.Run(30.0);
            }
        }

    }
}

