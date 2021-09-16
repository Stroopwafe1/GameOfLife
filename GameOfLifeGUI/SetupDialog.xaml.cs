using System.Windows;

namespace GameOfLifeGUI {

	/// <summary>
	/// Interaction logic for SetupDialog.xaml
	/// </summary>
	public partial class SetupDialog : Window {

		public SetupDialog() {
			InitializeComponent();
		}

		~SetupDialog() {
			slHeight.ValueChanged -= slHeight_ValueChanged;
			slWidth.ValueChanged -= slWidth_ValueChanged;
			okButton.Click -= okButton_Click;
		}

		private void slHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			lblHeight.Content = e.NewValue;
			if (slWidth != null)
				slWidth.Value = e.NewValue;
		}

		private void slWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			lblWidth.Content = e.NewValue;
			if (slHeight != null)
				slHeight.Value = e.NewValue;
		}

		private void okButton_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
			Close();
		}
	}
}