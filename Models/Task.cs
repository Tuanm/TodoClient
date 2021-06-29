namespace Todo.Models {
    public class Task {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public System.DateTime DueDate { get; set; }
        public bool Done { get; set; }
    }
}
