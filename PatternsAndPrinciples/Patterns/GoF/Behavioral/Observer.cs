using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Behavioral
{
    /*
     * A pulish/subscribe pattern which allows a number of observer objects to see an event.
     *
     * Define a one-to-many dependency between objects so that when one object changes
     * state, all its dependents are notified and updated automatically.
    */

    #region "Observer"

    public interface IObserver
    {
        void Update(int data);
    }

    public class DbObserver : IObserver
    {
        public void Update(int data)
        {
            Trace.WriteLine($"Now I will send data to DB: {data}");
        }
    }

    public class ApiObserver : IObserver
    {
        public void Update(int data)
        {
            Trace.WriteLine($"Now I will send data to API: {data}");
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

    #endregion "Observer"

    public class ObserverTests
    {
        [Fact]
        public void Test()
        {
            var dbObserver = new DbObserver();
            var apiObserver = new ApiObserver();

            var monitor = new SensorMonitor();
            monitor.Start(dbObserver, apiObserver);
        }
    }

    #region "IObserver"

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

    #endregion "IObserver"

    public class SystemObserverTests
    {
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

    #region "Events"

    public class EventDataProvider
    {
        public event EventHandler<int> NewData;
        //public Action<int> NewData;

        public void Next(int data)
        {
            NewData?.Invoke(this, data);
            //NewData?.Invoke(data);
        }
    }

    public class EventDataObserver
    {
        private readonly EventDataProvider _provider;

        public EventDataObserver(EventDataProvider provider)
        {
            _provider = provider;
            _provider.NewData += (object sender, int e) =>
            {
                Trace.WriteLine($"Received new data: {e}");

            };
            //_provider.NewData += (int e) => Trace.WriteLine($"Received new data: {e}");
        }
    }

    public class EventTests
    {
        [Fact]
        public void Test()
        {
            var provider = new EventDataProvider();

            var observer = new EventDataObserver(provider);

            provider.Next(2);
            provider.Next(10);
        }
    }

    #endregion
}