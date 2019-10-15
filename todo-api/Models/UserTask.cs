using System;

namespace todo_api.Models
{
    public class UserTask
    {
        public UserTask() { }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Deadline { get; set; }
        public User? User { get; set; }
        public TaskStatus Status { get; set; }
        public TaskGroup TaskGroup { get; set; }
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed
    }
}