namespace PostingService.UnitTests;
using StatusAPI;
using MongoDB.Driver;
using EphemeralMongo;
using FluentAssertions;
using Moq;
using StatusDefinition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Configuration;
using Moq.Protected;
using System.Text.Json;
public class GetLocationTests : BaseUnitTest
{
    [Fact]
    public void GetLocation_StandardAPICall()
    {
        // Arrange
        //No need to create data - testing data not found
        StatusRequest statusRequest = new StatusRequest(){
            IPAddress = "1.0.0.127",
            Title = "",
            Message = ""
        };
        // Act
        var location = StatusAPI.GetLocation(statusRequest, MockRedisCache, MockHttpClientFactory.Object, MockConfiguration).Result;
        // Assert
        // Tested method with data found aswell, works as expected
        location.Should().BeEquivalentTo(httpStatusLocation);
    }

    [Fact]
    public void GetLocation_IPCached()
    {
        var cachedLocation = new StatusLocation(){
                    City = "Cached",
                    Region = "Cached",
                    Country = "Cached"};
        var expectedData = JsonSerializer.SerializeToUtf8Bytes(cachedLocation);
        // Arrange
        //No need to create data - testing data not found
        MockRedisCache.Set("1.0.0.127", expectedData);
        StatusRequest statusRequest = new StatusRequest(){
            IPAddress = "1.0.0.127",
            Title = "",
            Message = ""
        };
        // Act
        var location = StatusAPI.GetLocation(statusRequest, MockRedisCache, MockHttpClientFactory.Object, MockConfiguration).Result;
        // Assert
        // Tested method with data found aswell, works as expected
        location.Should().BeEquivalentTo(cachedLocation);
    }
}