using System.Windows.Controls;

namespace Todo.Controls {
    public partial class NewTaskControl : UserControl {
        public NewTaskControl() {
            InitializeComponent();
        }

        private void container_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Utils.Displayer.DisplayNotification("Whoops!", "This feature will be available soon.");
        }
    }
}
