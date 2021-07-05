using System.Collections.Generic;
using Todo.Models;

namespace Todo.Services {
    public abstract class Service {
        public abstract List<Task> GetAllTasks();

        public abstract Task AddTask(Task task);

        public abstract Task GetTask(int id);

        public abstract Task UpdateTask(Task task);

        public abstract void RemoveTask(int id);
    }
}
