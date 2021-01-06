using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public static PlayerInteractions Instance;

    public bool disabled = false;
    private Camera cam;
    private InputsManager Imanager;

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
            if (hit.transform.gameObject.layer == 7)
            {
                var segs = RoomBuilder.Instance.GetWallSegmentAndFriendFromInsID(hit.transform.gameObject.GetInstanceID());
                WallSegment seg = segs.segment;
                WallSegment friend = segs.friend;

                List<Vector3> holeIn = new List<Vector3>();
                List<Vector3> holeOut = new List<Vector3>();

                Vector3 p1 = new Vector3();
                p1 =  Quaternion.Euler(0f, 90f, 0f) * hit.normal + hit.point;
                p1.y = 1.5f;
                holeOut.Add(p1);

                Vector3 p2 = new Vector3();
                p2 = Quaternion.Euler(0f, -90f, 0f) * hit.normal + hit.point;
                p2.y = 1.5f;
                holeOut.Add(p2);

                Vector3 p3 = new Vector3();
                p3 = Quaternion.Euler(0f, -90f, 0f) * hit.normal + hit.point;
                p3.y = 0.5f;
                holeOut.Add(p3);

                Vector3 p4 = new Vector3();
                p4 = Quaternion.Euler(0f, 90f, 0f) * hit.normal + hit.point;
                p4.y = 0.5f;
                holeOut.Add(p4);

                seg.AddHole(holeOut);
                seg.UpdateMesh();

                friend.AddHole(holeOut);
                friend.UpdateMesh();
            }
            else if (hit.transform.gameObject.layer == 6)
            {
                Vector3 gridPos = GridHandler.Instance.GetClosestGridPoint(hit.point);
                gridPos.y = gridPos.y + 0.05f;
                addedPoints.Add(gridPos);
            }
        }
    }

    private void Entered()
    {
        RoomBuilder.Instance.CreateRoom(addedPoints.ToArray());
        addedPoints.Clear();
    }
}
