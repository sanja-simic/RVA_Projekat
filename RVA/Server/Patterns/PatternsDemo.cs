using System;
using System.Collections.Generic;
using TravelSystem.Server.Patterns.Command;
using TravelSystem.Server.Patterns.Factory;
using TravelSystem.Server.Patterns.Observer;
using TravelSystem.Server.Patterns.State;
using TravelSystem.Server.Patterns.Adapter;
using TravelSystem.Server.Patterns.Repository;

namespace TravelSystem.Server.Patterns
{
    /// <summary>
    /// Demonstration class showing how to use all 6 design patterns
    /// </summary>
    public class PatternsDemo
    {
        /// <summary>
        /// Demonstrate all design patterns
        /// </summary>
        public static void RunDemo()
        {
            Console.WriteLine("=== Design Patterns Demonstration ===\n");

            DemonstrateCommandPattern();
            DemonstrateFactoryPattern();
            DemonstrateObserverPattern();
            DemonstrateStatePattern();
            DemonstrateAdapterPattern();
            DemonstrateRepositoryPattern();
        }

        private static void DemonstrateCommandPattern()
        {
            Console.WriteLine("1. Command Pattern Demo:");
            
            var commandManager = new CommandManager();
            
            // Kreiraj test komandu
            var testCommand = new TestCommand("Test Command");
            
            // Izvršavaj komande
            commandManager.ExecuteCommand(testCommand);
            Console.WriteLine($"Can Undo: {commandManager.CanUndo}");
            
            if (commandManager.CanUndo)
            {
                commandManager.UndoCommand();
                Console.WriteLine("Command undone");
            }
            
            Console.WriteLine();
        }

        private static void DemonstrateFactoryPattern()
        {
            Console.WriteLine("2. Factory Pattern Demo:");
            
            try
            {
                // Kreiraj repository preko factory-ja
                var csvRepo = TravelRepositoryFactory.CreateRepository("CSV");
                var jsonRepo = TravelRepositoryFactory.CreateRepository("JSON");
                var xmlRepo = TravelRepositoryFactory.CreateRepository("XML");
                
                Console.WriteLine("Successfully created repositories via Factory");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Factory demo: {ex.Message}");
            }
            
            Console.WriteLine();
        }

        private static void DemonstrateObserverPattern()
        {
            Console.WriteLine("3. Observer Pattern Demo:");
            
            var subject = new TestSubject();
            var observer1 = new TestObserver("Observer1");
            var observer2 = new TestObserver("Observer2");
            
            subject.Subscribe(observer1);
            subject.Subscribe(observer2);
            
            subject.NotifyObservers("Test notification");
            
            Console.WriteLine();
        }

        private static void DemonstrateStatePattern()
        {
            Console.WriteLine("4. State Pattern Demo:");
            
            var context = new TestStateContext();
            var state1 = new TestState("State1");
            var state2 = new TestState("State2");
            
            StateManager.TransitionTo(context, state1);
            StateManager.HandleCurrentState(context);
            
            StateManager.TransitionTo(context, state2);
            StateManager.HandleCurrentState(context);
            
            Console.WriteLine();
        }

        private static void DemonstrateAdapterPattern()
        {
            Console.WriteLine("5. Adapter Pattern Demo:");
            
            var adapter = new TestAdapter();
            AdapterRegistry.RegisterAdapter<string, int>(adapter);
            
            if (AdapterRegistry.HasAdapter<string, int>())
            {
                var registeredAdapter = AdapterRegistry.GetAdapter<string, int>();
                var result = registeredAdapter.Adapt("123");
                Console.WriteLine($"Adapted '123' to {result}");
            }
            
            Console.WriteLine();
        }

        private static void DemonstrateRepositoryPattern()
        {
            Console.WriteLine("6. Repository Pattern Demo:");
            
            var repository = new TestRepository();
            
            // Dodaj test entitete
            repository.Add("Entity1");
            repository.Add("Entity2");
            
            Console.WriteLine($"Repository contains {repository.Count()} entities");
            
            var allEntities = repository.GetAll();
            foreach (var entity in allEntities)
            {
                Console.WriteLine($"- {entity}");
            }
            
            Console.WriteLine();
        }
    }

    #region Test Classes

    public class TestCommand : CommandBase
    {
        public TestCommand(string description) : base(description) { }

        public override void Execute()
        {
            Console.WriteLine($"Executing: {Description}");
            MarkAsExecuted();
        }

        public override void Undo()
        {
            Console.WriteLine($"Undoing: {Description}");
        }
    }

    public class TestSubject
    {
        private readonly List<ITestObserver> _observers = new List<ITestObserver>();

        public void Subscribe(ITestObserver observer)
        {
            _observers.Add(observer);
        }

        public void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }

    public interface ITestObserver
    {
        void Update(string message);
    }

    public class TestObserver : ITestObserver
    {
        private readonly string _name;

        public TestObserver(string name)
        {
            _name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"{_name} received: {message}");
        }
    }

    public class TestState : TravelSystem.Server.Patterns.State.State
    {
        private readonly string _name;

        public TestState(string name)
        {
            _name = name;
        }

        public override void Handle(IStateContext context)
        {
            Console.WriteLine($"Handling in {_name}");
        }

        public override string GetStateName()
        {
            return _name;
        }
    }

    public class TestStateContext : IStateContext
    {
        private TravelSystem.Server.Patterns.State.State _currentState;

        public void ChangeState(TravelSystem.Server.Patterns.State.State state)
        {
            _currentState = state;
        }

        public TravelSystem.Server.Patterns.State.State GetCurrentState()
        {
            return _currentState;
        }
    }

    public class TestAdapter : AdapterBase<string, int>
    {
        protected override int DoAdapt(string source)
        {
            return int.Parse(source);
        }

        protected override string DoAdaptBack(int target)
        {
            return target.ToString();
        }
    }

    public class TestRepository : GenericRepositoryBase<string>
    {
        protected override object GetEntityId(string entity)
        {
            return entity;
        }
    }

    #endregion
}