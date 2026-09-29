using POE_CoffeeNChill.Models;
using Xunit;

namespace POE_CoffeeNChill.Tests;

// Unit tests for the MenuCategoryHelper class.These tests help verify that the menu category values are correctly
// validated before they are used by the application.

public class MenuCategoryHelperTests
{
    // Thi class verifies that the Normalize method returns an empty string when the supplied category is null.
    // This makes sure that the application can safely handle a missing category without producing an exception.
    
    [Fact]
    public void Normalize_ReturnsEmptyString_WhenCategoryIsNull()
    {
        // Arrange - to create a null category value
        string? category = null;

        // Act - to pass the category to the Normalize method.
        string result = MenuCategoryHelper.Normalize(category);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Normalize_ReplacesUnderscoreAndTrimsWhitespace()
    {
        // Verifies that the Normalize method replaces underscores
        // with spaces and removes unnecessary whitespace.

       // For example : " Hot_Drinks  " should become "Hot Drinks".

        string category = "  Hot_Drinks  ";
        string result = MenuCategoryHelper.Normalize(category);

        // Assert - a null category should be converted to an empty string.
        Assert.Equal("Hot Drinks", result);
    }

    [Fact]
    public void Normalize_DecodesUrlEncodedCategory()
    {
        // This verifies that the URL encoded characters in a menu category are correctly decoded.
        string category = "Hot%20Drinks";

        string result = MenuCategoryHelper.Normalize(category);
        Assert.Equal("Hot Drinks", result);
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenCategoryContainsNoInvalidCharacters()
    {
        // this verifies that a normal menu category which contain no not allowed characters is considered valid.

        string category = "Hot Drinks";
        bool result = MenuCategoryHelper.IsValid(category);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenCategoryContainsInvalidCharacter()
    {
        // Verifies that a category containing a forward slash is rejected
        string category = "Hot/Drinks";
        bool result = MenuCategoryHelper.IsValid(category);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenCategoryIsEmpty()
    {
        // Verifies and makes sure that an empty category is rejected.
        string category = "";
        bool result = MenuCategoryHelper.IsValid(category);
        Assert.False(result);
    }
}