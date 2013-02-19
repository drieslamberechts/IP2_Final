using System;

namespace XNA_GoingWild
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GoingWildDemo game = new GoingWildDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

