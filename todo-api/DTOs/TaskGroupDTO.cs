namespace todo_api.DTOs
{
    public class TaskGroupDTO
    {
        public TaskGroupDTO(){}
        public TaskGroupDTO(string name)
        {
            this.Name = name;
        }
        public string? Name { get; set; }
    }
}