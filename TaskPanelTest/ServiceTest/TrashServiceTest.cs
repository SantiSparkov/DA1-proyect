using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class TrashServiceTests
    {
        private Mock<ITrashRepository> _mockTrashRepository;
        private TrashService _trashService;

        [TestInitialize]
        public void Setup()
        {
            _mockTrashRepository = new Mock<ITrashRepository>();
            _trashService = new TrashService(_mockTrashRepository.Object);
        }

        [TestMethod]
        public void CreateTrash_ShouldCreateNewTrash()
        {
            // Arrange
            var user = new User { Id = 1 };
            var newTrash = new Trash
                { UserId = user.Id, Elements = 0, TaskList = new List<Task>(), PanelList = new List<Panel>() };

            _mockTrashRepository.Setup(repo => repo.AddTrash(It.IsAny<Trash>())).Returns(newTrash);

            // Act
            var result = _trashService.CreateTrash(user);

            // Assert
            Assert.AreEqual(user.Id, result.UserId);
            Assert.AreEqual(0, result.Elements);
        }

        [TestMethod]
        public void AddTaskToTrash_ShouldAddTask_WhenTrashIsNotFull()
        {
            // Arrange
            var trash = new Trash { Id = 1, TaskList = new List<Task>(), PanelList = new List<Panel>() };
            var task = new Task { Id = 1 };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.AddTaskToTrash(task, trash.Id);

            // Assert
            Assert.AreEqual(1, trash.TaskList.Count);
            Assert.AreEqual(task.Id, trash.TaskList[0].Id);
        }

        [TestMethod]
        public void AddTaskToTrash_ShouldNotAddTask_WhenTrashIsFull()
        {
            // Arrange
            var trash = new Trash
            {
                Id = 1,
                TaskList = new List<Task> { new Task(), new Task(), new Task(), new Task(), new Task() },
                PanelList = new List<Panel> { new Panel(), new Panel(), new Panel(), new Panel(), new Panel() }
            };
            var task = new Task { Id = 1 };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.AddTaskToTrash(task, trash.Id);

            // Assert
            Assert.AreEqual(5, trash.TaskList.Count);
        }

        [TestMethod]
        public void AddPanelToTrash_ShouldAddPanel_WhenTrashIsNotFull()
        {
            // Arrange
            var trash = new Trash { Id = 1, TaskList = new List<Task>(), PanelList = new List<Panel>() };
            var panel = new Panel { Id = 1 };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);
            
            // Act
            _trashService.AddPanelToTrash(panel, trash.Id);
            
            // Assert
            Assert.AreEqual(1, trash.PanelList.Count);
            Assert.AreEqual(panel.Id, trash.PanelList[0].Id);
        }
        
        [TestMethod]
        public void AddPanelToTrash_ShouldNotAddPanel_WhenTrashIsFull()
        {
            // Arrange
            var trash = new Trash
            {
                Id = 1,
                TaskList = new List<Task> { new Task(), new Task(), new Task(), new Task(), new Task() },
                PanelList = new List<Panel> { new Panel(), new Panel(), new Panel(), new Panel(), new Panel() }
            };
            var panel = new Panel { Id = 1 };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.AddPanelToTrash(panel, trash.Id);

            // Assert
            Assert.AreEqual(5, trash.PanelList.Count);
        }

        [TestMethod]
        public void RecoverTaskFromTrash_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var task = new Task { Id = 1 };
            var trash = new Trash { Id = 1, TaskList = new List<Task> { task } };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            var result = _trashService.RecoverTaskFromTrash(task.Id, trash.Id);

            // Assert
            Assert.AreEqual(task.Id, result.Id);
            Assert.AreEqual(0, trash.TaskList.Count);
        }

        [TestMethod]
        public void RecoverTaskFromTrash_ShouldThrowException_WhenTaskNotFound()
        {
            // Arrange
            var trash = new Trash { Id = 1, TaskList = new List<Task>() };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act & Assert
            Assert.ThrowsException<TaskNotValidException>(() => _trashService.RecoverTaskFromTrash(1, trash.Id));
        }

        [TestMethod]
        public void RemovePanelFromTrash_ShouldRemovePanel_WhenPanelExists()
        {
            // Arrange
            var panel = new Panel { Id = 1 };
            var trash = new Trash { Id = 1, PanelList = new List<Panel> { panel } };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.RemovePanelFromTrash(panel.Id, trash.Id);

            // Assert
            Assert.AreEqual(0, trash.PanelList.Count);
        }

        [TestMethod]
        public void RemovePanelFromTrash_ShouldDoNothing_WhenPanelNotExists()
        {
            // Arrange
            var trash = new Trash { Id = 1, PanelList = new List<Panel>() };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.RemovePanelFromTrash(1, trash.Id);

            // Assert
            Assert.AreEqual(0, trash.PanelList.Count);
        }

        [TestMethod]
        public void IsFull_ShouldReturnTrue_WhenTrashIsFull()
        {
            // Arrange
            var trash = new Trash
            {
                Id = 1,
                TaskList = new List<Task> { new Task(), new Task(), new Task(), new Task(), new Task() },
                PanelList = new List<Panel> { new Panel(), new Panel(), new Panel(), new Panel(), new Panel() }
            };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            var result = _trashService.IsFull(trash.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsFull_ShouldReturnFalse_WhenTrashIsNotFull()
        {
            // Arrange
            var trash = new Trash
            {
                Id = 1,
                TaskList = new List<Task> { new Task(), new Task() },
                PanelList = new List<Panel> { new Panel() }
            };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            var result = _trashService.IsFull(trash.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateTrash_ShouldUpdateElementsCount()
        {
            // Arrange
            var trash = new Trash
            {
                Id = 1,
                TaskList = new List<Task> { new Task(), new Task() },
                PanelList = new List<Panel> { new Panel(), new Panel() }
            };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.UpdateTrash(trash.Id);

            // Assert
            Assert.AreEqual(4, trash.Elements);
            _mockTrashRepository.Verify(repo => repo.UpdateTrash(trash), Times.Once);
        }
        
        
        [TestMethod]
        public void RemoveTaskFromTrash_ShouldRemoveTask_WhenTaskExists()
        {
            // Arrange
            var task = new Task { Id = 1 };
            var trash = new Trash { Id = 1, TaskList = new List<Task> { task } };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.RemoveTaskFromTrash(task.Id, trash.Id);

            // Assert
            Assert.AreEqual(0, trash.TaskList.Count);
            _mockTrashRepository.Verify(repo => repo.UpdateTrash(trash), Times.Once);
        }

        [TestMethod]
        public void RemoveTaskFromTrash_ShouldDoNothing_WhenTaskDoesNotExist()
        {
            // Arrange
            var trash = new Trash { Id = 1, TaskList = new List<Task>() };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            _trashService.RemoveTaskFromTrash(1, trash.Id);

            // Assert
            Assert.AreEqual(0, trash.TaskList.Count);
            _mockTrashRepository.Verify(repo => repo.UpdateTrash(It.IsAny<Trash>()), Times.Never);
        }

        [TestMethod]
        public void RecoverPanelFromTrash_ShouldReturnPanel_WhenPanelExists()
        {
            // Arrange
            var panel = new Panel { Id = 1 };
            var trash = new Trash { Id = 1, PanelList = new List<Panel> { panel } };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            var result = _trashService.RecoverPanelFromTrash(panel.Id, trash.Id);

            // Assert
            Assert.AreEqual(panel.Id, result.Id);
            Assert.AreEqual(0, trash.PanelList.Count);
        }

        [TestMethod]
        public void RecoverPanelFromTrash_ShouldThrowException_WhenPanelNotFound()
        {
            // Arrange
            var trash = new Trash { Id = 1, PanelList = new List<Panel>() };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act & Assert
            Assert.ThrowsException<PanelNotValidException>(() => _trashService.RecoverPanelFromTrash(1, trash.Id));
        }

        [TestMethod]
        public void DeleteTrash_ShouldDeleteTrash_WhenTrashExists()
        {
            // Arrange
            var trashId = 1;

            _mockTrashRepository.Setup(repo => repo.DeleteTrashForId(trashId));

            // Act
            _trashService.DeleteTrash(trashId);

            // Assert
            _mockTrashRepository.Verify(repo => repo.DeleteTrashForId(trashId), Times.Once);
        }

        [TestMethod]
        public void GetTrashById_ShouldReturnTrash_WhenTrashExists()
        {
            // Arrange
            var trash = new Trash { Id = 1 };

            _mockTrashRepository.Setup(repo => repo.GetTrashById(trash.Id)).Returns(trash);

            // Act
            var result = _trashService.GetTrashById(trash.Id);

            // Assert
            Assert.AreEqual(trash.Id, result.Id);
        }

        [TestMethod]
        public void GetTrashById_ShouldReturnNull_WhenTrashDoesNotExist()
        {
            // Arrange
            _mockTrashRepository.Setup(repo => repo.GetTrashById(It.IsAny<int>())).Returns((Trash)null);

            // Act
            var result = _trashService.GetTrashById(1);

            // Assert
            Assert.IsNull(result);
        }
    }
}