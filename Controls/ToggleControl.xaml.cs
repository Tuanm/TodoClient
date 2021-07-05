using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Todo.Controls {
    public partial class ToggleControl : UserControl {
        private Thickness _originMargin;

        public ToggleControl() {
            InitializeComponent();
            ConfigureStyles();
        }

        public ToggleControl WithContent(string text, bool input) {
            this.text.Text = text;
            this.input.Text = input.ToString();
            return this;
        }

        public ToggleControl WithWidth(double width) {
            if (width > 0) outter.Width = width;
            return this;
        }

        public bool GetInput() {
            return bool.Parse(input.Text);
        }

        private void ConfigureStyles() {
            containerShadow.Width = container.Width;
            containerShadow.Height = container.Height;
            _originMargin = container.Margin;
            container.Margin = containerShadow.Margin;
        }

        public ToggleControl WithBackground(System.Windows.Media.Color color) {
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
            input.Text = (!GetInput()).ToString();
        }
    }
}
