using Xunit;
using FlexCore.Infrastructure.Commands;

namespace FlexCore.Infrastructure.Commands.Tests
{
    public class CommandBaseTests
    {
        private class TestCommand : CommandBase
        {
            public bool Executed { get; private set; }
            public override void Execute() => Executed = true;
        }

        [Fact]
        public void Execute_ShouldSetExecutedToTrue()
        {
            var command = new TestCommand();
            command.Execute();
            Assert.True(command.Executed);
        }
    }
}
