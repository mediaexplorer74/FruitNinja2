using Mortar;
using System;

namespace FruitNinja2
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new TheGame())
                game.Run();
        }
    }

}
