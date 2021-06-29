using System.Collections.Generic;
using Todo.Models;

namespace Todo.Services {
    public class TaskService {
        protected BaseAPI _api;

        public TaskService() {
            _api = new BaseAPI().WithBaseAddress("http://localhost:6969");
        }

        public List<Task> GetAllTasks() {
            var response = _api.Get<List<Task>>("/tasks");
            if (response == null) response = new List<Task>();
            return response;
        }

        public Task AddTask(Task task) {
            var response = _api.Post<Task>("/tasks/add", body: TaskDAO.from(task));
            return response;
        }

        public Task GetTask(int id) {
            var response = _api.Get<Task>($"/tasks/{id}");
            return response;
        }

        public Task UpdateTask(Task task) {
            var response = _api.Put<Task>("/tasks/update", body: TaskDAO.from(task));
            return response;
        }

        public void RemoveTask(int id) {
            _api.Post<object>($"/tasks/remove/{id}");
        }
    }

    public class TaskDAO {
        public int id { get; set; }
        public string title { get; set; }
        public string details { get; set; }
        public System.DateTime dueDate { get; set; }
        public bool done { get; set; }

        public Task asTask() {
            return new Task() {
                Id = id,
                Title = title,
                Details = details,
                DueDate = dueDate,
                Done = done
            };
        }

        public static TaskDAO from(Task task) {
            return new TaskDAO() {
                id = task.Id,
                title = task.Title,
                details = task.Details,
                dueDate = task.DueDate,
                done = task.Done
            };
        }
    }
}
