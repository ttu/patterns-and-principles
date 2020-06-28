using System;
using System.Collections.Generic;
using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Behavioral
{
    /*
     * Provides the ability to restore an object to its previous state (undo).
     *
     * Without violating encapsulation, capture and externalize an object's internal state
     * so that the object can be restored to this state later.
     */

    #region "Memento"

    public class Memento<T>
    {
        public Memento(T state) => State = state;

        public T State { get; private set; }
    }

    public class Originator<T>
    {
        public void SetMemento(Memento<T> memento) => State = memento.State;

        public T State { get; set; }

        public Memento<T> CreateMemento() => new Memento<T>(State);
    }

    public class CareTaker<T>
    {
        private readonly List<Memento<T>> _savedStates = new List<Memento<T>>();

        public void SaveState(Memento<T> memento) => _savedStates.Add(memento);

        public Memento<T> Get(int index) => _savedStates[index];
    }

    #endregion "Memento"

    #region "Undo/Redo"

    // C# has also a powerful way to create undo/redo pattern with delegates

    public class UndoAction
    {
        public Action Do { get; set; }
        public Action Undo { get; set; }
    }

    public class History<T>
    {
        public Stack<UndoAction> _actions = new Stack<UndoAction>();
        public Stack<UndoAction> _redo = new Stack<UndoAction>();

        public void Do(UndoAction action)
        {
            _redo.Clear();
            _actions.Push(action);
            action.Do();
        }

        public void Redo()
        {
            var redo = _redo.Pop();

            if (redo == null)
                return;

            _actions.Push(redo);
            redo.Do();
        }

        public void Undo()
        {
            var toUndo = _actions.Pop();

            if (toUndo == null)
                return;

            _redo.Push(toUndo);
            toUndo.Undo();
        }
    }

    #endregion "Undo/Redo"

    public class MementoTests
    {
        [Fact]
        public void Test()
        {
            var careTaker = new CareTaker<MyData>();
            var originator = new Originator<MyData>();

            originator.State = new MyData { Value = 2 };
            careTaker.SaveState(originator.CreateMemento());

            originator.State = new MyData { Value = 6 };
            careTaker.SaveState(originator.CreateMemento());
            Assert.Equal(6, originator.State.Value);

            originator.SetMemento(careTaker.Get(0));
            Assert.Equal(2, originator.State.Value);
        }

        [Fact]
        public void HistoryTest()
        {
            var history = new History<MyData>();

            MyData data = null;

            var ac = new UndoAction
            {
                Do = () => data = new MyData { Value = 2 },
                Undo = () => data = null,
            };

            history.Do(ac);
            Assert.Equal(2, data.Value);

            var ac2 = new UndoAction
            {
                Do = () => data = new MyData { Value = 6 },
                Undo = () => data = new MyData { Value = 2 },
            };

            history.Do(ac2);
            Assert.Equal(6, data.Value);

            history.Undo();
            Assert.Equal(2, data.Value);

            history.Redo();
            Assert.Equal(6, data.Value);
        }

        private class MyData
        {
            public int Value { get; set; }
        }
    }
}