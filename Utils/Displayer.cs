using System;

namespace Todo.Utils {
    public class Displayer {
        public delegate void MouseDownEvent(object sender, EventArgs e);

        public static void DisplayNotification(string title, string message, MouseDownEvent mouseDownEvent = null) {
            var popup = new Views.Popup(new Controls.NotificationControl()
                .WithContent(new Models.Notification() {
                    Title = title,
                    Message = message
                }).WithBackground(Utils.Colors.White))
                .WithSize(300, 100).Fade().FloatUp();
            popup.Show();
            if (mouseDownEvent != null) {
                popup.MouseDown += new System.Windows.Input.MouseButtonEventHandler(mouseDownEvent);
            }
        }
    }
}
