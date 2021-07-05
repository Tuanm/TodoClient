namespace Todo.Utils {
    public class Displayer {
        public static void PushNotification(string title, string message,
            System.Windows.Media.Color color, MouseDownAction action = null) {
            var popup = new Views.Popup(new Controls.NotificationControl()
                .WithContent(new Models.Notification() {
                    Title = title,
                    Message = message
                }).WithBackground(color))
                .WithVisibleWidth(350)
                .BottomToScreen().RightToScreen()
                .Fade().FloatUp();
            if (action != null) {
                popup.MouseDown += new System.Windows.Input.MouseButtonEventHandler(action);
            }
            popup.Show();
        }

        public static void DisplayBackToWindowPopup(string title, string message, System.Windows.Window window) {
            if (window == null) return;
            else if (window == Utils.Windows.GetTaskViewer()) window = Utils.Windows.GetTaskViewer();
            else if (window == Utils.Windows.GetNewTaskViewer()) window = Utils.Windows.GetNewTaskViewer();
            var popup = new Views.Popup(new Controls.NotificationControl()
                    .WithContent(new Models.Notification() {
                        Title = title,
                        Message = message
                    }).WithBackground(Utils.Colors.Green))
                    .WithVisibleWidth(200)
                    .TopToScreen().RightToScreen();
            popup.MouseDown += (s, action) => {
                popup.Close();
                Utils.Windows.Show(window);
                window.Focus();
            };
            var timer = new System.Windows.Threading.DispatcherTimer() {
                Interval = System.TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) => {
                if (window.IsActive) {
                    popup.Close();
                    timer.Stop();
                }
            };
            timer.Start();
            popup.Show();
        }

        public static void DisplayRestartPopup(string title, string message) {
            var popup = new Views.Popup(new Controls.NotificationControl()
                    .WithContent(new Models.Notification() {
                        Title = title,
                        Message = message
                    }).WithBackground(Utils.Colors.Green))
                    .WithVisibleWidth(200)
                    .TopToScreen().RightToScreen();
            popup.MouseDown += (s, action) => {
                popup.Close();
                App.Restart();
            };
            popup.Show();
        }

        public static void DisplayHelpPopup(string title, string message) {
            var popup = new Views.Popup(new Controls.NotificationControl()
                    .WithContent(new Models.Notification() {
                        Title = title,
                        Message = message
                    }).WithBackground(Utils.Colors.Green))
                    .WithVisibleWidth(450)
                    .CenterToScreen();
            popup.MouseDown += (s, action) => {
                popup.Close();
            };
            popup.Show();
        }
    }
}
