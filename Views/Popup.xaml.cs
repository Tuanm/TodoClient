using System.Windows;
using System.Windows.Controls;

namespace Todo.Views {
    public partial class Popup : Window {
        private UserControl _control;

        private Popup() {
            InitializeComponent();
        }

        public Popup(UserControl control) : this() {
            _control = control;
            if (_control != null) container.Children.Add(_control);
        }

        public Popup WithSize(int width, int height) {
            if (width > 0) _control.Width = width;
            if (height > 0) _control.Height = height;
            return this;
        }

        public Popup FloatUp(double step = 0.5d, double seconds = 0.025d) {
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
