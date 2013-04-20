using System;

namespace Sudoku {
#if WINDOWS
    static class Program {
        /// The main entry point for the application.
        static void Main(string[] args) {
            using (Sudoku game = new Sudoku()) {
                game.Run();
            }
        }
    }
#endif
}

