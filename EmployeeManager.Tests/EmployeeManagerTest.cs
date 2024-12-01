using Xunit;
using Moq;
using EmployeeManager.Controllers;
using EmployeeManager.Models;
using Microsoft.AspNetCore.Mvc;

public class EmployeeControllerTest
{
    private readonly Mock<IEmployeeRepository> _mockRepo;
    private readonly EmployeeController _controller;

    public EmployeeControllerTest()
    {
        _mockRepo = new Mock<IEmployeeRepository>();
        _controller = new EmployeeController(_mockRepo.Object);
    }

    [Fact]
    public void Delete_EmployeeExists_ReturnsViewResultWithEmployee()
    {
        // Arrange
        var employee = new Employee { 
            Id = 1,
            FirstName = "Jan",
            LastName = "Kowalski",
            PESEL = "12345678901",
            Department = "Lorem",
            Position = "Ipsum"
            };
        _mockRepo.Setup(repo => repo.GetById(1)).Returns(employee);

        // Act
        var result = _controller.Delete(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Employee>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Id);
    }

    [Fact]
    public void Delete_EmployeeDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var employeeId = 1;
        _mockRepo.Setup(repo => repo.GetById(employeeId)).Returns((Employee)null);

        // Act
        var result = _controller.Delete(employeeId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public void Index_ShouldReturnViewWithEmployees()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "John", LastName = "Doe", PESEL = "12345678901", Department = "HR", Position = "Manager" },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", PESEL = "98765432109", Department = "IT", Position = "Developer" }
        };
        _mockRepo.Setup(repo => repo.GetAll()).Returns(employees);

        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var returnedEmployees = Assert.IsAssignableFrom<List<Employee>>(result.Model);
        Assert.Equal(2, returnedEmployees.Count);
    }

    [Fact]
    public void Details_ShouldReturnView_WhenEmployeeExists()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901",
            Department = "HR",
            Position = "Manager"
        };
        _mockRepo.Setup(repo => repo.GetById(1)).Returns(employee);

        // Act
        var result = _controller.Details(1) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var returnedEmployee = Assert.IsAssignableFrom<Employee>(result.Model);
        Assert.Equal("John", returnedEmployee.FirstName);
    }

    [Fact]
    public void Details_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Employee?)null);

        // Act
        var result = _controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_ShouldAddEmployee_WhenModelIsValid()
    {
        // Arrange
        var employee = new Employee
        {
            FirstName = "Alice",
            LastName = "Johnson",
            PESEL = "11223344556",
            Department = "QA",
            Position = "Tester"
        };

        _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Employee>());

        // Act
        var result = _controller.Create(employee) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        _mockRepo.Verify(repo => repo.Add(It.IsAny<Employee>()), Times.Once);
    }


    [Fact]
    public void Create_ShouldReturnView_WhenModelStateIsInvalid()
    {
        // Arrange
        var employee = new Employee
        {
            FirstName = "Bob",
            LastName = "", 
            Department = "Finance",
            Position = "Analyst",
            PESEL = "99887766554"
        };
        _controller.ModelState.AddModelError("LastName", "Required");

        // Act
        var result = _controller.Create(employee) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Create_Post_ShouldNotAddEmployee_WhenModelStateIsInvalid()
    {
        // Arrange
        var employee = new Employee
        {
            FirstName = "Alice",
            LastName = "", 
            PESEL = "11223344556",
            Department = "QA",
            Position = "Tester"
        };
        _controller.ModelState.AddModelError("LastName", "Required");

        // Act
        var result = _controller.Create(employee) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.False(_controller.ModelState.IsValid);
        _mockRepo.Verify(repo => repo.Add(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public void Edit_ShouldReturnView_WhenEmployeeExists()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901",
            Department = "HR",
            Position = "Manager"
        };
        _mockRepo.Setup(repo => repo.GetById(1)).Returns(employee);

        // Act
        var result = _controller.Edit(1) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var returnedEmployee = Assert.IsAssignableFrom<Employee>(result.Model);
        Assert.Equal("John", returnedEmployee.FirstName);
    }

    [Fact]
    public void Edit_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Employee?)null);

        // Act
        var result = _controller.Edit(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_ShouldUpdateEmployee_WhenModelIsValid()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901",
            Department = "HR",
            Position = "Manager"
        };

        // Act
        var result = _controller.Edit(employee) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        _mockRepo.Verify(repo => repo.Update(employee), Times.Once);
    }

    [Fact]
    public void Edit_Post_ShouldReturnView_WhenModelStateIsInvalid()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "",
            Department = "HR",
            Position = "Manager",
            PESEL = "12345678901"
        };
        _controller.ModelState.AddModelError("LastName", "Required");

        // Act
        var result = _controller.Edit(employee) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Edit_Post_ShouldNotUpdateEmployee_WhenModelStateIsInvalid()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            FirstName = "Bob",
            LastName = "", // Invalid LastName
            PESEL = "99887766554",
            Department = "Finance",
            Position = "Analyst"
        };
        _controller.ModelState.AddModelError("LastName", "Required");

        // Act
        var result = _controller.Edit(employee) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.False(_controller.ModelState.IsValid);
        _mockRepo.Verify(repo => repo.Update(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public void GetPositions_ReturnsJsonResult_WithPositions()
    {
        // Arrange
        var department = "IT";
        var positions = new List<string> { "Developer", "Tester", "Manager" };
        Positions.DepartmentPositions = new Dictionary<string, List<string>>
        {
            { "IT", positions }
        };

        // Act
        var result = _controller.GetPositions(department) as JsonResult;

        // Assert
        Assert.NotNull(result);
        var returnedPositions = Assert.IsAssignableFrom<List<string>>(result.Value);
        Assert.Equal(positions.Count, returnedPositions.Count);
    }

    [Fact]
    public void GetPositions_ReturnsEmptyList_WhenDepartmentNotFound()
    {
        // Arrange
        var department = "Unknown";
        Positions.DepartmentPositions = new Dictionary<string, List<string>>
        {
            { "IT", new List<string> { "Developer", "Tester", "Manager" } }
        };

        // Act
        var result = _controller.GetPositions(department) as JsonResult;

        // Assert
        Assert.NotNull(result);
        var returnedPositions = Assert.IsAssignableFrom<List<string>>(result.Value);
        Assert.Empty(returnedPositions);
    }

    [Fact]
    public void GetEmployees_ShouldReturnJsonResult_WithEmployees()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "John", LastName = "Doe", PESEL = "12345678901", Department = "HR", Position = "Manager" }
        };
        _mockRepo.Setup(repo => repo.GetAll()).Returns(employees);

        // Act
        var result = _controller.GetEmployees() as JsonResult;

        // Assert
        Assert.NotNull(result);
        var returnedEmployees = Assert.IsAssignableFrom<List<Employee>>(result.Value);
        Assert.Single(returnedEmployees);
    }
}