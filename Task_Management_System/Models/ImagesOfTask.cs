namespace Task_Management_System.Models
{
    public class ImagesOfTask
    {
        public int Id { get; set; }
        [Required]
        public string imageUrl { get; set; }
        [ForeignKey("TaskModel")]
        public string TaskID { get; set; }
        public TaskModel? TaskModel { get; set; }
        [NotMapped]
        public IFormFile ClientFile { get; set; }
    }
}
