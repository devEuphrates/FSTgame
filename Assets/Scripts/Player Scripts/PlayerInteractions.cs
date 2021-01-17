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
    public LayerMask gridLayerMask;

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

        if (Physics.Raycast(castedRay, out RaycastHit hit, Mathf.Infinity, gridLayerMask))
        {
            PlayerInteractInfo inInfo = new PlayerInteractInfo(hit, InteractionType.GridSelect);
            if (hit.transform.gameObject.layer == 7) inInfo = new PlayerInteractInfo(hit, InteractionType.WallSelect);
            else if (hit.transform.gameObject.layer == 6) inInfo = new PlayerInteractInfo(hit, InteractionType.GridSelect);

            if(!EventSystem.current.IsPointerOverGameObject()) onPlayerInteract?.Invoke(this, inInfo);
        }
    }

    private void Entered()
    {
        RoomBuilder.Instance.CreateRoom(addedPoints.ToArray());
        addedPoints.Clear();
    }
}

public enum InteractionType { GridSelect, WallSelect}

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
