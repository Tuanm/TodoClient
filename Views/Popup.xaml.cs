using System.Windows;
using System.Windows.Controls;

namespace Todo.Views {
    public partial class Popup : Window {
        private UserControl _control;

        private Popup() {
            InitializeComponent();
            SetFullScreen();
        }

        private void SetFullScreen() {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = desktopWorkingArea.Right;
            this.Height = desktopWorkingArea.Bottom;
            this.Left = 0;
            this.Top = 0;
        }

        public Popup(UserControl control) : this() {
            _control = control;
            if (_control != null) container.Children.Add(_control);
        }

        public Popup WithVisibleWidth(int width) {
            if (width > 0) _control.Width = width;
            container.Width = _control.Width + container.Margin.Left + container.Margin.Right;
            container.Height = _control.Height + container.Margin.Top + container.Margin.Bottom;
            return this;
        }

        public Popup CenterToScreen() {
            container.HorizontalAlignment = HorizontalAlignment.Center;
            container.VerticalAlignment = VerticalAlignment.Center;
            return this;
        }

        public Popup RightToScreen() {
            container.HorizontalAlignment = HorizontalAlignment.Right;
            return this;
        }

        public Popup LeftToScreen() {
            container.HorizontalAlignment = HorizontalAlignment.Left;
            return this;
        }

        public Popup TopToScreen() {
            container.VerticalAlignment = VerticalAlignment.Top;
            return this;
        }

        public Popup BottomToScreen() {
            container.VerticalAlignment = VerticalAlignment.Bottom;
            return this;
        }

        public Popup FloatUp(double step = 0.5d, double seconds = 0.0025d) {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(seconds);
            timer.Tick += (sender, e) => {
                container.Margin = new Thickness(
                    container.Margin.Left, container.Margin.Top, container.Margin.Right,
                    container.Margin.Bottom + step
                );
                if (container.Margin.Bottom >= this.Height / 2) this.Close();
            };
            timer.Start();
            return this;
        }

        public Popup Fade(double step = 0.0125d, double seconds = 0.05d) {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(seconds);
            timer.Tick += (sender, e) => {
                if (container.Opacity - step > 0) container.Opacity -= step;
                else this.Close();
            };
            timer.Start();
            return this;
        }
    }
}
