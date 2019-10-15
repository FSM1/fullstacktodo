using System;

namespace todo_api.ViewModels
{
    public class UserTaskViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Deadline { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public int? GroupId { get; set; }
    }
}