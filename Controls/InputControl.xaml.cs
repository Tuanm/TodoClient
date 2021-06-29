using System.Windows;
using System.Windows.Controls;

namespace Todo.Controls {
    public partial class InputControl : UserControl {
        private Thickness _originMargin;

        public InputControl() {
            InitializeComponent();
            ConfigureStyles();
        }

        public InputControl WithContent(string text, string input) {
            this.text.Text = text;
            this.input.Text = input;
            return this;
        }

        public string GetInput() {
            return input.Text;
        }

        private void ConfigureStyles() {
            containerShadow.Width = container.Width;
            containerShadow.Height = container.Height;
            _originMargin = container.Margin;
            container.Margin = containerShadow.Margin;
        }

        public InputControl WithBackground(System.Windows.Media.Color color) {
            container.Background = new System.Windows.Media.SolidColorBrush(color);
            return this;
        }

        private void container_GotFocus(object sender, RoutedEventArgs e) {
            container.Margin = _originMargin;
        }

        private void container_LostFocus(object sender, RoutedEventArgs e) {
            container.Margin = containerShadow.Margin;
        }

        private void container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            input.Focus();
        }
    }
}
