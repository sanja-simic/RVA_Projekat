namespace TravelSystem.Server.Patterns.Command
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        bool CanUndo { get; }
        string Description { get; }
    }
}
