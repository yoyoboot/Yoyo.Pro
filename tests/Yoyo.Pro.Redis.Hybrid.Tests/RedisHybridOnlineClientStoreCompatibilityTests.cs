using System;
using Abp.RealTime;
using Abp.Runtime.Caching.Redis;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace Yoyo.Pro;

public class RedisHybridOnlineClientStoreCompatibilityTests
{
    [Fact]
    public void TryGet_Should_FallBack_To_Json_When_Serializer_Throws()
    {
        const string connectionId = "connection-1";
        const string ipAddress = "127.0.0.1";
        const string tenantId = "42";
        const string userId = "84";
        var connectTime = new DateTime(2026, 4, 22, 12, 30, 0, DateTimeKind.Utc);
        var legacyJson = JsonConvert.SerializeObject(new
        {
            ConnectionId = connectionId,
            IpAddress = ipAddress,
            TenantId = tenantId,
            UserId = userId,
            ConnectTime = connectTime
        });

        var database = new Mock<IDatabase>();
        database
            .Setup(x => x.StringGet("n:AbpOnlineClients,c:c_connection-1", CommandFlags.None))
            .Returns((RedisValue)legacyJson);

        var databaseProvider = new Mock<IAbpRedisCacheDatabaseProvider>();
        databaseProvider.Setup(x => x.GetDatabase()).Returns(database.Object);

        var store = new RedisHybridOnlineClientStore(databaseProvider.Object, new ThrowingRedisCacheSerializer());

        var found = store.TryGet("connection-1", out var restoredClient);

        found.ShouldBeTrue();
        var onlineClient = restoredClient.ShouldBeOfType<OnlineClient>();
        onlineClient.ConnectionId.ShouldBe(connectionId);
        onlineClient.IpAddress.ShouldBe(ipAddress);
        onlineClient.TenantId.ShouldBe(tenantId);
        onlineClient.UserId.ShouldBe(userId);
        onlineClient.ConnectTime.ShouldBe(connectTime);
    }

    private sealed class ThrowingRedisCacheSerializer : IRedisCacheSerializer
    {
        public RedisValue Serialize(object value, Type type)
        {
            throw new NotSupportedException();
        }

        public object Deserialize(RedisValue objbyte)
        {
            throw new JsonException("serializer failed");
        }
    }
}