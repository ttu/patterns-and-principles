using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace PatternsAndPinciples.Patterns.GoF.Behavioral
{
    public interface IObserver
    {
        void Update(int data);
    }

    public class DbObserver : IObserver
    {
        public void Update(int data)
        {
            Trace.WriteLine($"Send data to DB: {data}");
        }
    }

    public class ApiObserver : IObserver
    {
        public void Update(int data)
        {
            Trace.WriteLine($"Send data to API: {data}");
        }
    }

    public interface ISubject
    {
        void Attach(IObserver observer);

        void Detach(IObserver observer);

        void Notify(int data);
    }

    public class Subject : ISubject
    {
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer) => _observers.Add(observer);

        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void Notify(int data) => _observers.ForEach(o => o.Update(data));
    }

    public class SensorMonitor
    {
        private readonly Subject _subject = new Subject();

        private IEnumerable<int> GetDataFromSensor()
        {
            yield return 2;
            yield return 7;
        }

        public void Start(params IObserver[] observers)
        {
            foreach (var observer in observers)
                _subject.Attach(observer);

            // Start monitoring
            foreach (var data in GetDataFromSensor())
            {
                _subject.Notify(data);
            }
        }
    }

    public class ObserverTests
    {
        public ObserverTests(ITestOutputHelper outputHelper) => Trace.Listeners.Add(new TestTraceListener(outputHelper));

        [Fact]
        public void Test()
        {
            var dbObserver = new DbObserver();
            var apiObserver = new ApiObserver();

            var monitor = new SensorMonitor();
            monitor.Start(dbObserver, apiObserver);
        }
    }

    // C# has also interfaces IObserver<T> and IObservable<T>
    // https://docs.microsoft.com/en-us/dotnet/standard/events/observer-design-pattern

    // Recommend to use rx.net https://github.com/dotnet/reactive

    internal class DataMonitor : IObserver<int>
    {
        public void OnCompleted()
        {
            // Do some cleanup etc. when data is handled
        }

        public void OnError(Exception error)
        {
            // Handle error
        }

        public void OnNext(int value)
        {
            Trace.WriteLine($"Received new data: {value}");
        }
    }

    public class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> _observers;
        private IObserver<T> _observer;

        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    internal class DataProvider : IObservable<int>
    {
        private readonly List<IObserver<int>> _observers = new List<IObserver<int>>();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber<int>(_observers, observer);
        }

        public void Next(int data) => _observers.ForEach(o => o.OnNext(data));
    }

    public class SystemObserverTests
    {
        public SystemObserverTests(ITestOutputHelper outputHelper) => Trace.Listeners.Add(new TestTraceListener(outputHelper));

        [Fact]
        public void Test()
        {
            var monitor = new DataMonitor();
            var provider = new DataProvider();

            var unsibscirber = provider.Subscribe(monitor);

            provider.Next(2);
            provider.Next(4);

            unsibscirber.Dispose();

            provider.Next(10);
        }
    }
}