namespace Simple.HA.UnitTests.ExtensionsTests;

using Simple.HAApi.Extensions;
using System.Linq;
using Xunit;

public class ChunkTests
{
    [Fact]
    public void Chunk_ArraySmallerThanChunkSize_ReturnsWholeArray()
    {
        int[] input = { 1, 2, 3 };
        var result = NetExtensions.Chunk(input, 5).ToList();

        Assert.Single(result);
        Assert.Equal(input, result[0]);
    }

    [Fact]
    public void Chunk_ArrayExactlyChunkSize_ReturnsWholeArray()
    {
        int[] input = { 1, 2, 3 };
        var result = NetExtensions.Chunk(input, 3).ToList();

        Assert.Single(result);
        Assert.Equal(input, result[0]);
    }

    [Fact]
    public void Chunk_ArrayLargerThanChunkSize_ReturnsChunks()
    {
        int[] input = { 1, 2, 3, 4, 5, 6, 7 };
        var result = NetExtensions.Chunk(input, 3).ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { 1, 2, 3 }, result[0]);
        Assert.Equal(new[] { 4, 5, 6 }, result[1]);
        Assert.Equal(new[] { 7 }, result[2]);
    }
}
