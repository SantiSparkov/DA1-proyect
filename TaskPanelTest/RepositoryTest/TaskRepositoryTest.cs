using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.RepositoryTest;

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
        Assert.IsNotNull(_taskRepository);
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
            Priority = EPriority.HIGH
        };
        
        // Act
        _taskRepository.AddTask(task);
        
        // Assert
        var actualTask = _taskRepository.GetTaskById(task.Id);
        Assert.IsNotNull(actualTask, "the task is not added to the repository");
        Assert.AreEqual("Task 1", actualTask.Title, "the task title is not stored correctly");
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
            Priority = EPriority.HIGH
        };
        _taskRepository.AddTask(task);
        
        // Act
        _taskRepository.DeleteTask(task.Id);
        
        // Assert
        var exception = Assert.ThrowsException<TaskNotValidException>(() => _taskRepository.GetTaskById(task.Id));
        Assert.AreEqual(exception.Message, $"Task with id {task.Id} not found");
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
            Priority = EPriority.HIGH
        };
        var task2 = new Task
        {
            Id = 2,
            Title = "Task 2",
            Description = "Description 2",
            DueDate = DateTime.Now,
            Priority = EPriority.MEDIUM
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
        var task = new Task()
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = EPriority.HIGH
        };
        
        var task2 = new Task()
        {
            Id = 2,
            Title = "Task 2",
            Description = "Description 2",
            DueDate = DateTime.Now,
            Priority = EPriority.MEDIUM
        };
        
        _taskRepository.AddTask(task);
        
        // Act
        var actualTask = _taskRepository.AddTask(task2);
        
        // Assert
        Assert.IsNotNull(actualTask, "the task is not found in the repository");
        Assert.AreEqual("Task 2", actualTask.Title, "the task is not found correctly");
    }
    
    [TestMethod]
    public void TaskRepository_UpdateTask()
    {
        // Arrange
        var task = new Task()
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = EPriority.HIGH
        };
        
        var addTask = _taskRepository.AddTask(task);
        
        // Act
        var updatedTask = new Task()
        {
            Id = addTask.Id,
            Title = "Task 1 Updated",
            Description = "Description 1 Updated",
        };
        
        _taskRepository.UpdateTask(updatedTask);
        
        // Assert
        var actualTask = _taskRepository.GetTaskById(addTask.Id);
        Assert.IsNotNull(actualTask, "the task is not found in the repository");
        Assert.AreEqual("Task 1 Updated", actualTask.Title, "the task is not updated correctly");
        Assert.AreEqual("Description 1 Updated", actualTask.Description, "the task is not updated correctly");
    }
    
    [TestMethod]
    public void UpdateTask_ShouldThrowExceptionIfTaskDoesNotExist()
    {
        // Arrange
        var task = new Task()
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            DueDate = DateTime.Now,
            Priority = EPriority.HIGH
        };
        
        // Act & Assert
        var exception = Assert.ThrowsException<TaskNotValidException>
            ((new Action(() => _taskRepository.UpdateTask(task))));
        Assert.AreEqual("Task with id 1 not found", exception.Message);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        var tasks = _taskRepository.GetAllTasks();
        var tasksToDelete = tasks.ToList();
        foreach (var task in tasksToDelete)
        {
            _taskRepository.DeleteTask(task.Id);
        }
    }

}