namespace todo_api.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(int id, string fullName)
        {
            this.Id = id;
            this.FullName = fullName;
        }
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}