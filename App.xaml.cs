using System.Windows;

namespace Todo {
    public partial class App : Application {
        private void Application_Startup(object sender, StartupEventArgs e) {
            try {
                Utils.Displayer.Limit = 5;
                Utils.Windows.GetTaskViewer().WithGrid(10, 3).LoadTasks().Show();
            } catch (System.Exception ex) {
                Utils.Displayer.PushNotification(
                    title: "Whoops!",
                    message: ex.Message,
                    color: Utils.Colors.White);
            }
        }
    }
}
