using System.Windows;
using System.Windows.Controls;

namespace Todo.Controls {
    public enum InputType {
        None, Text, Number, DateTime, Date, Time, Email, YesNo
    }

    public partial class InputControl : UserControl {
        private Thickness _originMargin;
        private System.Windows.Media.Brush _originBackground;

        public InputType InputType { get; set; }

        public InputControl() {
            InitializeComponent();
            ConfigureStyles();
        }

        public InputControl WithContent(string text, object input) {
            this.text.Text = text;
            this.input.Text = input != null ? input.ToString() : string.Empty;
            if (input == null) InputType = InputType.None;
            else if (input is bool) InputType = InputType.YesNo;
            else if (input is byte | input is int | input is float | input is double) InputType = InputType.Number;
            else if (input is System.DateTime) InputType = InputType.DateTime;
            else InputType = InputType.Text;
            return this;
        }

        public InputControl WithInputType(InputType inputType) {
            InputType = inputType;
            return this;
        }

        public InputControl WithWidth(double width) {
            if (width > 0) outter.Width = width;
            return this;
        }

        public string GetInput() {
            return input.Text;
        }

        private void ConfigureStyles() {
            containerShadow.Width = container.Width;
            containerShadow.Height = container.Height;
            _originMargin = container.Margin;
            _originBackground = container.Background;
            input.Background = container.Background;
            container.Margin = containerShadow.Margin;
        }

        public InputControl WithBackground(System.Windows.Media.Color color) {
            container.Background = new System.Windows.Media.SolidColorBrush(color);
            _originBackground = container.Background;
            input.Background = container.Background;
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

        public static bool IsInputValid(string input, InputType inputType) {
            switch (inputType) {
                case InputType.Number:
                    return double.TryParse(input, out double number);
                case InputType.YesNo:
                    return bool.TryParse(input, out bool isTrue);
                case InputType.DateTime:
                    return System.DateTime.TryParse(input, out System.DateTime dateTime);
            }
            return true;
        }

        private void input_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
            if (!IsInputValid(input.Text, InputType)) {
                container.Background = new System.Windows.Media.SolidColorBrush(Utils.Colors.Red);
                input.Background = container.Background;
            }
            else {
                container.Background = _originBackground;
                input.Background = container.Background;
            }
        }
    }
}
