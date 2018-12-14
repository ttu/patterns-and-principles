using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PatternsAndPrinciples.Patterns.Other
{
    /*
     * Simple IoC container implemenatation
     *
     * Based on: https://www.codeproject.com/Articles/347651/Define-your-own-IoC-container
     * See more complete example: https://github.com/Microsoft/MinIoC
     */

    public class Container
    {
        private readonly IDictionary<Type, Type> _types = new Dictionary<Type, Type>();

        private readonly IDictionary<Type, object> _typeInstances = new Dictionary<Type, object>();

        private readonly IDictionary<Type, Func<object>> _typedActivators = new Dictionary<Type, Func<object>>();

        public void Register<TContract, TImplementation>()
            where TContract : class
            where TImplementation : class
        {
            _types[typeof(TContract)] = typeof(TImplementation);
        }

        public void Register<TContract, TImplementation>(TImplementation instance)
            where TContract : class
            where TImplementation : class
        {
            _typeInstances[typeof(TContract)] = instance;
        }

        public void Register<TContract>(Func<TContract> activator)
            where TContract : class
        {
            _typedActivators[typeof(TContract)] = activator;
        }

        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        private object Resolve(Type contract)
        {
            if (_typeInstances.ContainsKey(contract))
            {
                return _typeInstances[contract];
            }

            if (_typedActivators.ContainsKey(contract))
            {
                return _typedActivators[contract].Invoke();
            }

            var implementation = _types[contract];
            var constructor = implementation.GetConstructors()[0];
            var constructorParameters = constructor.GetParameters();

            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementation);
            }

            var parameters = constructorParameters.Select(e => Resolve(e.ParameterType)).ToArray();
            return constructor.Invoke(parameters);
        }
    }

    public class ContainerTest
    {
        [Fact]
        public void Test()
        {
            var container = new Container();

            container.Register<IPeopleService, PeopleService>();

            var peopleService = container.Resolve<IPeopleService>();
            var peopleService2 = container.Resolve<IPeopleService>();
            Assert.NotSame(peopleService, peopleService2);

            container.Register<IStoreService>(() => new StoreService());

            var storeService = container.Resolve<IStoreService>();
            Assert.NotNull(storeService);

            var messageBus = new MessageBus();
            container.Register<IMessageBus, MessageBus>(messageBus);

            var busFromContainer = container.Resolve<IMessageBus>();
            Assert.Same(messageBus, busFromContainer);

            container.Register<ISenderService, SenderService>();

            var senderService = container.Resolve<ISenderService>();
            Assert.Same(messageBus, senderService.Bus);
        }
    }

    public interface IPeopleService
    { }

    public class PeopleService : IPeopleService
    { }

    public interface IStoreService
    { }

    public class StoreService : IStoreService
    { }

    public interface ISenderService
    {
        IMessageBus Bus { get; }
    }

    public class SenderService : ISenderService
    {
        public SenderService(IMessageBus bus) => Bus = bus;

        public IMessageBus Bus { get; private set; }
    }

    public interface IMessageBus
    { }

    public class MessageBus : IMessageBus
    { }
}