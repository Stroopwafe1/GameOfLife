using Model;
using System.Timers;

namespace Controller {

	public class Runner {
		private Board _board;
		private Timer _timer;

		public Runner(Board board) {
			_board = board;
			_timer = new Timer(50);
			_timer.Elapsed += OnTimedEvent;
		}

		public void Start() {
			_timer.Start();
		}

		public void Dispose() {
			_timer.Stop();
			_timer.Elapsed -= OnTimedEvent;
			_board = null;
			_timer = null;
		}

		private void OnTimedEvent(object sender, ElapsedEventArgs e) {
			_board.NextState();
		}
	}
}