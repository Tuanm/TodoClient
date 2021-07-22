using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Todo {
    public partial class App : Application {
        private void Application_Startup(object sender, StartupEventArgs e) {
            try {
                Utils.Services.NoServer = false; // no server
                ConfigureSettings();
                this.MainWindow = Utils.Windows.GetTaskViewer().WithReminder();
                this.MainWindow.Show();
                StartPushingTips();
            } catch (System.Exception ex) {
                Utils.Displayer.PushNotification(
                    title: "Whoops!",
                    message: ex.Message,
                    color: Utils.Colors.Red
                );
            }
        }

        private static void ConfigureSettings() {
            Services.LocalTaskService.FileName = @"./tasks.json";
            Utils.Tasks.SettingsFileName = @"./settings.json";
            Utils.Tasks.LoadSettings();
        }

        private static void StartPushingTips() {
            Utils.Stuff.TipsFileName = @"./tips.json";
            var tips = Utils.Stuff.GetTips();
            var timer = new DispatcherTimer() {
                Interval = System.TimeSpan.FromMinutes(1)
            };
            var random = new System.Random();
            timer.Tick += (sender, e) => {
                Utils.Displayer.PushNotification(
                    title: "Tips",
                    message: tips[random.Next(tips.Count)],
                    color: Utils.Colors.Green
                );
            };
            timer.Start();
        }

        public static void Restart() {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
