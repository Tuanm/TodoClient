using System.Windows.Controls;
using Todo.Models;

namespace Todo.Controls {
    public partial class NotificationControl : UserControl {
        private Notification _notification;

        public NotificationControl() {
            InitializeComponent();
        }

        public NotificationControl WithBackground(System.Windows.Media.Color color) {
            container.Background = new System.Windows.Media.SolidColorBrush(color);
            return this;
        }

        public NotificationControl WithContent(Notification notification) {
            _notification = notification;
            this.DataContext = _notification;
            return this;
        }
    }
}
