using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Todo.Models;

namespace Todo.Services {
    public class LocalTaskService : TaskService {
        private List<Task> _tasks;

        public static string FileName { get; set; }

        public LocalTaskService() {
            _tasks = new List<Task>();
        }

        private void SaveTasks() {
            string text = JsonConvert.SerializeObject(_tasks);
            File.WriteAllText(FileName, text);
        }

        public new List<Task> GetAllTasks() {
            string text = File.ReadAllText(FileName);
            _tasks = JsonConvert.DeserializeObject<List<Task>>(text);
            return _tasks;
        }

        public new Task AddTask(Task task) {
            task.Id = System.DateTime.Now.GetHashCode();
            _tasks.Add(task);
            SaveTasks();
            return task;
        }

        public new Task GetTask(int id) {
            foreach (var task in GetAllTasks()) {
                if (task.Id == id) return task;
            }
            return null;
        }

        public new Task UpdateTask(Task task) {
            foreach (var oldTask in GetAllTasks()) {
                if (oldTask.Id == task.Id) {
                    _tasks.Remove(oldTask);
                    AddTask(task);
                    break;
                }
            }
            SaveTasks();
            return task;
        }

        public new void RemoveTask(int id) {
            foreach (var oldTask in GetAllTasks()) {
                if (oldTask.Id == id) {
                    _tasks.Remove(oldTask);
                    break;
                }
            }
            SaveTasks();
        }
    }
}
