using System;

namespace ValkyrieLibrary
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PokeGame game = new PokeGame())
            {
                game.Run();
            }
        }
    }
}

