using System.Diagnostics;
using System.Windows;

namespace GameOfLifeGUI {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private int size = 0;

		public MainWindow() {
			InitializeComponent();
		}

		private void mnSetup_Click(object sender, RoutedEventArgs e) {
			SetupDialog dialog = new SetupDialog();
			bool? result = dialog.ShowDialog();
			if (result.HasValue && result.Value) {
				int width = (int)dialog.slWidth.Value;
				int height = (int)dialog.slHeight.Value;
				Debug.WriteLine($"Width: {width} Height: {height}");

				Frame.Content = $"Width: {width} Height: {height}";
				size = width;
				dialog = null;
			}
		}

		private void mnRun_Click(object sender, RoutedEventArgs e) {
			if (size < 5) return;

			if (Frame.Content.GetType() == (typeof(GameOfLifePage))) {
				var prevContent = (GameOfLifePage)Frame.Content;
				prevContent.Reset();
			}
			GameOfLifePage golPage = new GameOfLifePage(size);
			Frame.Content = golPage;
		}
	}
}