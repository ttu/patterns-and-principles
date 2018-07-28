using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PatternsAndPinciples.Patterns.GoF.Behavioral
{
    /*
     * Encapsulate a request as an object, thereby letting you parameterize clients
     * with different requests, queue or log requests, and support undoable operations
     */

    public abstract class Device
    {
        public void TurnOn()
        {
            Trace.WriteLine("Device on");
        }

        public void TurnOff()
        {
            Trace.WriteLine("Device off");
        }
    }

    public class ExternalDisplay : Device { }

    public class LaboratoryDevice : Device
    {
        public void Measure(int protocolId)
        {
            Trace.WriteLine($"Execute mesurement protocol {protocolId}");
        }
    }

    public interface ICommand
    {
        void Execute();
    }

    public class DeviceOnCommand : ICommand
    {
        private readonly Device _device;

        public DeviceOnCommand(Device device) => _device = device;

        public void Execute() => _device.TurnOn();
    }

    public class DeviceOffCommand : ICommand
    {
        private readonly Device _device;

        public DeviceOffCommand(Device device) => _device = device;

        public void Execute() => _device.TurnOff();
    }

    public class StartMesurementProgramCommand : ICommand
    {
        private readonly LaboratoryDevice _device;
        private readonly int _protocolToExecute;

        public StartMesurementProgramCommand(LaboratoryDevice device, int protocolId)
        {
            _device = device;
            _protocolToExecute = protocolId;
        }

        public void Execute() => _device.Measure(_protocolToExecute);
    }

    public class CommandExecutor
    {
        private readonly BlockingCollection<ICommand> _commands = new BlockingCollection<ICommand>();

        public CommandExecutor()
        {
            // CommandEexecutor will execute commands in background
            Task.Run(() =>
            {
                while (true)
                {
                    _commands.TryTake(out ICommand commandToExecute);
                    commandToExecute?.Execute();
                }
            });
        }

        public bool HasCommands => _commands.Any();

        public void AddCommand(ICommand command) => _commands.TryAdd(command);
    }

    public class CommandTest
    {
        public CommandTest(ITestOutputHelper outputHelper) => Trace.Listeners.Add(new TestTraceListener(outputHelper));

        [Fact]
        public void Test()
        {
            var tv = new ExternalDisplay();
            var measurementnDevice = new LaboratoryDevice();

            var tvOnCommand = new DeviceOnCommand(tv);
            var deviceOnCommand = new DeviceOnCommand(measurementnDevice);
            var programOneCommand = new StartMesurementProgramCommand(measurementnDevice, 1);
            var programTwoCommand = new StartMesurementProgramCommand(measurementnDevice, 2);
            var deviceOffCommand = new DeviceOffCommand(measurementnDevice);

            var commandProcessor = new CommandExecutor();

            commandProcessor.AddCommand(tvOnCommand);
            commandProcessor.AddCommand(deviceOnCommand);
            commandProcessor.AddCommand(programOneCommand);
            commandProcessor.AddCommand(programTwoCommand);
            commandProcessor.AddCommand(deviceOffCommand);

            while (commandProcessor.HasCommands)
                Thread.Sleep(10);
        }
    }
}