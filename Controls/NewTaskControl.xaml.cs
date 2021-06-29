using System.Windows.Controls;

namespace Todo.Controls {
    public partial class NewTaskControl : UserControl {
        public NewTaskControl() {
            InitializeComponent();
        }

        public NewTaskControl WithAction(Utils.MouseDownAction action) {
            container.MouseDown += new System.Windows.Input.MouseButtonEventHandler(action);
            return this;
        }
    }
}
