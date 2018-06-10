using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Behavioral
{
    public abstract class Device
    {
        public void TurnOn()
        {
            // Turn device on
        }

        public void TurnOff()
        {
            // Turn device off
        }
    }

    public class ExternalDisplay : Device { }

    public class LaboratoryDevice : Device
    {
        public void Measure(int protocolId)
        {
            // Execute some measurement protocol
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

    public class CommandEexecutor
    {
        private readonly BlockingCollection<ICommand> _commands = new BlockingCollection<ICommand>();

        public CommandEexecutor()
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

            var commandProcessor = new CommandEexecutor();

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