public interface ICommand
{
    string CommandName { get; }
    bool ExecuteCommand(string[] args);
}
