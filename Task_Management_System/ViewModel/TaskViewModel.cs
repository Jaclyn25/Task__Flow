namespace Task_Management_System.ViewModel
{
    public class TaskViewModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public List<IFormFile>? TaskImages { get; set; }
    }
}
