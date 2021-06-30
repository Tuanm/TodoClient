using Todo.Models;

namespace Todo.Utils {
    public class Tasks {
        public static double DangerDays = 1.0d;

        public static System.Windows.Media.Color GetColorOf(Task task) {
            if (task == null) return Utils.Colors.White;
            if (task.Done) return Utils.Colors.Green;
            else {
                var daysLeft = (task.DueDate - System.DateTime.Now).TotalDays;
                if (daysLeft < 0) return Utils.Colors.Gray;
                else {
                    if (daysLeft < 2 * DangerDays + 1) {
                        if (daysLeft < DangerDays) return Utils.Colors.Red;
                        else return Utils.Colors.Yellow;
                    }
                }
            }
            return Utils.Colors.White;
        }

        public static System.Windows.Threading.DispatcherTimer GetTimerWith(double seconds,
            Utils.TimerTickAction action) {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = System.TimeSpan.FromSeconds(seconds);
            timer.Tick += new System.EventHandler(action);
            return timer;
        }
    }
}
