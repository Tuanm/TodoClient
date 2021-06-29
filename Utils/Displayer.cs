using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;

namespace Todo.Utils {
    public class Displayer {
        private static object _lock = new object();
        private static int _count = 0;

        public static int Limit = 10;

        public static void PushNotification(string title, string message,
            System.Windows.Media.Color color, MouseDownAction action = null) {
            var popup = new Views.Popup(new Controls.NotificationControl()
                .WithContent(new Models.Notification() {
                    Title = title,
                    Message = message
                }).WithBackground(color))
                .WithSize(300, 100).Fade().FloatUp();
            if (action != null) {
                popup.MouseDown += new System.Windows.Input.MouseButtonEventHandler(action);
            }
            if (_count < Limit) {
                _count++;
                lock (_lock) {
                    popup.Show();
                    _count--;
                }
            }
        }
    }
}
