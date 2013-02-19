using System;

namespace XNA_LoadContent
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LoadContentDemo game = new LoadContentDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

