using TaskPanelLibrary.Config;
using TaskPanelLibrary.Repository.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Repository;

public class TaskSqlRepository : ITaskRepository
{
    private SqlContext _tasksDataBase;

    public TaskSqlRepository(SqlContext tasksDataBase)
    {
        _tasksDataBase = tasksDataBase;
    }

    public Task AddTask(Task task)
    {
        using (SqlContext ctx = new SqlContext(null))
        { 
            _tasksDataBase.Tasks.Add(task);
            _tasksDataBase.SaveChanges();
        }
        return task;
    }

    public Task DeleteTask(int id)
    {
        Task task = _tasksDataBase.Tasks.Find(id);
        if (task == null)
        {
            throw new System.Exception($"Task with id: {id} does not exist");

        }
        _tasksDataBase.Tasks.Remove(task);
        return task;
    }

    public Task GetTaskById(int id)
    {
        Task task = _tasksDataBase.Tasks.Find(id);
        if (task == null)
        {
            throw new System.Exception($"Task with id: {id} does not exist");

        }
        return task;
    }

    public List<Task> GetAllTasks()
    {
        return _tasksDataBase.Tasks.ToList();
    }

    public Task UpdateTask(Task task)
    {
        _tasksDataBase.Tasks.Update(task);
        return task;
    }

}