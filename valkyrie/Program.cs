using System;

namespace valkyrie
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RPGGame game = new RPGGame())
            {
                game.Run();
            }
        }
    }
}

