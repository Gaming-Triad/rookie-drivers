using System;


namespace RookieDrivers
{
#if WINDOWS || XBOX
    static class Launcher
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameMain game = new GameMain())
            {
                game.Run();
            }
        }
    }
#endif
}

