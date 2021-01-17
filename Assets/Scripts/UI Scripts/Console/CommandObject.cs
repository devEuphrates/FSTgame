using System.Collections.Generic;
using UnityEngine;

public abstract class CommandObject : ScriptableObject, ICommand
{
    [SerializeField] private string commandName = "";
    [SerializeField] private List<string> aliases = new List<string>();
    public string CommandName => commandName;
    public List<string> Aliases => aliases;
    public abstract bool ExecuteCommand(string[] args);
}
