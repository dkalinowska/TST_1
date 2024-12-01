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
}