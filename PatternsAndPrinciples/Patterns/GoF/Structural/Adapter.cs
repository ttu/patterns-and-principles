using System.Collections.Generic;
using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Structural
{
    /*
     * The primary purpose of the adapter pattern is to change the interface of class/library A to the expectations of client B.
     * The typical implementation is a wrapper class or set of classes.
     * 
     * The purpose is not to facilitate future interface changes, but current interface incompatibilities.
     */

    public interface IOwnDevice
    {
        bool ExecuteCommand(int cmd);
    }

    public class DeviceOwn : IOwnDevice
    {
        public bool ExecuteCommand(int cmd)
        {
            // Open Connection
            // Execute command
            // Close connection
            return true;
        }
    }

    // 3rd party device has different API
    // This is imported e.g. from another dll
    public class ThirdPartyDevice
    {
        public void OpenConnection()
        {
        }

        public int ExecuteCommand(int cmd)
        {
            return 0;
        }

        public void EndConnection()
        {
        }
    }

    internal class Adapter : IOwnDevice
    {
        private readonly ThirdPartyDevice _device;

        public Adapter(ThirdPartyDevice device) => _device = device;

        public bool ExecuteCommand(int cmd)
        {
            _device.OpenConnection();
            var returnValue = _device.ExecuteCommand(cmd);
            _device.EndConnection();
            return returnValue >= 0;
        }
    }

    public class AdapterTests
    {
        [Fact]
        public void Test()
        {
            var ownDevice = new DeviceOwn();

            var otherDevice = new ThirdPartyDevice();
            var adapter = new Adapter(otherDevice);

            var devices = new List<IOwnDevice> { ownDevice, adapter };

            foreach (var device in devices)
            {
                var success = device.ExecuteCommand(1);
                Assert.True(success);
            }
        }
    }
}