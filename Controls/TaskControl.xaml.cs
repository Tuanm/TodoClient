using System.Windows.Controls;
using Todo.Models;

namespace Todo.Controls {
    public partial class TaskControl : UserControl {
        public static double DangerDays = 2d;

        private Task _task;
        private bool _isSelected;
        private System.Windows.Thickness _selectedMargin;

        public Task Task { get { return _task; } }
        public bool IsSelected { get { return _isSelected; } }

        public TaskControl() {
            InitializeComponent();
            ConfigureStyles();
        }

        public TaskControl WithTicker(int seconds = 1) {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(seconds);
            timer.Tick += timer_Tick;
            timer.Start();
            return this;
        }

        private void timer_Tick(object sender, System.EventArgs e) {
            LoadBackground();
        }

        public TaskControl WithTask(Task task) {
            _task = task;
            this.DataContext = _task;
            return this;
        }

        private void ConfigureStyles() {
            containerShadow.Width = container.Width;
            containerShadow.Height = container.Height;
            _selectedMargin = container.Margin;
            container.Margin = containerShadow.Margin;
        }

        private void LoadBackground() {
            if (_task.Done) {
                container.Background = new System.Windows.Media.SolidColorBrush(Utils.Colors.Green);
            }
            else {
                var currentDateTime = System.DateTime.Now;
                if (_task.DueDate.CompareTo(currentDateTime) < 0) {
                    container.Background = new System.Windows.Media.SolidColorBrush(Utils.Colors.Gray);
                }
                else {
                    if ((_task.DueDate - currentDateTime).TotalDays < DangerDays) {
                        container.Background = new System.Windows.Media.SolidColorBrush(Utils.Colors.Red);
                    }
                    else {
                        container.Background = new System.Windows.Media.SolidColorBrush(Utils.Colors.Yellow);
                    }
                }
            }
        }

        private void container_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            LoadBackground();
        }

        private void container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            _isSelected = !_isSelected;
            if (_isSelected) {
                container.Margin = _selectedMargin;
            }
            else {
                container.Margin = containerShadow.Margin;
            }
        }
    }
}
