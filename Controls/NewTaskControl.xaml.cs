using System.Windows.Controls;

namespace Todo.Controls {
    public partial class NewTaskControl : UserControl {
        public delegate void MouseDownAction(object sender, System.EventArgs e);

        public NewTaskControl() {
            InitializeComponent();
        }

        public NewTaskControl WithAction(MouseDownAction action) {
            container.MouseDown += new System.Windows.Input.MouseButtonEventHandler(action);
            return this;
        }
    }
}
