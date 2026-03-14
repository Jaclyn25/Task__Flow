namespace Task_Management_System.ViewModel
{
    public class TaskViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
    }
}
