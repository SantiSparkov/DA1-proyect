using Task = TaskPanelLibrary.Entity.Task;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.RepositoryTest.TaskRepositoryTest;

[TestClass]
public class TaskRepositoryTests
{
    private ITaskRepository _taskRepository;

    [TestInitialize]
    public void Initialize()
    {
        _taskRepository = new TaskRepository();
    }
    
    [TestMethod]
    public void CreateTaskRepository()
    {
        Assert.IsNotNull(_taskRepository, "TaskRepository is not created");
    }

    [TestMethod]
    public void TaskRepository_AddTask()
    {
        // Arrange
        var task = new Task
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = Task.TaskPriority.HIGH
        };
        
        _taskRepository.AddTask(task);
        
        // Act
        var actualTask = _taskRepository.GetTaskById(task.Id);
        
        // Assert
        Assert.IsNotNull(actualTask, "the task is not added to the repository");
        Assert.AreEqual(task, actualTask);
    }

    [TestMethod]
    public void TaskRepository_DeleteTask()
    {
        // Arrange
        var task = new Task
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = Task.TaskPriority.HIGH
        };
        _taskRepository.AddTask(task);
        
        // Act
        _taskRepository.DeleteTask(task.Id);
        
        // Assert
        Assert.IsNull(_taskRepository.GetTaskById(task.Id), "the task is not deleted from the repository");
    }

    [TestMethod]
    public void TaskRepository_GetAllTasks()
    {
        // Arrange
        var task1 = new Task
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = Task.TaskPriority.HIGH
        };
        var task2 = new Task
        {
            Id = 2,
            Title = "Task 2",
            Description = "Description 2",
            DueDate = DateTime.Now,
            Priority = Task.TaskPriority.MEDIUM
        };
        _taskRepository.AddTask(task1);
        _taskRepository.AddTask(task2);

        // Act
        var result = _taskRepository.GetAllTasks();

        // Assert
        Assert.AreEqual(2, result.Count, "the task list is not returned correctly");
    }

    [TestMethod]
    public void TaskRepository_GetTaskById()
    {
        // Arrange
        var task = new Task
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = Task.TaskPriority.HIGH
        };
        _taskRepository.AddTask(task);

        // Act
        var result = _taskRepository.GetTaskById(1);

        // Assert
        Assert.AreEqual(task, result, "the task is not returned correctly");
    }
    
    [TestMethod]
    public void TaskRepository_UpdateTask()
    {
        // Arrange
        var task = new Task
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = Task.TaskPriority.HIGH
        };
        _taskRepository.AddTask(task);
        
        var updatedTask = new Task
        {
            Id = 1,
            Title = "Task 1 Updated",
            Description = "Description 1 Updated",
            DueDate = DateTime.Now,
            Priority = Task.TaskPriority.MEDIUM
        };
        
        // Act
        _taskRepository.UpdateTask(updatedTask);
        var result = _taskRepository.GetTaskById(1);

        // Assert
        Assert.AreEqual(updatedTask, result, "the task is not updated correctly");
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        var tasks = _taskRepository.GetAllTasks();
        foreach (var task in tasks)
        {
            _taskRepository.DeleteTask(task.Id);
        }
    }
}