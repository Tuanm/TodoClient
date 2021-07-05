
using Todo.Services;

namespace Todo.Utils {
    public class Services {
        public static bool NoServer { get; set; }

        public static Service GetService() {
            if (NoServer) return new LocalTaskService();
            return new TaskService();
        }
    }
}
