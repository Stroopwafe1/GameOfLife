using Controller;
using Model;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GameOfLifeGUI {

	/// <summary>
	/// Interaction logic for GameOfLifePage.xaml
	/// </summary>
	public partial class GameOfLifePage : Page {

		private Board _board;
		public GameOfLifePage(int size) {
			InitializeComponent();

			if (Data.Board != null)
				Data.Board.StateChanged -= OnStateChanged;

			Board board = new Board(size);
			_board = board;
			Data.Initialise(board);
			board.StateChanged += OnStateChanged;
		}

		public void Reset() {
			Data.Dispose();
			_board.StateChanged -= OnStateChanged;
			_board = null;
		}
		
		private void OnStateChanged(object sender, EventArgs e) {
			GOLImage.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => {
				GOLImage.Source = null;
				GOLImage.Source = Visualiser.DrawNext();
			}));
		}
	}
}