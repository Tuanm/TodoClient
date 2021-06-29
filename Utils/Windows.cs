using Todo.Views;

namespace Todo.Utils {
    public class Windows {
        private static object _lock = new object();
        private static TaskViewer _taskViewer;
        private static NewTaskViewer _newTaskViewer;

        public static TaskViewer GetTaskViewer(bool isReminderEnabled = true) {
            if (_taskViewer == null || _taskViewer.IsClosed) {
                lock (_lock) {
                    if (_taskViewer == null || _taskViewer.IsClosed) {
                        _taskViewer = new TaskViewer() {
                            ReminderEnabled = isReminderEnabled
                        }.WithGrid(10, 3).LoadTasks();
                    }
                }
            }
            return _taskViewer;
        }

        public static NewTaskViewer GetNewTaskViewer() {
            if (_newTaskViewer == null || _newTaskViewer.IsClosed) {
                lock (_lock) {
                    if (_newTaskViewer == null || _newTaskViewer.IsClosed) {
                        _newTaskViewer = new NewTaskViewer();
                    }
                }
            }
            
            return _newTaskViewer;
        }

        public static void Show(System.Windows.Window window) {
            if (window == null) return;
            else if (window == _taskViewer) {
                GetTaskViewer().Show();
                GetNewTaskViewer().Hide();
            }
            else if (window == _newTaskViewer) {
                GetNewTaskViewer().Show();
                GetTaskViewer().Hide();
            }
            else window.Show();
        }

        public static void ShowDialog(System.Windows.Window window) {
            if (window == null) return;
            else if (window == _taskViewer) {
                GetTaskViewer().ShowDialog();
                GetNewTaskViewer().Hide();
            }
            else if (window == _newTaskViewer) {
                GetNewTaskViewer().ShowDialog();
                GetTaskViewer().Hide();
            }
            else window.ShowDialog();
        }

        public static void Close() {
            throw new System.Exception();
        }
    }
}
