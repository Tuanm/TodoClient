using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Todo.Controls;
using Todo.Models;
using Todo.Services;

namespace Todo.Views {
    public partial class TaskViewer : Window {
        private int _rows = 3;
        private int _columns = 3;
        private Service _taskService = Utils.Services.GetService();
        private bool _isClosed = false;

        public bool IsClosed { get { return _isClosed; } }
        
        public List<UserControl> SelectedTaskControls {
            get {
                var selectedTaskControls = new List<UserControl>();
                foreach (var control in container.Children) {
                    if (control is TaskControl) {
                        var taskControl = control as TaskControl;
                        if (taskControl.IsSelected) selectedTaskControls.Add(taskControl);
                    }
                }
                return selectedTaskControls;
            }
        }

        public TaskViewer() {
            InitializeComponent();
            LoadMenu();
            LoadExtraMenu();
        }

        public TaskViewer WithGrid(int rows, int columns) {
            if (rows > 0) _rows = rows;
            if (columns > 0) _columns = columns;
            container.RowDefinitions.Clear();
            for (var rowIndex = 0; rowIndex < _rows; rowIndex++) {
                var rowDefinition = new RowDefinition();
                container.RowDefinitions.Add(rowDefinition);
            }
            container.ColumnDefinitions.Clear();
            for (var columnIndex = 0; columnIndex < _columns; columnIndex++) {
                var columnDefinition = new ColumnDefinition() {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                container.ColumnDefinitions.Add(columnDefinition);
            }
            return this;
        }

        private void AddTaskControl(UserControl taskControl) {
            int rowIndex = container.Children.Count / _columns;
            int columnIndex = container.Children.Count % _columns;
            taskControl.SetValue(Grid.RowProperty, rowIndex);
            taskControl.SetValue(Grid.ColumnProperty, columnIndex);
            container.Children.Add(taskControl);
        }

        public TaskViewer LoadTasks() {
            container.Children.Clear();
            AddTaskControl(new NewTaskControl().WithAction((sender, e) => {
                this.Hide();
                Utils.Windows.GetNewTaskViewer().WithTask(null).ShowDialog();
                Utils.Windows.Show(this);
                LoadTasks();
                ChangeMenuVisibility();
            }));
            foreach (var task in _taskService.GetAllTasks()) {
                var taskControl = new TaskControl().WithTask(task)
                    .WithBackgroundLoader();
                AddTaskControl(taskControl);
            }
            return this;
        }

        public TaskViewer WithReminder() {
            var whiteTasks = new List<Task>();
            var greenTasks = new List<Task>();
            var grayTasks = new List<Task>();
            var redTasks = new List<Task>();
            var yellowTasks = new List<Task>();
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(2.5); // reload tasks' status every 2.5 seconds
            timer.Tick += (sender, e) => {
                whiteTasks.Clear();
                greenTasks.Clear();
                grayTasks.Clear();
                redTasks.Clear();
                yellowTasks.Clear();
                if (Utils.Tasks.ReminderEnabled) {
                    foreach (var control in container.Children) {
                        if (control is TaskControl) {
                            var task = (control as TaskControl).Task;
                            var color = Utils.Tasks.GetColorOf(task);
                            if (color == Utils.Colors.White) whiteTasks.Add(task);
                            else if (color == Utils.Colors.Green) greenTasks.Add(task);
                            else if (color == Utils.Colors.Gray) grayTasks.Add(task);
                            else if (color == Utils.Colors.Red) redTasks.Add(task);
                            else if (color == Utils.Colors.Yellow) yellowTasks.Add(task);
                        }
                    }
                }
            };
            timer.Start();

            Utils.MouseDownAction action = (sender, e) => {
                Utils.Windows.Show(this);
                this.Focus();
            };
            var whiteTimer = Utils.Tasks.GetTimerWith(Utils.Tasks.ReminderSeconds, (sender, e) => { });
            var greenTimer = Utils.Tasks.GetTimerWith(Utils.Tasks.ReminderSeconds, (sender, e) => { });
            var grayTimer = Utils.Tasks.GetTimerWith(Utils.Tasks.ReminderSeconds, (sender, e) => { });
            var redTimer = Utils.Tasks.GetTimerWith(Utils.Tasks.ReminderSeconds, (sender, e) => {
                var redCount = redTasks.Count;
                if (redCount > 0) {
                    Utils.Displayer.PushNotification(
                        title: "Reminder",
                        message: $"You have {redCount} "
                            + (redCount > 1 ? "tasks" : "task")
                            + " closed to " 
                            + (redCount > 1 ? "their" : "its")
                            + " due date.",
                        color: Utils.Colors.Red,
                        action: action
                    );
                }
            });
            var yellowTimer = Utils.Tasks.GetTimerWith(Utils.Tasks.ReminderSeconds, (sender, e) => { });

            redTimer.Start();

            return this;
        }

        private void LoadMenu() {
            menu.Children.Clear();
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Green)
                .WithAction((sender, e) => {
                    foreach (var selectedControl in this.SelectedTaskControls) {
                        var selectedTaskControl = selectedControl as TaskControl;
                        selectedTaskControl.IsSelected = false;
                        selectedTaskControl.Task.Done = true;
                        _taskService.UpdateTask(selectedTaskControl.Task);
                    }
                    LoadTasks();
                    ChangeMenuVisibility();
                    Utils.Displayer.PushNotification(
                        title: "Done!",
                        message: "Very good.",
                        color: Utils.Colors.White
                    );
                }).WithContent("Done").Disable());
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Red)
                .WithAction((sender, e) => {
                    foreach (var selectedControl in this.SelectedTaskControls) {
                        var selectedTaskControl = selectedControl as TaskControl;
                        _taskService.RemoveTask(selectedTaskControl.Task.Id);
                    }
                    LoadTasks();
                    ChangeMenuVisibility();
                    Utils.Displayer.PushNotification(
                        title: "Deleted!",
                        message: "Maybe you have finished them.",
                        color: Utils.Colors.White
                    );
                }).WithContent("Delete").Disable());
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Yellow)
                .WithAction((sender, e) => {
                    if (this.SelectedTaskControls.Count != 1) {
                        Utils.Displayer.PushNotification(
                            title: "Whoops!",
                            message: "Just select ONE task to edit.",
                            color: Utils.Colors.Red
                        );
                    }
                    else {
                        this.Hide();
                        var selectedTask = (this.SelectedTaskControls[0] as TaskControl).Task;
                        Utils.Windows.GetNewTaskViewer().WithTask(selectedTask).ShowDialog();
                        Utils.Windows.Show(this);
                    }
                    LoadTasks();
                    ChangeMenuVisibility();
                }).WithContent("Edit").Disable());
        }

        private void ChangeMenuVisibility() {
            if (this.SelectedTaskControls.Count > 0) {
                foreach (var control in menu.Children) {
                    if (control is SquareControl) (control as SquareControl).Enable();
                }
            }
            else {
                foreach (var control in menu.Children) {
                    if (control is SquareControl) (control as SquareControl).Disable();
                }
            }
        }

        private void container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ChangeMenuVisibility();
        }

        private void LoadExtraMenu() {
            extraMenu.Children.Clear();
            extraMenu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Green)
                .WithAction((sender, e) => {
                    new SettingsViewer().ShowDialog();
                }).WithNoShadow());
            extraMenu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Yellow)
                .WithAction((sender, e) => {
                    this.Hide();
                    Utils.Displayer.DisplayBackToWindowPopup(
                        title: "Back",
                        message: "Click to back.",
                        window: this
                    );
                }).WithNoShadow());
            extraMenu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Red)
                .WithAction((sender, e) => {
                    this.Close();
                }).WithNoShadow());
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Escape) {
                this.Hide();
                Utils.Displayer.DisplayBackToWindowPopup(
                    title: "Back",
                    message: "Click to back.",
                    window: this
                );
            }
            else if (e.Key == System.Windows.Input.Key.X) {
                this.Close();
            }
            else if (e.Key == System.Windows.Input.Key.H) {
                var tips = string.Empty;
                foreach (var tip in Utils.Stuff.GetTips()) {
                    tips += $"{tip}\r\n";
                }
                Utils.Displayer.DisplayHelpPopup(
                    title: "Tips",
                    message: tips
                );
            }
            else if (e.Key == System.Windows.Input.Key.S) {
                new SettingsViewer().ShowDialog();
            }
            
        }

        private void Window_Closed(object sender, System.EventArgs e) {
            _isClosed = true;
        }
    }
}
