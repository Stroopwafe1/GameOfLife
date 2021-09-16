using System;
using Model;

namespace Controller {

	public static class Data {
		public static Board Board { get; private set; }
		private static Runner _runner;

		public static void Initialise(Board board) {
			if (!board.Initialised)
				board.Initialise();

			Board = board;
			board.Randomise(DateTime.Now.Millisecond);
			Runner runner = new Runner(board);

			_runner = runner;
			runner.Start();
		}

		public static void Dispose() {
			_runner.Dispose();
			_runner = null;
			Board = null;
		}
	}
}