using System.Windows;

namespace Todo {
    public partial class App : Application {
        private void Application_Startup(object sender, StartupEventArgs e) {
            try {
                Utils.Windows.GetTaskViewer().Show();
                Utils.Displayer.PushNotification(
                    title: "Tips",
                    message: "Press Escape (ESC) to hide the window.",
                    color: Utils.Colors.Green
                );
            } catch (System.Exception ex) {
                Utils.Displayer.PushNotification(
                    title: "Whoops!",
                    message: ex.Message,
                    color: Utils.Colors.White);
            }
        }
    }
}
