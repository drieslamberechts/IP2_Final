using System;

namespace XNA_Movement
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MovementDemo game = new MovementDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

