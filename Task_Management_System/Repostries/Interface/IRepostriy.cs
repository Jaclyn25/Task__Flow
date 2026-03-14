public interface IRepostriy<T>
{
    List<T> GetAll();
    T GetByID(string id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Save();
}