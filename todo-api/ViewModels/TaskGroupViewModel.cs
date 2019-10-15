namespace todo_api.Models
{
    public class TaskGroupViewModel
    {
        public TaskGroupViewModel(int id, string name, int taskCount)
        {
            this.Id = id;
            this.Name = name;
            this.TaskCount = taskCount;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TaskCount { get; set; }
    }
}