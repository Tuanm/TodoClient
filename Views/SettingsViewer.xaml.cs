using System.Windows;
using Todo.Controls;

namespace Todo.Views {
    public partial class SettingsViewer : Window {
        public SettingsViewer() {
            InitializeComponent();
            LoadButtons();
            LoadInput();
        }

        private void LoadInput() {
            container.Children.Clear();
            container.Children.Add(new InputControl()
                .WithContent(
                    "Danger Days",
                    Utils.Tasks.DangerDays
                )
                .WithWidth(1200)
            );
            container.Children.Add(new ToggleControl()
                .WithContent(
                    "Reminder Enabled",
                    Utils.Tasks.ReminderEnabled
                )
                .WithWidth(1200)
            );
            container.Children.Add(new InputControl()
                .WithContent(
                    "Reminder Seconds",
                    Utils.Tasks.ReminderSeconds
                )
                .WithWidth(1200)
            );
        }

        private void LoadButtons() {
            menu.Children.Clear();
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Green)
                .WithAction((sender, e) => {
                    try {
                        Utils.Tasks.DangerDays = double.Parse(
                            (container.Children[0] as InputControl).GetInput()
                        );
                        Utils.Tasks.ReminderEnabled = (container.Children[1] as ToggleControl).GetInput();
                        Utils.Tasks.ReminderSeconds = double.Parse(
                            (container.Children[2] as InputControl).GetInput()
                        );
                        Utils.Displayer.DisplayRestartPopup(
                            title: "Requirement",
                            message: "Restart to apply!"
                        );
                    } catch (System.Exception ex) {
                        Utils.Displayer.PushNotification(
                            title: "Whoops!",
                            message: ex.Message,
                            color: Utils.Colors.Red
                        );
                    }
                    this.Close();
                }).WithContent("Apply"));
            menu.Children.Add(new SquareControl().WithBackground(Utils.Colors.Yellow)
                .WithAction((sender, e) => {
                    Utils.Displayer.PushNotification(
                        title: "Canceled!",
                        message: "Hmm.",
                        color: Utils.Colors.White
                    );
                    this.Hide();
                }).WithContent("Cancel"));
        }
    }
}
