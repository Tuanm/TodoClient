namespace Todo.Utils {
    public class Displayer {
        public static void PushNotification(string title, string message,
            System.Windows.Media.Color color, MouseDownAction action = null) {
            var popup = new Views.Popup(new Controls.NotificationControl()
                .WithContent(new Models.Notification() {
                    Title = title,
                    Message = message
                }).WithBackground(color))
                .WithSize(350, 100).Fade().FloatUp();
            if (action != null) {
                popup.MouseDown += new System.Windows.Input.MouseButtonEventHandler(action);
            }
            popup.Show();
        }
    }
}
