using System;
using AutoMapper;
using Xunit;

namespace BrewView.Server.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var mapperConfiguration = new MapperConfiguration(Startup.ConfigureMapper);
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
