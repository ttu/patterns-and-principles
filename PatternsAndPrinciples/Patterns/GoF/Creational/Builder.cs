using System.Collections.Generic;
using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Creational
{
    /*
     * Constructs complex objects by separating construction and representation
     * 
     * Construction with builders and representation with IDeviceModules
     */

    public interface IDeviceModule
    {
    }

    public class DeviceModule : IDeviceModule
    {
        public DeviceModule(string id) => Id = id;

        public string Id { get; private set; }
    }

    public class LaboratoryDevice
    {
        public LaboratoryDevice(IDeviceModule baseModule, IDeviceModule optics)
        {
        }
    }

    public class LaboratoryDeviceBuilder
    {
        private IDeviceModule _baseModule;
        private IDeviceModule _opticsModule;

        public LaboratoryDeviceBuilder AddBaseModule(IDeviceModule module)
        {
            // Validate base module
            _baseModule = module;
            return this;
        }

        public LaboratoryDeviceBuilder AddOpticalModule(IDeviceModule module)
        {
            // Validate optical module
            _opticsModule = module;
            return this;
        }

        public LaboratoryDeviceBuilder AddModule(IDeviceModule module)
        {
            // Check module type
            // Call corret Add method
            return this;
        }

        public LaboratoryDevice Build()
        {
            // Validate all modules
            return new LaboratoryDevice(_baseModule, _opticsModule);
        }
    }

    public class BuilderTest
    {
        [Fact]
        public void Build_Test()
        {
            var device = new LaboratoryDeviceBuilder()
                            .AddBaseModule(new DeviceModule("base45"))
                            .AddOpticalModule(new DeviceModule("op42"))
                            .Build();
        }

        [Fact]
        public void DelayedBuild_Test()
        {
            // Communicate to real device to get found module info

            var deviceBuilderDelayed = new LaboratoryDeviceBuilder();

            foreach (var foundModule in GetDeviceInfo("1234"))
            {
                deviceBuilderDelayed.AddModule(foundModule);
            }

            var device = deviceBuilderDelayed.Build();
        }

        private IEnumerable<IDeviceModule> GetDeviceInfo(string deviceId)
        {
            // Get connection to device
            // Map received device module information data to IDeviceModule
            yield return new DeviceModule("op23");
            yield return new DeviceModule("baseAA");
        }
    }
}