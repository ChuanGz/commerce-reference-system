using System;
using System.Collections.Generic;
using OrderService.Application.Commands;
using OrderService.Application.Validators;
using Xunit;

namespace OrderService.UnitTests;

public class CreateOrderCommandValidatorTests {
    private readonly CreateOrderCommandValidator _validator = new();

    [Fact]
    public void Invalid_WhenRequiredFieldsMissing() {
        var command = new CreateOrderCommand(
            Guid.Empty,
            "",
            new List<OrderItemDto>()
        );

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CustomerId");
        Assert.Contains(result.Errors, e => e.PropertyName == "ShippingAddress");
        Assert.Contains(result.Errors, e => e.PropertyName == "OrderItems");
    }

    [Fact]
    public void Valid_WhenPayloadIsCorrect() {
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            "123 Test Street, City, Country",
            new List<OrderItemDto> { new(Guid.NewGuid(), 1, 10) }
        );

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }
}
