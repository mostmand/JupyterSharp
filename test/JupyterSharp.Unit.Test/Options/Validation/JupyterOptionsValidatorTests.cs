using AwesomeAssertions;
using JupyterSharp.Options;
using JupyterSharp.Options.Validation;
using JupyterSharp.Options.Validation.Errors;
using Xunit;

namespace JupyterSharp.Unit.Test.Options.Validation;

public sealed class JupyterOptionsValidatorTests
{
    private readonly IJupyterOptionsValidator _sut;

    public JupyterOptionsValidatorTests()
    {
        _sut = new JupyterOptionsValidator();
    }
    
    [Fact]
    public void Validate_ShouldIncludeNullHostError_WhenHostIsNull()
    {
        // Arrange
        var options = new JupyterOptions { Host = null! };

        // Act
        var actual = _sut.Validate(options);

        // Assert
        actual.Should().Contain(error => error is NullHostError);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_ShouldIncludeTokenNotProvidedError_WhenTokenIsNullOrWhiteSpace(string? token)
    {
        // Arrange
        var options = new JupyterOptions { Token = token! };

        // Act
        var actual = _sut.Validate(options);

        // Assert
        actual.Should().Contain(error => error is TokenNotProvidedError);
    }
    
    [Fact]
    public void Validate_ShouldIncludePortNotProvidedError_WhenPortIsNotProvided()
    {
        // Arrange
        var options = new JupyterOptions();

        // Act
        var actual = _sut.Validate(options);

        // Assert
        actual.Should().Contain(error => error is PortIsNotProvidedError);
    }
}
