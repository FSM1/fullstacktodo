using System;
using todo_api.Models;

namespace todo_api.DTOs
{
    public class UserTaskDTO
    {
        public UserTaskDTO() { }

        public string? Name { get; set; }
        public DateTime? Deadline { get; set; }
        public int? UserId { get; set; }
        public TaskStatus Status { get; set; }
        public int GroupId { get; set; }
    }
}