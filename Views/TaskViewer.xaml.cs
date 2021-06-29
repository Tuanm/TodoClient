using System.Windows;
using System.Windows.Controls;
using Todo.Controls;
using Todo.Services;

namespace Todo.Views {
    public partial class TaskViewer : Window {
        private int _rows = 3;
        private int _columns = 3;

        public TaskViewer() {
            InitializeComponent();
        }

        public TaskViewer WithGrid(int rows, int columns) {
            if (rows > 0) _rows = rows;
            if (columns > 0) _columns = columns;
            for (var rowIndex = 0; rowIndex < _rows; rowIndex++) {
                var rowDefinition = new RowDefinition();
                container.RowDefinitions.Add(rowDefinition);
            }
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
            AddTaskControl(new NewTaskControl());
            var taskService = new TaskService();
            foreach (var task in taskService.GetAllTasks()) {
                var taskControl = new TaskControl().WithTask(task).WithTicker();
                AddTaskControl(taskControl);
            }
            return this;
        }
    }
}
