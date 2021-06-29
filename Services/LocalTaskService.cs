using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Todo.Models;

namespace Todo.Services {
    public class LocalTaskService : TaskService {
        private static object _lock = new object();
        private List<Task> _tasks;

        public static string FileName { get; set; }

        public LocalTaskService() {
            FileName = @"./data/tasks.json";
            LoadTasks();
        }

        private void SaveTasks() {
            string text = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
            File.WriteAllText(FileName, text);
        }

        private void LoadTasks() {
            if (!File.Exists(FileName)) {
                File.Create(FileName);
            }
            string text = File.ReadAllText(FileName);
            _tasks = JsonConvert.DeserializeObject<List<Task>>(text);
            if (_tasks == null) _tasks = new List<Task>();
        }

        public override List<Task> GetAllTasks() {
            lock (_lock) {
                LoadTasks();
            }
            return _tasks;
        }

        public override Task AddTask(Task task) {
            lock (_lock) {
                LoadTasks();
                task.Id = System.DateTime.Now.GetHashCode();
                _tasks.Add(task);
                SaveTasks();
            }
            return task;
        }

        public override Task GetTask(int id) {
            lock (_lock) {
                LoadTasks();
                foreach (var task in _tasks) {
                    if (task.Id == id) return task;
                }
            }
            return null;
        }

        public override Task UpdateTask(Task task) {
            lock (_lock) {
                LoadTasks();
                foreach (var oldTask in _tasks) {
                    if (oldTask.Id == task.Id) {
                        _tasks.Remove(oldTask);
                        _tasks.Add(task);
                        break;
                    }
                }
                SaveTasks();
            }
            return task;
        }

        public override void RemoveTask(int id) {
            lock (_lock) {
                LoadTasks();
                foreach (var oldTask in _tasks) {
                    if (oldTask.Id == id) {
                        _tasks.Remove(oldTask);
                        break;
                    }
                }
                SaveTasks();
            }
        }
    }
}
