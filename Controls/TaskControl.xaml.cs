using System.Windows.Controls;
using Todo.Models;

namespace Todo.Controls {
    public partial class TaskControl : UserControl {
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

        private void ConfigureStyles() {
            containerShadow.Width = container.Width;
            containerShadow.Height = container.Height;
            _selectedMargin = container.Margin;
            container.Margin = containerShadow.Margin;
        }

        private void LoadBackground() {
            var color = Utils.Tasks.GetColorOf(_task);
            container.Background = new System.Windows.Media.SolidColorBrush(color);
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
