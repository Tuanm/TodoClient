using System.Windows;
using Todo.Controls;
using Todo.Models;
using Todo.Services;

namespace Todo.Views {
    public partial class NewTaskViewer : Window {
        private TaskService _taskService = new TaskService();
        private Task _task;
        private bool _isUpdate;

        public NewTaskViewer() {
            InitializeComponent();
            _task = new Task() { 
                Title = "Title",
                Details = "Details",
                DueDate = System.DateTime.Now
            };
            _isUpdate = false;
            LoadButtons();
            LoadInput();
        }

        public NewTaskViewer WithTask(Task task) {
            _task = task;
            _isUpdate = true;
            LoadInput();
            return this;
        }

        private void LoadInput() {
            container.Children.Clear();
            container.Children.Add(new InputControl().WithContent("Title", _task.Title));
            container.Children.Add(new InputControl().WithContent("Details", _task.Details));
            container.Children.Add(new InputControl().WithContent("Due Date", _task.DueDate.ToString()));
        }

        private Task LoadTaskFromInput() {
            return new Task() {
                Title = (container.Children[0] as InputControl).GetInput(),
                Details = (container.Children[1] as InputControl).GetInput(),
                DueDate = System.DateTime.Parse((container.Children[2] as InputControl).GetInput())
            };
        }

        private void LoadButtons() {
            menu.Children.Clear();
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Green)
                .WithAction((sender, e) => {
                    try {
                        _task = LoadTaskFromInput();
                        if (_isUpdate) {
                            _taskService.UpdateTask(_task);
                            Utils.Displayer.DisplayNotification(
                                "Saved!",
                                "Let's finish this task."
                            );
                        }
                        else {
                            _taskService.AddTask(_task);
                            Utils.Displayer.DisplayNotification(
                                "Added!",
                                "Let's finish this task."
                            );
                        }
                    } catch (System.Exception ex) {
                        Utils.Displayer.DisplayNotification(
                            "Whoops!",
                            ex.Message
                        );
                    }
                    this.Close();
                }).WithContent("ok"));
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Yellow)
                .WithAction((sender, e) => {
                    Utils.Displayer.DisplayNotification(
                        "Canceled!",
                        "You still have to finish this task."
                    );
                    this.Close();
                }).WithContent("cancel"));
        }
    }
}
