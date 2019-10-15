using System.Collections.Generic;

namespace todo_api.Models
{
    public class User
    {
        public User() 
        {
            this.Tasks = new List<UserTask>();
        }
        public int Id {get;set;}
        public string? FirstName {get;set;}
        public string? LastName {get;set;}
        public virtual ICollection<UserTask> Tasks {get;set;}
        public virtual string FullName() {
            return $"{FirstName} {LastName}";
        }
    }
}