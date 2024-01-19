using FluentAssertions;
using Microsoft.Extensions.FileProviders;
using Moq;

namespace Craft.Extensions.Tests.FileInfo;

public class FileInfoTests
{
    [Fact]
    public void ContentType_ShouldReturnGif_WhenImageExtensionIsGif()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("Fake.Gif");

        // Act
        var result = fileInfo.ContentType();

        // Assert
        result.Should().Be("image/gif");
    }

    [Fact]
    public void ContentType_ShouldReturnJpeg_WhenImageExtensionIsJpg()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("Fake.Jpg");

        // Act
        var result = fileInfo.ContentType();

        // Assert
        result.Should().Be("image/jpeg");
    }

    [Fact]
    public void ContentType_ShouldReturnOctetStream_WhenImageExtensionIsUnknown()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("Fake.xsl");

        // Act
        var result = fileInfo.ContentType();

        // Assert
        result.Should().Be("application/octet-stream");
    }

    [Fact]
    public void ContentType_ShouldReturnPng_WhenImageExtensionIsPng()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("Fake.Png");

        // Act
        var result = fileInfo.ContentType();

        // Assert
        result.Should().Be("image/png");
    }

    [Fact]
    public void ContentType_ShouldReturnSvg_WhenImageExtensionIsSvg()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("Fake.Svg");

        // Act
        var result = fileInfo.ContentType();

        // Assert
        result.Should().Be("image/svg");
    }

    [Fact]
    public void Extension_ShouldReturnEmptyString_WhenNoExtensionExists()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("file");

        // Act
        var result = fileInfo.Extension();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Extension_ShouldReturnLowerCaseExtension_WhenExtensionExists()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("example.txt");

        // Act
        var result = fileInfo.Extension();

        // Assert
        result.Should().Be(".txt");
    }

    [Fact]
    public void Extension_ShouldReturnLowerCaseExtension_WhenExtensionExistsWithLeadingDot()
    {
        // Arrange
        var fileInfo = new FakeFileInfo(".document");

        // Act
        var result = fileInfo.Extension();

        // Assert
        result.Should().Be(".document");
    }

    [Fact]
    public void Extension_ShouldReturnLowerCaseExtension_WhenExtensionExistsWithMixedCase()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("file.PDF");

        // Act
        var result = fileInfo.Extension();

        // Assert
        result.Should().Be(".pdf");
    }

    [Fact]
    public void Extension_ShouldReturnLowerCaseExtension_WhenMultipleDotsExist()
    {
        // Arrange
        var fileInfo = new FakeFileInfo("file.test.txt");

        // Act
        var result = fileInfo.Extension();

        // Assert
        result.Should().Be(".txt");
    }

    [Fact]
    public void GetFileExtensions()
    {
        var txt = Mock.Of<IFileInfo>(f => f.Name == "test.txt");
        Assert.Equal(".txt", txt.Extension());
    }

    [Fact]
    public void GetImageExtension_ShouldReturnGif_WhenExtensionIsGif()
    {
        // Arrange
        var fileInfo = new FakeFileInfo(".gif");

        // Act
        var result = fileInfo.GetImageExtension();

        // Assert
        result.Should().Be(ImageExtension.Gif);
    }

    [Fact]
    public void GetImageExtension_ShouldReturnJpg_WhenExtensionIsJpg()
    {
        // Arrange
        var fileInfo = new FakeFileInfo(".jpg");

        // Act
        var result = fileInfo.GetImageExtension();

        // Assert
        result.Should().Be(ImageExtension.Jpg);
    }

    [Fact]
    public void GetImageExtension_ShouldReturnPng_WhenExtensionIsPng()
    {
        // Arrange
        var fileInfo = new FakeFileInfo(".png");

        // Act
        var result = fileInfo.GetImageExtension();

        // Assert
        result.Should().Be(ImageExtension.Png);
    }

    [Fact]
    public void GetImageExtension_ShouldReturnSvg_WhenExtensionIsSvg()
    {
        // Arrange
        var fileInfo = new FakeFileInfo(".svg");

        // Act
        var result = fileInfo.GetImageExtension();

        // Assert
        result.Should().Be(ImageExtension.Svg);
    }

    [Fact]
    public void GetImageExtension_ShouldReturnUnknown_WhenExtensionIsNotRecognized()
    {
        // Arrange
        var fileInfo = new FakeFileInfo(".txt");

        // Act
        var result = fileInfo.GetImageExtension();

        // Assert
        result.Should().Be(ImageExtension.Unknown);
    }

    [Fact]
    public void PngImageExtensions()
    {
        var png = Mock.Of<IFileInfo>(f => f.Name == "test.png");
        Assert.Equal(".png", png.Extension());
        Assert.Equal(ImageExtension.Png, png.GetImageExtension());
        Assert.Equal("image/png", png.ContentType());
    }

    [Fact]
    public void SvgImageExtensions()
    {
        var svg = Mock.Of<IFileInfo>(f => f.Name == "test.svg");
        Assert.Equal(".svg", svg.Extension());
        Assert.Equal(ImageExtension.Svg, svg.GetImageExtension());
        Assert.Equal("image/svg", svg.ContentType());
    }

    [Fact]
    public void UnknownImageExtensions()
    {
        var webp = Mock.Of<IFileInfo>(f => f.Name == "test.webp");
        Assert.Equal(".webp", webp.Extension());
        Assert.Equal(ImageExtension.Unknown, webp.GetImageExtension());
        Assert.Equal("application/octet-stream", webp.ContentType());
    }
}

public class FakeFileInfo(string name) : IFileInfo
{
    public bool Exists => throw new NotImplementedException();

    public bool IsDirectory { get; }
    public DateTimeOffset LastModified { get; }
    public long Length => throw new NotImplementedException();

    public string Name { get; } = name;
    public string PhysicalPath => throw new NotImplementedException();

    public Stream CreateReadStream()
    {
        throw new NotImplementedException();
    }
}
