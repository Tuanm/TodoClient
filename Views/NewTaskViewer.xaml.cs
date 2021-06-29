﻿using System.Windows;
using Todo.Controls;
using Todo.Models;
using Todo.Services;

namespace Todo.Views {
    public partial class NewTaskViewer : Window {
        private TaskService _taskService = new LocalTaskService();
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

        public NewTaskViewer WithTask(Task task = null) {
            if (task == null) {
                _isUpdate = false;
                _task = new Task();
            }
            else {
                _isUpdate = true;
                _task = task;
            }
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
            _task.Title = (container.Children[0] as InputControl).GetInput();
            _task.Details = (container.Children[1] as InputControl).GetInput();
            _task.DueDate = System.DateTime.Parse((container.Children[2] as InputControl).GetInput());
            return _task;
        }

        private void LoadButtons() {
            menu.Children.Clear();
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Green)
                .WithAction((sender, e) => {
                    try {
                        _task = LoadTaskFromInput();
                        if (_isUpdate) {
                            _taskService.UpdateTask(_task);
                            Utils.Displayer.PushNotification(
                                title: "Saved!",
                                message: "Let's finish this task.",
                                color: Utils.Colors.White
                            );
                        }
                        else {
                            _taskService.AddTask(_task);
                            Utils.Displayer.PushNotification(
                                title: "Added!",
                                message: "Let's finish this task.",
                                color: Utils.Colors.White
                            );
                        }
                    } catch (System.Exception ex) {
                        Utils.Displayer.PushNotification(
                            title: "Whoops!",
                            message: ex.Message,
                            color: Utils.Colors.White
                        );
                    }
                    this.Hide();
                }).WithContent("ok"));
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Yellow)
                .WithAction((sender, e) => {
                    Utils.Displayer.PushNotification(
                        title: "Canceled!",
                        message: "Hmm.",
                        color: Utils.Colors.White
                    );
                    this.Hide();
                }).WithContent("cancel"));
        }
    }
}
