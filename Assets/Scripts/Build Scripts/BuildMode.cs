using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMode : MonoBehaviour
{
    public static BuildMode Instance;

    // References
    private PlayerInteractions pis;
    private GridHandler gh;

    // private bool placingFurniture = false;
    // Materials
    [Header("Materials")]
    public Material PlaceHolderMaterial;

    // Layer Masks
    [Header("Layer Mask")]
    public LayerMask checkLayers;

    // Room Generator
    public GameObject wallPH;
    private bool buildingRoom = false;
    private List<Vector3> currentPoints;
    private RoomBuilder rb;
    private List<GameObject> generatedPlaceholders;
    private bool unbuildable = false;

    private void Awake()
    {
        if (Instance != null) Destroy(this); else Instance = this;
        buildingRoom = false;
        unbuildable = false;
        // placingFurniture = false;
        currentPoints = new List<Vector3>();
        generatedPlaceholders = new List<GameObject>();
    }

    private void Start()
    {
        pis = PlayerInteractions.Instance;
        rb = RoomBuilder.Instance;
        gh = GridHandler.Instance;

        pis.onPlayerInteract += Pis_onPlayerInteract;
    }

    private void Pis_onPlayerInteract(object sender, PlayerInteractInfo e)
    {
        if (!buildingRoom) return;
        if (e.typeInfo == InteractionType.GridSelect)
        {
            Vector3 wantedPoint = (gh.gridStatus) ? gh.GetClosestGridPoint(e.hitInfo.point) : gh.PointOnGridHeight(e.hitInfo.point);
            if (currentPoints.Count >= 2 && Vector3.Distance(currentPoints[0], wantedPoint) < 1 && !unbuildable)
            {
                float angle = Vector3.Angle((currentPoints[currentPoints.Count - 1] - currentPoints[0]).normalized, (currentPoints[1] - currentPoints[0]).normalized);
                if (angle < 22.5f) return;
                rb.CreateRoom(currentPoints.ToArray());
                ClearRoomBuilderTemps();
                ToggleRoomBuilder(false);
            }
            else
            {
                if (unbuildable) return;
                if (generatedPlaceholders.Count > 0) SetLastPH(wantedPoint);
                if (generatedPlaceholders.Count != 0) generatedPlaceholders[generatedPlaceholders.Count - 1].GetComponentInChildren<BoxCollider>().enabled = true;
                
                currentPoints.Add(wantedPoint);
                GameObject newPH = Instantiate(wallPH, transform);
                generatedPlaceholders.Add(newPH);
            }
        }
    }

    private void ClearRoomBuilderTemps()
    {
        currentPoints.Clear();
        generatedPlaceholders.ForEach(p => Destroy(p));
        generatedPlaceholders.Clear();
    }

    public void ToggleRoomBuilder(bool state)
    {
        buildingRoom = state;
        if (!buildingRoom) ClearRoomBuilderTemps();
    }
    public void ToggleRoomBuilder()
    {
        ToggleRoomBuilder(!buildingRoom);
    }

    private void Update()
    {
        if (buildingRoom)
        {
            if (generatedPlaceholders.Count > 0)
            {
                Ray castedRay = pis.cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(castedRay, out RaycastHit hit, Mathf.Infinity, pis.gridLayerMask))
                {
                    Vector3 wantedPoint = (gh.gridStatus) ? gh.GetClosestGridPoint(hit.point) : gh.PointOnGridHeight(hit.point);
                    GameObject last = generatedPlaceholders[generatedPlaceholders.Count - 1];
                    if (currentPoints.Count > 1)
                        unbuildable = !CheckWallBuildable(wantedPoint, currentPoints[currentPoints.Count - 1], currentPoints[currentPoints.Count - 2]);
                    else
                        unbuildable = !CheckWallBuildable(wantedPoint, currentPoints[currentPoints.Count - 1]);

                    if (unbuildable && last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_01a6a682d2684805bc6d06ecbaf6fb7b") != Color.red)
                        last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_01a6a682d2684805bc6d06ecbaf6fb7b", Color.red);
                    else if (!unbuildable && last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_01a6a682d2684805bc6d06ecbaf6fb7b") != Color.gray)
                        last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_01a6a682d2684805bc6d06ecbaf6fb7b", Color.gray);

                    SetLastPH(wantedPoint);
                }
            }
        }
    }

    private bool CheckWallBuildable(Vector3 point1, Vector3 point2)
    {
        Ray checkRay = new Ray(point1, (point2 - point1));
        bool canDo = !Physics.Raycast(checkRay, out RaycastHit checkHit, Vector3.Distance(point1, point2), checkLayers);
        return canDo;
    }

    private bool CheckWallBuildable(Vector3 point1, Vector3 point2, Vector3 point3)
    {
        float angle = Vector3.Angle((point1 - point2).normalized, (point3 - point2).normalized);
        if (angle < 22.5f) return false;
        return CheckWallBuildable(point1, point2);
    }

    private void SetLastPH(Vector3 wantedPoint)
    {
        GameObject last = generatedPlaceholders[generatedPlaceholders.Count - 1];
        Vector3 p1 = currentPoints[currentPoints.Count - 1];
        last.transform.position = (p1 - wantedPoint).normalized * (Vector3.Distance(p1, wantedPoint) * 0.5f) + wantedPoint;
        last.transform.localScale = new Vector3(last.transform.localScale.x, last.transform.localScale.y, Vector3.Distance(p1, wantedPoint));
        last.transform.LookAt(p1);
    }

    public bool GetRoomBuilderState()
    {
        return buildingRoom;
    }
}
