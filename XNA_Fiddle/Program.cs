using System;

namespace XNA_Fiddle
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FiddleDemo game = new FiddleDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

