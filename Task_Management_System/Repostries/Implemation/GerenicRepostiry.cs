namespace Task_Management_System.Repostries.Implemation
{
    public class GenericRepository<T> : IRepostriy<T> where T : class
    {
        private readonly TaskDbContext _taskDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(TaskDbContext taskDbContext)
        {
            _taskDbContext = taskDbContext;
            _dbSet = _taskDbContext.Set<T>();
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetByID(string id)
        {
            return _dbSet.Find(id)!;
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Save()
        {
            _taskDbContext.SaveChanges();
        }
    }
}