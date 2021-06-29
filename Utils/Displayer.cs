using System;

namespace Todo.Utils {
    public class Displayer {
        public delegate void MouseDownAction(object sender, EventArgs e);

        public static void DisplayNotification(string title, string message, MouseDownAction action = null) {
            var popup = new Views.Popup(new Controls.NotificationControl()
                .WithContent(new Models.Notification() {
                    Title = title,
                    Message = message
                }).WithBackground(Colors.White))
                .WithSize(300, 100).Fade().FloatUp();
            popup.Show();
            if (action != null) {
                popup.MouseDown += new System.Windows.Input.MouseButtonEventHandler(action);
            }
        }
    }
}
