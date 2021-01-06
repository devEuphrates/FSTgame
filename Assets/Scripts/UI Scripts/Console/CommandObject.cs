using UnityEngine;

public abstract class CommandObject : ScriptableObject, ICommand
{
    [SerializeField] private string commandName = "";
    public string CommandName => commandName;
    public abstract bool ExecuteCommand(string[] args);
}
