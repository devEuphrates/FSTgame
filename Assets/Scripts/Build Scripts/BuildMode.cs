using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [Space]
    [Header("Layer Mask")]
    public LayerMask checkLayers;

    // Room Generator
    [Space]
    [Header("Prefabs")]
    public GameObject placeholderPrefab;
    public GameObject cornerPrefab;

    private bool buildingRoom = false;
    private bool placedFirst = false;
    private List<Vector3> currentPoints;
    private RoomBuilder rb;
    private List<GameObject> generatedPlaceholders;
    private List<GameObject> corners;
    private bool unPlaceable = false;
    private bool connected = false;
    private bool interiorWall = false;

    private GameObject selectedWall;

#pragma warning disable IDE0051 // Remove unused private members
    private void Awake()
    {
        if (Instance != null) Destroy(this); else Instance = this;
        buildingRoom = false;
        placedFirst = false;
        unPlaceable = false;
        interiorWall = false;
        connected = false;
        // placingFurniture = false; 
        currentPoints = new List<Vector3>();
        generatedPlaceholders = new List<GameObject>();
        corners = new List<GameObject>();
    }

    private void Start()
#pragma warning restore IDE0051 // Remove unused private members
    {
        pis = PlayerInteractions.Instance;
        rb = RoomBuilder.Instance;
        gh = GridHandler.Instance;

        pis.onPlayerInteract += Pis_onPlayerInteract;
    }

    private void Pis_onPlayerInteract(object sender, PlayerInteractInfo e)
    {
        if (!buildingRoom) return;

        GameObject newPH;
        GameObject newCR;

        switch (e.typeInfo)
        {
            case InteractionType.Grid:

                Vector3 wantedPoint = (gh.gridStatus) ? gh.GetClosestGridPoint(e.hitInfo.point) : gh.PointOnGridHeight(e.hitInfo.point);
                if (unPlaceable) return;
                if (generatedPlaceholders.Count > 0) SetLastPH(wantedPoint);
                if (placedFirst)
                {
                    generatedPlaceholders[generatedPlaceholders.Count - 1].GetComponentInChildren<BoxCollider>().enabled = true;
                    Destroy(generatedPlaceholders[generatedPlaceholders.Count - 1].transform.GetChild(1).gameObject);
                }
                else
                {
                    newCR = Instantiate(cornerPrefab, transform);
                    corners.Add(newCR);
                    newCR.transform.position = wantedPoint;
                    newCR.name = "CORNER." + (corners.Count - 1).ToString();
                }

                placedFirst = true;

                currentPoints.Add(wantedPoint);
                newPH = Instantiate(placeholderPrefab, transform);
                generatedPlaceholders.Add(newPH);
                newPH.name = (generatedPlaceholders.Count - 1).ToString();
                break;
            case InteractionType.Wall:
                if (placedFirst)
                {
                    if (e.hitInfo.transform.gameObject.name == "INSIDE" && interiorWall && !unPlaceable)
                    {
                        
                    }
                    else if (e.hitInfo.transform.gameObject.name == "OUTSIDE" && connected && !unPlaceable)
                    {
                        Vector3 pt = gh.PointOnGridHeight(e.hitInfo.point);
                        pt += -e.hitInfo.normal * (rb.wallThickness * 0.5f);
                        currentPoints.Add(pt);
                        List<Vector3> extras = currentPoints.Skip(1).Take(currentPoints.Count - 2).ToList();

                        Room rm = rb.GetRoom(int.Parse(e.hitInfo.transform.parent.parent.name.Split('.')[1]));
                        bool isRev = !rb.TorqueDown(currentPoints);
                        rm.ExpandRoom(int.Parse(selectedWall.transform.parent.name.Split('.')[1]), int.Parse(e.hitInfo.transform.parent.name.Split('.')[1]), currentPoints[0], currentPoints[currentPoints.Count - 1], extras, isRev);

                        ClearRoomBuilderTemps();
                        ToggleRoomBuilder(false);
                        unPlaceable = false;
                    }
                }
                else
                {
                    Vector3 pt = gh.PointOnGridHeight(e.hitInfo.point);
                    pt += -e.hitInfo.normal * (rb.wallThickness * 0.5f);

                    currentPoints.Add(pt);
                    newPH = Instantiate(placeholderPrefab, new Vector3(0f, -10000f, 0f), Quaternion.identity, transform);
                    generatedPlaceholders.Add(newPH);
                    newPH.name = (generatedPlaceholders.Count - 1).ToString();
                    newPH.GetComponentInChildren<WallDistanceCheck>().connectedWallID = int.Parse(e.hitInfo.transform.parent.gameObject.name.Split('.')[1]);
                    selectedWall = e.hitInfo.transform.gameObject;
                    placedFirst = true;

                    if (e.hitInfo.transform.gameObject.name == "INSIDE") interiorWall = true;
                    else if (e.hitInfo.transform.gameObject.name == "OUTSIDE") connected = true;
                }
                
                break;
            case InteractionType.Placeholder:
                break;
            case InteractionType.PhCorner:

                if (currentPoints.Count < 2) return;
                if (e.hitInfo.transform.gameObject != corners[0]) return;
                float angle = Vector3.Angle((currentPoints[currentPoints.Count - 1] - currentPoints[0]).normalized, (currentPoints[1] - currentPoints[0]).normalized);
                if (angle < 22.5f) return;
                rb.CreateRoom(currentPoints.ToArray());
                ClearRoomBuilderTemps();
                ToggleRoomBuilder(false);
                unPlaceable = false;
                break;
            default:
                break;
        }
    }

    private void ClearRoomBuilderTemps()
    {
        currentPoints.Clear();
        generatedPlaceholders.ForEach(p => Destroy(p));
        generatedPlaceholders.Clear();
        corners.ForEach(p => Destroy(p));
        corners.Clear();
        placedFirst = false;
        interiorWall = false;
        connected = false;
        selectedWall = null;
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

#pragma warning disable IDE0051 // Remove unused private members
    private void Update()
#pragma warning restore IDE0051 // Remove unused private members
    {
        if (buildingRoom)
        {
            if (placedFirst)
            {
                Ray castedRay = pis.cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(castedRay, out RaycastHit hit, Mathf.Infinity, pis.selectionLayerMask) && !EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject last = generatedPlaceholders[generatedPlaceholders.Count - 1];
                    if (hit.transform.gameObject.layer == 7)
                    {
                        Vector3 wantedPoint = (gh.gridStatus) ? gh.GetClosestGridPoint(hit.point) : gh.PointOnGridHeight(hit.point);
                        
                        if (currentPoints.Count > 1)
                            unPlaceable = !CheckPointPlaceable(wantedPoint, currentPoints[currentPoints.Count - 1], currentPoints[currentPoints.Count - 2]);
                        else
                            unPlaceable = !CheckPointPlaceable(wantedPoint, currentPoints[currentPoints.Count - 1]);

                        if (unPlaceable && last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_Second") != Color.red)
                            last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_Second", Color.red);
                        else if (!unPlaceable && last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_Second") != Color.gray)
                            last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_Second", Color.gray);

                        SetLastPH(wantedPoint);
                    }
                    else if (hit.transform.gameObject.layer == 8)
                    {
                        _ = new Vector3();
                        Vector3 wantedPoint;

                        if (hit.transform.gameObject.name.Split('.')[0] == "CORNER")
                        {
                            wantedPoint = gh.PointOnGridHeight(hit.transform.position);
                        }
                        else
                        {
                            wantedPoint = gh.PointOnGridHeight(hit.point);
                            wantedPoint += -hit.normal * (rb.wallThickness * 0.5f);

                            if (currentPoints.Count > 1)
                                unPlaceable = !CheckPointPlaceableOnWall(wantedPoint, currentPoints[currentPoints.Count - 1], currentPoints[currentPoints.Count - 2], hit.transform.gameObject);
                            else
                                unPlaceable = true;

                            if (unPlaceable && last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_Second") != Color.red)
                                last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_Second", Color.red);
                            else if (!unPlaceable && last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_Second") != Color.gray)
                                last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_Second", Color.gray);
                        }
                        SetLastPH(wantedPoint);
                    }
                    else if (hit.transform.gameObject.layer == 9 && hit.transform.gameObject.name.Split('.')[0] == "CORNER")
                    {
                        Vector3 wantedPoint = hit.transform.position;

                        if (hit.transform.gameObject == corners[0])
                        {
                            if (last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_Second") != Color.gray)
                                last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_Second", Color.gray);
                        }
                        else
                        {
                            if (last.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_Second") != Color.red)
                                last.GetComponentInChildren<MeshRenderer>().material.SetColor("Color_Second", Color.red);
                        }
                        
                        SetLastPH(wantedPoint);
                    }
                    else if (hit.transform.gameObject.layer == 10 && interiorWall)
                    {
                        Vector3 wantedPoint = hit.point;

                        SetLastPH(wantedPoint);
                    }
                }
            }
        }
    }

    private bool CheckPointPlaceable(Vector3 point1, Vector3 point2)
    {
        float dis = Vector3.Distance(point1, point2);
        // Check if placeholder is colliding with another struct.
        if (currentPoints.Count <= 1 && selectedWall != null) selectedWall.GetComponent<Collider>().enabled = false;
        Ray checkRay = new Ray(point1, (point2 - point1));
        bool canDo = !Physics.Raycast(checkRay, out RaycastHit checkHit, dis - 0.1f, checkLayers);
        if (currentPoints.Count <= 1 && selectedWall != null) selectedWall.GetComponent<Collider>().enabled = true;

        // Distance check
        if (dis < 1f) return false;

        // Check to see if wanted point is too close to another struct.
        List<Collider> overlapList = Physics.OverlapSphere(new Vector3(point1.x, point1.y + 1f, point1.z), 0.9f, checkLayers).ToList();
        overlapList.RemoveAll(p => p.transform.parent.gameObject.name == (generatedPlaceholders.Count - 2).ToString() || p.gameObject.name.StartsWith("CORNER"));
        if (overlapList.Count > 0) return false;

        // Check if placeholder is too close to any structs.
        if (generatedPlaceholders[generatedPlaceholders.Count - 1].GetComponentInChildren<WallDistanceCheck>().isColliding) return false;
        
        return canDo;
    }

    private bool CheckPointPlaceable(Vector3 point1, Vector3 point2, Vector3 point3)
    {
        // Angle between placeholders check.
        float angle = Vector3.Angle((point1 - point2).normalized, (point3 - point2).normalized);
        if (angle < 22.5f) return false;

        // Check for other problems.
        return CheckPointPlaceable(point1, point2);
    }

    private bool CheckPointPlaceableOnWall(Vector3 point1, Vector3 point2, Vector3 point3, GameObject hitOBJ)
    {
        // Angle between placeholders check.
        float angle = Vector3.Angle((point1 - point2).normalized, (point3 - point2).normalized);
        if (angle < 22.5f) return false;

        // Distance check.
        float dis = Vector3.Distance(point1, point2);
        if (dis < 1f) return false;

        // Check if building same thing.
        if ((connected && hitOBJ.name == "INSIDE") || (interiorWall && hitOBJ.name == "OUTSIDE")) return false;

        // Check if placeholder is too close to any structs.
        generatedPlaceholders[generatedPlaceholders.Count - 1].GetComponentInChildren<WallDistanceCheck>().connectedWallID = int.Parse(hitOBJ.transform.parent.gameObject.name.Split('.')[1]);
        if (generatedPlaceholders[generatedPlaceholders.Count - 1].GetComponentInChildren<WallDistanceCheck>().isColliding) return false;

        // Check if placeholder is colliding with another struct.
        hitOBJ.GetComponent<MeshCollider>().enabled = false;
        Ray checkRay = new Ray(point1, (point2 - point1));
        bool canDo = !Physics.Raycast(checkRay, out _ , dis - 0.1f, checkLayers);
        hitOBJ.GetComponent<MeshCollider>().enabled = true;
        return canDo;
    }

    private bool CheckPointPlaceableOnCorner()
    {
        return true;
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
