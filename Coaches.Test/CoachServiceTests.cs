using System;
using System.Collections.Generic;
using Coaches.Infrastructure;
using Coaches.MainApp.Data;
using Coaches.MainApp.Models;
using Coaches.MainApp.Repositories;
using Coaches.MainApp.Services;
using Coaches.MainApp.Services.Implementations;
using FluentAssertions;
using Moq;
using Xunit;

namespace Coaches.Test
{
    public class CoachServiceTests
    {
        [Fact]
        public void GetCoachList_IsSuccess()
        {
            var productRepositoryMock = new Mock<ICoachRepository>();
            productRepositoryMock.Setup(repository => repository.GetCoaches()).Returns(new List<Coach>());
            var trackingLogsService = new Mock<ITrackingLogsService>();
            //trackingLogsService.Setup(service=> service.SendEvent())

            var sut = new CoachService(productRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.GetCoachList().Should().Match(response => response.As<ServiceResponse>().IsSuccess);
        }
    }
}
