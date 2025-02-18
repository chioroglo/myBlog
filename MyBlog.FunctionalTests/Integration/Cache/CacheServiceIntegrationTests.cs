using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Common.Options;
using MyBlog.Common.Utils;
using MyBlog.Service;
using MyBlog.Service.Abstract;

namespace MyBlog.FunctionalTests.Integration.Cache;

public class CacheServiceFunctionalTests : IClassFixture<CacheTestWebAppFactory>
{
    private readonly RedisDistributedCacheService _redisService;
    private readonly InMemoryCacheService _inMemoryService;

    public CacheServiceFunctionalTests(CacheTestWebAppFactory factory)
    {
        this._redisService = factory.Services.GetRequiredService<RedisDistributedCacheService>()
                               ?? throw new ArgumentNullException();
        this._inMemoryService = factory.Services.GetRequiredService<InMemoryCacheService>()
                                ?? throw new ArgumentNullException();
    }

    [Theory]
    [InlineData(CacheProvider.InMemory)]
    [InlineData(CacheProvider.Redis)]
    public async Task SetAndGetStringAsync_ShouldStoreExactSameValue(CacheProvider provider)
    {
        // Arrange
        var subject = ResolveSubject(provider);
        const string value = "cached-value";
        var key = Guid.NewGuid().ToString();

        // Act
        await subject.SetAsync<string>(key, value, TimeSpan.FromDays(1));
        var actual = await subject.GetStringAsync(key);

        // Assert
        actual.Should().Be(value);
    }

    [Theory]
    [InlineData(CacheProvider.InMemory)]
    [InlineData(CacheProvider.Redis)]
    public async Task SetAndGetAsync_ShouldHandleSerializingAndDeserializingObjects(CacheProvider provider)
    {
        // Arrange
        var subject = ResolveSubject(provider);
        var key = Guid.NewGuid().ToString();
        var value = new TestObject
        {
            TestString = "strstrstr",
            TestInt32 = 32,
            NestedObject = new TestNestedObject
            {
                TestNestedString = "nested-string",
                TestInt32 = 2000
            },
            TestCollection = [ "value1", "value2" ],
            TestDictionary = new Dictionary<string, int>
            { 
                { "key1", 200 },
                { "key2", -400 },
                { "key3", 0 }
            }
        };

        // Act
        //  Copy to avoid referencing the same value in memory
        var expected = CopyUtils.DeepCopyJson(value);
        await subject.SetAsync<TestObject>(key, expected, TimeSpan.FromDays(1));
        var actual = await subject.GetAsync<TestObject>(key);

        // Assert
        actual.Should().BeEquivalentTo(value);
    }

    private ICacheService ResolveSubject(CacheProvider provider) => provider switch
    {
        CacheProvider.InMemory => _inMemoryService,
        CacheProvider.Redis => _redisService,
        _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
    };
}

public class TestObject
{
    public string TestString { get; set; }
    public int TestInt32 { get; set; }
    public TestNestedObject NestedObject { get; set; }
    public string[] TestCollection { get; set; }
    public Dictionary<string, int> TestDictionary { get; set; }
}

public class TestNestedObject
{
    public string TestNestedString { get; set; }
    public int TestInt32 { get; set; }
}