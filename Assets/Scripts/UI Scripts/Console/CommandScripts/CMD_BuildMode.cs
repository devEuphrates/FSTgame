using UnityEngine;

[CreateAssetMenu(fileName = "New Build Mode Command", menuName = "Developer Console/Commands/Build Mode Command")]
public class CMD_BuildMode : CommandObject
{
    public override bool ExecuteCommand(string[] args)
    {
        if (args != null && (args.Length == 1 || args.Length == 2))
        {
            if (args[0] == "room")
            {
                if (args.Length == 1)
                {
                    BuildMode.Instance.ToggleRoomBuilder();
                    string txt = "Room Build mode toggled ";
                    txt += (BuildMode.Instance.GetRoomBuilderState()) ? "on." : "off.";
                    ConsoleHandler.Instance.Log(txt, Color.yellow);
                    return true;
                }
                else if (args.Length == 2)
                {
                    if (args[1] == "on")
                    {
                        BuildMode.Instance.ToggleRoomBuilder(true);
                        ConsoleHandler.Instance.Log("Room Build mode activated", Color.yellow);
                        return true;
                    }
                    else if (args[1] == "off")
                    {
                        BuildMode.Instance.ToggleRoomBuilder(false);
                        ConsoleHandler.Instance.Log("Room Build mode deactivated", Color.yellow);
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else if (args[0] == "on")
            {
                return true;
            }
            else if (args[0] == "off")
            {
                return true;
            }
            else return false;
        }
        else return false;
    }
}
