using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Todo.Controls;
using Todo.Services;

namespace Todo.Views {
    public partial class TaskViewer : Window {
        private int _rows = 3;
        private int _columns = 3;
        private TaskService _taskService = new TaskService();
        
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
                new NewTaskViewer().ShowDialog();
                this.Show();
                LoadTasks();
                ChangeMenuVisibility();
            }));
            foreach (var task in _taskService.GetAllTasks()) {
                var taskControl = new TaskControl().WithTask(task).WithTicker();
                AddTaskControl(taskControl);
            }
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
                    Utils.Displayer.DisplayNotification(
                        "Done!",
                        "Very good."
                    );
                }).WithContent("ok"));
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Red)
                .WithAction((sender, e) => {
                    foreach (var selectedControl in this.SelectedTaskControls) {
                        var selectedTaskControl = selectedControl as TaskControl;
                        _taskService.RemoveTask(selectedTaskControl.Task.Id);
                    }
                    LoadTasks();
                    ChangeMenuVisibility();
                    Utils.Displayer.DisplayNotification(
                        "Deleted!",
                        "Maybe you have finished them."
                    );
                }).WithContent("del"));
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Yellow)
                .WithAction((sender, e) => {
                    if (this.SelectedTaskControls.Count != 1) {
                        Utils.Displayer.DisplayNotification(
                            "Whoops!",
                            "Just select ONE task to edit."
                        );
                    }
                    else {
                        this.Hide();
                        var selectedTask = (this.SelectedTaskControls[0] as TaskControl).Task;
                        new NewTaskViewer().WithTask(selectedTask).ShowDialog();
                        this.Show();
                    }
                    LoadTasks();
                    ChangeMenuVisibility();
                }).WithContent("edit"));
        }

        private void ChangeMenuVisibility() {
            if (this.SelectedTaskControls.Count > 0) menu.Visibility = Visibility.Visible;
            else menu.Visibility = Visibility.Hidden;
        }

        private void container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            ChangeMenuVisibility();
        }
    }
}
