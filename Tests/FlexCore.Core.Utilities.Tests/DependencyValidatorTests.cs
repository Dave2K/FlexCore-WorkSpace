namespace FlexCore.Core.Utilities.Tests;

using FlexCore.Core.Utilities;
using System;
using System.Collections.Generic;
using Xunit;

public class DependencyValidatorTests
{
    [Fact]
    public void ValidateDependencies_ThrowsException_WhenCircularDependencyExists()
    {
        var projectDependencies = new Dictionary<string, List<string>>
        {
            { "ProjectA", new List<string> { "ProjectB" } },
            { "ProjectB", new List<string> { "ProjectA" } }
        };

        Assert.Throws<InvalidOperationException>(() => DependencyValidator.ValidateDependencies(projectDependencies));
    }

    [Fact]
    public void ValidateDependencies_DoesNotThrow_WhenNoCircularDependencyExists()
    {
        var projectDependencies = new Dictionary<string, List<string>>
        {
            { "ProjectA", new List<string> { "ProjectB" } },
            { "ProjectB", new List<string> { "ProjectC" } },
            { "ProjectC", new List<string>() }
        };

        var exception = Record.Exception(() => DependencyValidator.ValidateDependencies(projectDependencies));
        Assert.Null(exception);
    }

    [Fact]
    public void ResolveCircularDependencies_ThrowsException_WhenCircularDependencyExists()
    {
        var projectDependencies = new Dictionary<string, List<string>>
        {
            { "ProjectA", new List<string> { "ProjectB" } },
            { "ProjectB", new List<string> { "ProjectA" } }
        };

        Assert.Throws<InvalidOperationException>(() => DependencyValidator.ResolveCircularDependencies(projectDependencies));
    }

    [Fact]
    public void ResolveCircularDependencies_DoesNotThrow_WhenNoCircularDependencyExists()
    {
        var projectDependencies = new Dictionary<string, List<string>>
        {
            { "ProjectA", new List<string> { "ProjectB" } },
            { "ProjectB", new List<string> { "ProjectC" } },
            { "ProjectC", new List<string>() }
        };

        var exception = Record.Exception(() => DependencyValidator.ResolveCircularDependencies(projectDependencies));
        Assert.Null(exception);
    }
}