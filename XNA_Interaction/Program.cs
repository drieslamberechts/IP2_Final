using System;

namespace XNA_Interaction
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (InteractionDemo game = new InteractionDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

