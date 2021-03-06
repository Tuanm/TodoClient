using System.Windows;
using System.Windows.Controls;

namespace Todo.Controls {
    public partial class SquareControl : UserControl {
        private Thickness _originMargin;

        public SquareControl() {
            InitializeComponent();
            ConfigureStyles();
        }

        public SquareControl WithContent(string content) {
            text.Text = content;
            return this;
        }

        public SquareControl WithWidth(double width) {
            if (width > 0) container.Width = width;
            ConfigureStyles();
            return this;
        }

        private void ConfigureStyles() {
            container.Height = container.Width;
            containerShadow.Width = container.Width;
            containerShadow.Height = container.Height;
            _originMargin = container.Margin;
        }

        public SquareControl WithNoShadow() {
            _originMargin = containerShadow.Margin;
            container.Margin = _originMargin;
            return this;
        }

        public SquareControl WithBackground(System.Windows.Media.Color color) {
            container.Background = new System.Windows.Media.SolidColorBrush(color);
            return this;
        }

        public SquareControl WithAction(Utils.MouseDownAction action) {
            if (action != null) {
                container.MouseDown += new System.Windows.Input.MouseButtonEventHandler(action);
            }
            return this;
        }

        public SquareControl Disable() {
            container.Visibility = Visibility.Hidden;
            return this;
        }

        public SquareControl Enable() {
            container.Visibility = Visibility.Visible;
            return this;
        }

        private void container_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            container.Margin = containerShadow.Margin;
        }

        private void container_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            container.Margin = _originMargin;
        }
    }
}
