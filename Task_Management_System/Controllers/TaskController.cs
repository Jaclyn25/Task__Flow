using Microsoft.EntityFrameworkCore;

namespace Task_Management_System.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IRepostriy<TaskModel> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaskController(IRepostriy<TaskModel> repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = _repository.GetAll()
                .Where(t => t.UserId == userId)
                .Include(t => t.Images)
                .ToList();
            return View(tasks);
        }

        [HttpPost]
        public IActionResult SaveNew(TaskViewModel taskViewModel)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (string.IsNullOrEmpty(taskViewModel.Name) || string.IsNullOrEmpty(taskViewModel.Description))
            {
                return BadRequest(new { success = false, message = "Name and Description are required" });
            }

            TaskModel model = new TaskModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = taskViewModel.Name,
                Description = taskViewModel.Description,
                DueDate = taskViewModel.DueDate,
                IsCompleted = taskViewModel.IsCompleted,
                UserId = userId,
                Images = new List<ImagesOfTask>()
            };

            if (taskViewModel.TaskImages != null)
            {
                string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                foreach (var file in taskViewModel.TaskImages)
                {
                    if (file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(imagesFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        model.Images.Add(new ImagesOfTask { imageUrl = fileName });
                    }
                }
            }
            _repository.Add(model);
            _repository.Save();
            
            return Json(new { success = true, message = "Task created successfully", taskId = model.Id });
        }
        [HttpGet]
        public IActionResult GetTask(string id)
        {
            var task = _repository.GetAll()
                        .Where(t => t.Id == id)
                        .Include(t => t.Images)
                        .Select(t => new {
                            id = t.Id,
                            name = t.Name,
                            description = t.Description,
                            dueDate = t.DueDate.ToString("yyyy-MM-dd"),
                            isCompleted = t.IsCompleted,
                            images = t.Images.Select(img => new { imageUrl = img.imageUrl }).ToList()
                        })
                        .FirstOrDefault();

            if (task == null) return NotFound();

            return Json(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskViewModel taskViewModel)
        {
            if (string.IsNullOrEmpty(taskViewModel.Id))
            {
                return BadRequest(new { success = false, message = "Task ID is required" });
            }

            var existingTask = _repository.GetByID(taskViewModel.Id);
            if (existingTask == null)
                return NotFound(new { success = false, message = "Task not found" });

            // Update basic fields
            existingTask.Name = taskViewModel.Name;
            existingTask.Description = taskViewModel.Description;
            existingTask.DueDate = taskViewModel.DueDate;
            existingTask.IsCompleted = taskViewModel.IsCompleted;

            // Handle new images if provided
            if (taskViewModel.TaskImages != null && taskViewModel.TaskImages.Count > 0)
            {
                string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                foreach (var file in taskViewModel.TaskImages)
                {
                    if (file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(imagesFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        existingTask.Images.Add(new ImagesOfTask { imageUrl = fileName });
                    }
                }
            }

            _repository.Update(existingTask);
            _repository.Save();
            
            return Json(new { success = true, message = "Task updated successfully" });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { success = false, message = "Task ID is required" });

            var task = _repository.GetByID(id);
            if (task == null)
                return NotFound(new { success = false, message = "Task not found" });

            _repository.Delete(task);
            _repository.Save();
            
            return Json(new { success = true, message = "Task deleted successfully" });
        }
    }
}