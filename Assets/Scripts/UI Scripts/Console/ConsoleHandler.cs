using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleHandler : MonoBehaviour
{
    private InputsManager Imanager;

    public static ConsoleHandler Instance;

    [Header("Prefabs")]
    public GameObject logPrefab;
    private List<GameObject> logs = new List<GameObject>();

    [Header("References")]
    [SerializeField]public List<CommandObject> commands = new List<CommandObject>();
    public InputField inputF;
    public GameObject console;
    public GameObject logHolder;
    private bool consoleState = false;

    private void Awake()
    {
        if (Instance != null) Destroy(this); else Instance = this;

        consoleState = false;
        Imanager = new InputsManager();
        Imanager.UI.DeveloperConsole.performed += _ => ToggleDevConsole();
        Imanager.UI.Enter.performed += _ => Entered();

        logs = new List<GameObject>();
    }

    private void ToggleDevConsole()
    {
        consoleState = !consoleState;
        console.SetActive(consoleState);
        if (consoleState)
        {
            PlayerController.Instance.DisableControls();
            PlayerInteractions.Instance.DisableInteractions();
        }
        else
        {
            PlayerController.Instance.EnableControls();
            PlayerInteractions.Instance.EnableInteractions();
        }
    }

    private void OnEnable()
    {
        Imanager.Enable();
    }

    private void OnDisable()
    {
        Imanager.Disable();
    }

    public void Entered()
    {
        if (!consoleState) return;

        string[] ins = inputF.text.ToLower().Split('.');
        ICommand cmd = commands.FindLast(p => p.CommandName.ToLower() == ins[0]);
        string[] args = ins.Skip(1).ToArray();
        if (cmd != null) cmd.ExecuteCommand(args);
    }

    public void Log(string text)
    {
        Log(text, Color.white);
    }

    public void Log(string text, Color color)
    {
        GameObject newLog = Instantiate(logPrefab, logHolder.transform);
        newLog.GetComponentInChildren<Text>().text = text;
        newLog.GetComponentInChildren<Text>().color = color;
        newLog.transform.SetSiblingIndex(0);
    }
}
