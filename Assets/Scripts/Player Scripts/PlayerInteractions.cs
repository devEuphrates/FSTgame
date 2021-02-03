using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public static PlayerInteractions Instance;

    public bool disabled = false;
    public Camera cam;
    private InputsManager Imanager;

    // Events
    public event EventHandler<PlayerInteractInfo> onPlayerInteract;

    [Header("Layer Masks")]
    public LayerMask selectionLayerMask;

    private List<Vector3> addedPoints = new List<Vector3>();

    private void Awake()
    {
        if (Instance != null) Destroy(this); else Instance = this;
        Imanager = new InputsManager();
    }

    private void Start()
    {
        cam = transform.GetComponentInChildren<Camera>();
        Imanager.UI.Select.performed += _ => Clicked();
        Imanager.UI.Enter.performed += _ => Entered();
    }

    public void EnableInteractions()
    {
        Imanager.Enable();
    }

    public void DisableInteractions()
    {
        Imanager.Disable();
    }

    private void OnEnable()
    {
        Imanager.Enable();
    }

    private void OnDisable()
    {
        Imanager.Disable();
    }

    private void Clicked()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray castedRay = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(castedRay, out RaycastHit hit, Mathf.Infinity, selectionLayerMask))
        {
            PlayerInteractInfo inInfo = new PlayerInteractInfo(hit, InteractionType.Grid);

            switch (hit.transform.gameObject.layer)
            {
                case 7:
                    inInfo = new PlayerInteractInfo(hit, InteractionType.Grid);
                    break;
                case 8:
                    if (hit.transform.gameObject.name.StartsWith("CORNER"))
                        inInfo = new PlayerInteractInfo(hit, InteractionType.WallCorner);
                    else
                        inInfo = new PlayerInteractInfo(hit, InteractionType.Wall);
                    break;
                case 9:
                    if (hit.transform.gameObject.name.StartsWith("CORNER"))
                        inInfo = new PlayerInteractInfo(hit, InteractionType.PhCorner);
                    else
                        inInfo = new PlayerInteractInfo(hit, InteractionType.Placeholder);
                    break;
                case 10:
                    inInfo = new PlayerInteractInfo(hit, InteractionType.RoomFloor);
                    break;
                default:
                    break;
            }

            if(!EventSystem.current.IsPointerOverGameObject()) onPlayerInteract?.Invoke(this, inInfo);
        }
    }

    private void Entered()
    {
        RoomBuilder.Instance.CreateRoom(addedPoints.ToArray());
        addedPoints.Clear();
    }
}

public enum InteractionType { Grid, Wall, WallCorner, Placeholder, PhCorner, RoomFloor}

public class PlayerInteractInfo
{
    public RaycastHit hitInfo { get; set; }
    public InteractionType typeInfo { get; set; }

    public PlayerInteractInfo(RaycastHit hit, InteractionType type)
    {
        hitInfo = hit;
        typeInfo = type;
    }
}
