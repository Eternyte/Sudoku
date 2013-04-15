using System;

namespace Game {
#if WINDOWS
    static class Program {
        /// The main entry point for the application.
        static void Main(string[] args) {
            using (Game game = new Game()) {
                game.Run();
            }
        }
    }
#endif
}

