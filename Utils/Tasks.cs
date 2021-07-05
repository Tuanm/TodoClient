using System.IO;
using Todo.Models;

namespace Todo.Utils {
    public class Tasks {
        private class TaskSettings {
            public double DangerDays { get; set; }
            public double ReminderSeconds { get; set; }
            public bool ReminderEnabled { get; set; }

            public TaskSettings() {
                DangerDays = 1;
                ReminderSeconds = 30;
                ReminderEnabled = true;
            }
        }

        private static TaskSettings _settings = new TaskSettings();

        public static string SettingsFileName { get; set; }

        public static double DangerDays {
            get { return _settings.DangerDays; }
            set {
                if (value > 0) {
                    _settings.DangerDays = value;
                    SaveSettings();
                }
            }
        }

        public static double ReminderSeconds {
            get { return _settings.ReminderSeconds; }
            set {
                if (value > 0) {
                    _settings.ReminderSeconds = value;
                    SaveSettings();
                }
            }
        }

        public static bool ReminderEnabled {
            get { return _settings.ReminderEnabled; }
            set {
                _settings.ReminderEnabled = value;
                SaveSettings();
            }
        }

        public static void LoadSettings() {
            if (File.Exists(SettingsFileName)) {
                var text = File.ReadAllText(SettingsFileName);
                _settings = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskSettings>(text);
                if (_settings == null) _settings = new TaskSettings();
            }
        }

        public static void SaveSettings() {
            if (!File.Exists(SettingsFileName)) {
                File.Create(SettingsFileName);
            }
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(_settings);
            File.WriteAllText(SettingsFileName, text);
        }

        public static System.Windows.Media.Color GetColorOf(Task task) {
            if (task == null) return Utils.Colors.White;
            if (task.Done) return Utils.Colors.Green;
            else {
                var daysLeft = (task.DueDate - System.DateTime.Now).TotalDays;
                if (daysLeft < 0) return Utils.Colors.Gray;
                else {
                    if (daysLeft < 2 * _settings.DangerDays + 1) {
                        if (daysLeft < _settings.DangerDays) return Utils.Colors.Red;
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
