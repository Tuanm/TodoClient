using System.Windows.Controls;
using Todo.Models;

namespace Todo.Controls {
    public partial class TaskControl : UserControl {
        public static double DangerDays = 1.0d;

        private Task _task;
        private bool _isSelected;
        private System.Windows.Thickness _selectedMargin;

        public Task Task { get { return _task; } }
        public bool IsSelected {
            get {
                return _isSelected;
            }
            set {
                ChangeSelection(value);
            }
        }

        public static System.Windows.Media.Color GetColorOf(Task task) {
            if (task.Done) return Utils.Colors.Green;
            else {
                var daysLeft = (task.DueDate - System.DateTime.Now).TotalDays;
                if (daysLeft < 0) return Utils.Colors.Gray;
                else {
                    if (daysLeft < 2 * DangerDays + 1) {
                        if (daysLeft < DangerDays) return Utils.Colors.Red;
                        else return Utils.Colors.Yellow;
                    }
                }
            }
            return Utils.Colors.White;
        }

        public TaskControl() {
            InitializeComponent();
            ConfigureStyles();
        }

        public TaskControl WithBackgroundLoader(int seconds = 1) {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(seconds);
            timer.Tick += (sender, e) => {
                LoadBackground();
            };
            timer.Start();
            return this;
        }

        public TaskControl WithTask(Task task) {
            _task = task;
            this.DataContext = _task;
            return this;
        }

        public TaskControl WithReminder(Utils.MouseDownAction action) {
            System.TimeSpan GetTimerInterval(System.Windows.Media.Color color) {
                if (color == Utils.Colors.White) return System.TimeSpan.Zero;
                else if (color == Utils.Colors.Green)
                    return System.TimeSpan.FromSeconds(900);
                else if (color == Utils.Colors.Gray)
                    return System.TimeSpan.FromSeconds(600);
                else if (color == Utils.Colors.Red)
                    return System.TimeSpan.FromSeconds(100);
                else if (color == Utils.Colors.Yellow)
                    return System.TimeSpan.FromSeconds(300);
                return System.TimeSpan.Zero;
            }
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = GetTimerInterval(GetColorOf(_task));
            timer.Tick += (sender, e) => {
                var currentTimer = sender as System.Windows.Threading.DispatcherTimer;
                if (currentTimer.Interval == System.TimeSpan.Zero) currentTimer.Stop();
                PushReminder(action);
            };
            timer.Start();
            return this;
        }

        private void ConfigureStyles() {
            containerShadow.Width = container.Width;
            containerShadow.Height = container.Height;
            _selectedMargin = container.Margin;
            container.Margin = containerShadow.Margin;
        }

        private void LoadBackground() {
            var color = GetColorOf(_task);
            container.Background = new System.Windows.Media.SolidColorBrush(color);
        }

        private void PushReminder(Utils.MouseDownAction action) {
            var color = GetColorOf(_task);
            var message = string.Empty;
            if (color == Utils.Colors.White) return;
            else if (color == Utils.Colors.Green)
                message = $"Task {_task.Id} has been done. Clean it!";
            else if (color == Utils.Colors.Gray)
                message = $"You didn't finish task {_task.Id}.";
            else if (color == Utils.Colors.Red)
                message = $"Task {_task.Id}'s due date is so closed. Do it!";
            else if (color == Utils.Colors.Yellow)
                message = $"Task {_task.Id} is waiting to finish...";
            Utils.Displayer.PushNotification(
                title: "Reminder",
                message: message,
                color: color,
                action: action
            );
        }

        private void ChangeSelection(bool isSelected) {
            _isSelected = isSelected;
            if (_isSelected) {
                container.Margin = _selectedMargin;
            }
            else {
                container.Margin = containerShadow.Margin;
            }
        }

        private void container_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            LoadBackground();
        }

        private void container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ChangeSelection(!_isSelected);
        }
    }
}
