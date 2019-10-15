using System.Collections.Generic;

namespace todo_api.Models
{
    public class TaskGroup
    {
        public TaskGroup()
        {
            UserTasks = new List<UserTask>();
        }
        public TaskGroup(string name)
        {
            UserTasks = new List<UserTask>();
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
    }
}