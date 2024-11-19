using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _mockTaskRepository;
        private Mock<ITrashService> _mockTrashService;
        private Mock<IUserService> _mockUserService;
        private TaskService _taskService;

        [TestInitialize]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockTrashService = new Mock<ITrashService>();
            _mockUserService = new Mock<IUserService>();

            _taskService = new TaskService(
                _mockTaskRepository.Object,
                _mockUserService.Object,
                _mockTrashService.Object
            );
        }

        [TestMethod]
        public void CreateTask_ShouldThrowException_WhenTaskIsNull()
        {
            // Act & Assert
            Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(null));
        }

        [TestMethod]
        public void CreateTask_ShouldThrowException_WhenTaskTitleIsEmpty()
        {
            // Arrange
            var task = new Task
            {
                Title = "",
                Description = "Valid description",
                DueDate = DateTime.Now.AddDays(1)
            };

            // Act & Assert
            Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(task));
        }

        [TestMethod]
        public void CreateTask_ShouldThrowException_WhenTaskDescriptionIsEmpty()
        {
            // Arrange
            var task = new Task
            {
                Title = "Valid title",
                Description = "",
                DueDate = DateTime.Now.AddDays(1)
            };

            // Act & Assert
            Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(task));
        }

        [TestMethod]
        public void CreateTask_ShouldThrowException_WhenTaskDueDateIsInPast()
        {
            // Arrange
            var task = new Task
            {
                Title = "Valid title",
                Description = "Valid description",
                DueDate = DateTime.Now.AddDays(-1)
            };

            // Act & Assert
            Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(task));
        }

        [TestMethod]
        public void CreateTask_ShouldAddTask_WhenTaskIsValid()
        {
            // Arrange
            var task = new Task
            {
                Title = "Valid title",
                Description = "Valid description",
                DueDate = DateTime.Now.AddDays(1)
            };

            _mockTaskRepository.Setup(repo => repo.AddTask(It.IsAny<Task>())).Returns(task);

            // Act
            var createdTask = _taskService.CreateTask(task);

            // Assert
            Assert.AreEqual(task.Title, createdTask.Title);
            Assert.AreEqual(task.Description, createdTask.Description);
        }

        [TestMethod]
        public void GetTasksFromPanel_ShouldReturnTasks_WhenPanelIdIsValid()
        {
            // Arrange
            var panelId = 1;
            var tasks = new List<Task>
            {
                new Task { PanelId = panelId },
                new Task { PanelId = 2 }
            };
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Returns(tasks);

            // Act
            var result = _taskService.GetTasksFromPanel(panelId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(panelId, result[0].PanelId);
        }

        [TestMethod]
        public void GetTaskById_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var task = new Task { Id = 1 };
            _mockTaskRepository.Setup(repo => repo.GetTaskById(It.IsAny<int>())).Returns(task);

            // Act
            var result = _taskService.GetTaskById(1);

            // Assert
            Assert.AreEqual(task.Id, result.Id);
        }

        [TestMethod]
        public void UpdateTask_ShouldUpdateTask_WhenTaskExists()
        {
            // Arrange
            var task = new Task { Id = 1, Title = "Old Task" };
            _mockTaskRepository.Setup(repo => repo.GetTaskById(It.IsAny<int>())).Returns(task);
            _mockTaskRepository.Setup(repo => repo.UpdateTask(It.IsAny<Task>())).Returns(task);

            // Act
            var updatedTask = _taskService.UpdateTask(task);

            // Assert
            Assert.AreEqual(task.Title, updatedTask.Title);
        }

        [TestMethod]
        public void DeleteTask_ShouldMarkAsDeleted_WhenTaskIsNotDeleted()
        {
            // Arrange
            var task = new Task { Id = 1, IsDeleted = false };
            var user = new User { Id = 1, TrashId = 123 };

            _mockTaskRepository.Setup(repo => repo.GetTaskById(It.IsAny<int>())).Returns(task);

            // Act
            var deletedTask = _taskService.DeleteTask(task, user);

            // Assert
            Assert.IsTrue(deletedTask.IsDeleted);
            _mockTrashService.Verify(service => service.AddTaskToTrash(task, user.TrashId), Times.Once);
        }

        [TestMethod]
        public void DeleteTask_ShouldRemoveFromTrash_WhenTaskIsAlreadyDeleted()
        {
            // Arrange
            var task = new Task { Id = 1, IsDeleted = true };
            var user = new User { Id = 1, TrashId = 123 };

            _mockTaskRepository.Setup(repo => repo.GetTaskById(It.IsAny<int>())).Returns(task);

            // Act
            var deletedTask = _taskService.DeleteTask(task, user);

            // Assert
            Assert.IsTrue(deletedTask.IsDeleted);
            _mockTrashService.Verify(service => service.RemoveTaskFromTrash(task.Id, user.TrashId), Times.Once);
            _mockTaskRepository.Verify(repo => repo.DeleteTask(task.Id), Times.Once);
        }
        
        [TestMethod]
        public void RecoverTask_ShouldRestoreDeletedTask_WhenTaskIsInTrash()
        {
            // Arrange
            var task = new Task { Id = 1, IsDeleted = true };
            var user = new User { Id = 1, TrashId = 123 };
            _mockTaskRepository.Setup(repo => repo.GetTaskById(It.IsAny<int>())).Returns(task);
            _mockTrashService.Setup(service => service.GetTrashById(It.IsAny<int>())).Returns(new Trash
            {
                TaskList = new List<Task> { task }
            });

            // Act
            var restoredTask = _taskService.RecoverTask(task, user);

            // Assert
            Assert.IsFalse(restoredTask.IsDeleted);
        }

        [TestMethod]
        public void GetAllTasks_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task { Id = 1 },
                new Task { Id = 2 }
            };
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Returns(tasks);

            // Act
            var result = _taskService.GetAllTasks();

            // Assert
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public void GetTasksFromPanel_ShouldReturnEmptyList_WhenNoTasksExistForPanel()
        {
            // Arrange
            var panelId = 1;
            var tasks = new List<Task>();
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Returns(tasks);

            // Act
            var result = _taskService.GetTasksFromPanel(panelId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetTasksFromPanel_ShouldHandleArgumentException_WhenRepositoryThrowsError()
        {
            // Arrange
            var panelId = 1;
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Throws<ArgumentException>();

            // Act
            var result = _taskService.GetTasksFromPanel(panelId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetTasksFromEpic_ShouldReturnTasks_WhenEpicIdIsValid()
        {
            // Arrange
            var epicId = 1;
            var tasks = new List<Task>
            {
                new Task { Id = 1, EpicId = epicId },
                new Task { Id = 2, EpicId = 2 }
            };
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Returns(tasks);

            // Act
            var result = _taskService.GetTasksFromEpic(epicId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(epicId, result[0].EpicId);
        }

        [TestMethod]
        public void GetTasksFromEpic_ShouldReturnEmptyList_WhenNoTasksExistForEpic()
        {
            // Arrange
            var epicId = 1;
            var tasks = new List<Task>();
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Returns(tasks);

            // Act
            var result = _taskService.GetTasksFromEpic(epicId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetTasksFromEpic_ShouldHandleArgumentException_WhenRepositoryThrowsError()
        {
            // Arrange
            var epicId = 1;
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Throws<ArgumentException>();

            // Act
            var result = _taskService.GetTasksFromEpic(epicId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}