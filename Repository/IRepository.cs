public interface IRepository<T>
{
    List<T> GetAll();
    T GetById(int id);
    T Create(T obj);
    //void Remove(int id);
    //void Update(T obj);
}
