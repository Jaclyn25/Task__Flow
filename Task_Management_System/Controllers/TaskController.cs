namespace Task_Management_System.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IRepostriy<TaskModel> _repository;

        public TaskController(IRepostriy<TaskModel> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = _repository.GetAll().Where(t => t.UserId == userId).ToList();
            return View(tasks);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveNew(TaskViewModel taskViewModel)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (ModelState.IsValid)
            {
                TaskModel model = new TaskModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = taskViewModel.Name,
                    Description = taskViewModel.Description,
                    DueDate = taskViewModel.DueDate,
                    IsCompleted = taskViewModel.IsCompleted,
                    UserId = userId
                };

                _repository.Add(model);
                _repository.Save();
                return RedirectToAction("Index");
            }

            return View("New", taskViewModel);
        }

        // Get task details as JSON for modal
        [HttpGet]
        public IActionResult GetTask(string id)
        {
            var task = _repository.GetByID(id);
            return Json(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
                _repository.Save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            var task = _repository.GetByID(id);
            if (task != null)
            {
                _repository.Delete(task);
                _repository.Save();
            }
            return RedirectToAction("Index");
        }
    }
}

