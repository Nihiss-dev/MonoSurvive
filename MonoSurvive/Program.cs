using System;

namespace MonoSurvive
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Menu menu = new Menu();
            menu.Run();
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
