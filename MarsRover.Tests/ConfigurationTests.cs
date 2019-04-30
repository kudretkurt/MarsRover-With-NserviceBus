using MarsRover.Shared.Configuration;
using Xunit;

namespace MarsRover.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        [Trait("ConfigurationTests", "GetValue")]
        public void GetValue_Must_Work_Correctly()
        {
            var transportUserName = ApplicationConfiguration.Instance.GetValue<string>("ServiceBus:TransportUserName");
            Assert.Equal("mars",transportUserName);
        }
    }
}
