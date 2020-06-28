using System.Diagnostics;
using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Behavioral
{
    /*
     * Allows an object to alter its behavior when its internal state changes.
     */

    public class VacuumCleaner
    {
        public IState Off;
        public IState On;
        public IState Ready;
        public IState Work;
        public IState Pause;

        private IState _currentState;

        public VacuumCleaner()
        {
            Off = new OffState(this);
            On = new OnState(this);
            Ready = new ReadyState(this);
            Work = new WorkState(this);
            Pause = new PauseState(this);

            _currentState = Off;
        }

        public string CurrentState => _currentState.GetType().ToString();

        public bool Initializing { get; internal set; }

        public void StartButton() => _currentState.StartRequest();

        public void StopButton() => _currentState.StopRequest();

        internal void SetState(IState nextState)
        {
            _currentState = nextState;
            _currentState.Handle();
        }

        internal void SetLed(string color)
        {
            Trace.WriteLine($"State: {color}");
        }

        internal void SendCommands(string commandParameters)
        {
        }
    }

    public interface IState
    {
        void StartRequest();

        void StopRequest();

        void Handle();
    }

    public class OffState : IState
    {
        private readonly VacuumCleaner _device;

        public OffState(VacuumCleaner device) => _device = device;

        public void StartRequest()
        {
            _device.SetState(_device.On);
        }

        public void StopRequest()
        {
            // Device off. Can't stop
        }

        public void Handle()
        {
            _device.SendCommands("XXXXXXX OFF XXXXX");
            _device.SetLed("OFF");
        }
    }

    public class OnState : IState
    {
        private readonly VacuumCleaner _device;

        public OnState(VacuumCleaner device) => _device = device;

        public void StartRequest()
        {
            // Device will change state when it is ready
        }

        public void StopRequest()
        {
            _device.SetState(_device.Off);
        }

        public void Handle()
        {
            _device.SetLed("ON");

            if (_device.Initializing) return;

            // Start warming up etc.
            _device.SendCommands("X0\\SS ON");

            _device.Initializing = true;

            //Task.Run(async () =>
            //{
            //    // Wait for device responses
            //    // After initialization is done, se to ready
            //    await Task.Delay(1000);

            //    _device.Initializing = false;
            //    _device.SetState(_device.Ready);
            //});

            _device.Initializing = false;
            _device.SetState(_device.Ready);
        }
    }

    public class ReadyState : IState
    {
        private readonly VacuumCleaner _device;

        public ReadyState(VacuumCleaner device) => _device = device;

        public void StartRequest()
        {
            _device.SetState(_device.Work);
        }

        public void StopRequest()
        {
            _device.SetState(_device.Off);
        }

        public void Handle()
        {
            _device.SendCommands("X0\\SS WORK WORK");
            _device.SetLed("READY");
        }
    }

    public class WorkState : IState
    {
        private readonly VacuumCleaner _device;

        public WorkState(VacuumCleaner device) => _device = device;

        public void StartRequest()
        {
            // Working... Nothing to do
        }

        public void StopRequest()
        {
            _device.SetState(_device.Pause);
        }

        public void Handle()
        {
            // Start executing some commands
            _device.SetLed("WORK");
        }
    }

    public class PauseState : IState
    {
        private readonly VacuumCleaner _device;

        public PauseState(VacuumCleaner device) => _device = device;

        public void StartRequest()
        {
            _device.SetState(_device.Work);
        }

        public void StopRequest()
        {
            _device.SetState(_device.Off);
        }

        public void Handle()
        {
            _device.SetLed("PAUSE");
        }
    }

    public class StateTests
    {
        [Fact]
        public void CommandsWithFunctions()
        {
            var vacuum = new VacuumCleaner();
            vacuum.StartButton();
            vacuum.StartButton();
            vacuum.StartButton();
            vacuum.StopButton();
            vacuum.StartButton();
            vacuum.StopButton();
            vacuum.StopButton();
        }
    }

    // Example has a vacuum cleaner that can be controlled with 2 buttons
    // Buttons handle state changes
    // State can be changed either from inside the state Execute function or outside from some manager class

    // Simple game state implementation
    // https://github.com/ttu/Strategy-Game/tree/master/GameEngine/States
}