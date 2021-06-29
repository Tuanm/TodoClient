using System.Windows;

namespace Todo {
    public partial class App : Application {
        private Window _window;

        private void Application_Startup(object sender, StartupEventArgs e) {
            try {
                LoadWindow();
            } catch (System.Exception ex) {
                Utils.Displayer.DisplayNotification("Whoops!", ex.Message);
            }
         }

        public void LoadWindow() {
            _window = new Views.TaskViewer().WithGrid(10, 3).LoadTasks();
            if (!_window.IsVisible) _window.Show();
        }
    }
}
