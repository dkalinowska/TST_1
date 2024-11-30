#nullable enable
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EmployeeManager.Controllers;
using EmployeeManager.Models;
using System.Collections.Generic;

public class EmployeeControllerTest
{
    [Fact]
    public void Delete_WithRightData_DeletesAnUser()
    {
        // Arrange
        var mockRepo = new Mock<IEmployeeRepository>();
        var employee = new Employee 
        { 
            Id = 1, 
            FirstName = "Jan", 
            LastName = "Kowalski", 
            PESEL = "12345678901", 
            Department = "Lorem", 
            Position = "Ipsum" 
        };
        mockRepo.Setup(repo => repo.GetById(1)).Returns(employee);
        var controller = new EmployeeController(mockRepo.Object);

        // Act
        var result = controller.DeleteConfirmed(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public void Delete_WithNonExistentUser_ReturnsNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IEmployeeRepository>();
        mockRepo.Setup(repo => repo.GetById(1)).Returns((Employee?)null);
        var controller = new EmployeeController(mockRepo.Object);

        // Act
        var result = controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    
}