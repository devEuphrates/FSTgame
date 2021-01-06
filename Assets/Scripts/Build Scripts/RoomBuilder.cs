using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class RoomBuilder : MonoBehaviour
{
    public static RoomBuilder Instance;

    [Header("References")]
    public Transform roomsParent;
    public GameObject emptyReference;
    public GameObject segmentReference;

    private GameObject CreatedRoom = null;
    [Header("Attirbutes")]
    [Space]
    [Range(0.01f, 1f)]
    public float wallThickness = 0.1f;
    public float wallHeight = 2f;

    private List<Room> generatedRooms = new List<Room>();

    public int initId = 0;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        initId = 0;
    }

    public void CreateRoom(Vector3[] points, float height = 2f, float thickness = 0.1f)
    {
        if (points.Length < 3) return;

        if (CreatedRoom != null)
        {
            Destroy(CreatedRoom);
            CreatedRoom = null;
        }

        Vector3 vec1_2 = (points[1] - points[0]).normalized;

        int selectedIndex = -1;
        for (int i = 2; i < points.Length; i++)
        {
            float dt = Vector3.Dot(vec1_2, (points[i] - points[0]).normalized);
            if (dt == 1) continue;
            else if (dt == -1) break;
            else
            {
                selectedIndex = i;
                break;
            }
        }

        if (selectedIndex == -1) return;

        Vector3 vec1_n = (points[selectedIndex] - points[0]).normalized;
        bool isInverted = Vector3.Dot(transform.up, Vector3.Cross(vec1_2, vec1_n)) < 0;
        if (isInverted) thickness *= -1;

        GameObject newRoom = Instantiate(emptyReference, Vector3.zero, Quaternion.identity, roomsParent);
        newRoom.name = "ROOM - " + initId++;

        List<Vector3> verts_in_down = new List<Vector3>();
        List<Vector3> verts_in_up = new List<Vector3>();
        List<Vector3> verts_out_down = new List<Vector3>();
        List<Vector3> verts_out_up = new List<Vector3>();
        List<Vector3> verts_lid_down = new List<Vector3>();
        List<Vector3> verts_lid_up = new List<Vector3>();

        List<WallSegment> inWalls = new List<WallSegment>();
        List<WallSegment> outWalls = new List<WallSegment>();
        List<WallSegment> lidWalls = new List<WallSegment>();

        Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
        poly.outside = new List<Vector3>();

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 vtn;
            Vector3 vtp;
            Vector3 mid;

            int nin = (i == points.Length - 1) ? 0 : i + 1;
            int pin = (i == 0) ? points.Length - 1 : i - 1;

            vtn = (points[nin] - points[i]).normalized;
            vtp = (points[pin] - points[i]).normalized;

            mid = (vtn + vtp).normalized;
            if (mid.magnitude == 0) mid = Vector3.Cross(vtn, transform.up).normalized;
            float angle = Vector3.Angle(vtp, mid);
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            sin = (sin == 0) ? 1 : sin;
            float ntck = thickness / sin;
            ntck = (ntck <= 0) ? thickness : ntck;



            Vector3 vx = (points[nin] - points[pin]).normalized;
            Vector3 vs = (points[i] - points[pin]).normalized;


            Vector3 perp = Vector3.Cross(vx, vs);
            float dir = Vector3.Dot(perp, transform.up);
            bool concave = (dir >= 0f);

            mid = concave ? -1 * mid : mid;

            Vector3 midHighPoint = points[i] + mid * ntck;
            Vector3 outHighPoint = points[i];
            midHighPoint.y += height;
            outHighPoint.y += height;

            Debug.DrawLine(points[i] + mid * ntck, midHighPoint, Color.black);

            verts_in_down.Add(points[i] + mid * ntck);
            verts_in_up.Add(midHighPoint);
            verts_out_down.Add(points[i]);
            verts_out_up.Add(outHighPoint);
            verts_lid_down.Add(outHighPoint);
            verts_lid_up.Add(midHighPoint);
        }

        WallSegment wlBottom = new WallSegment();
        for (int i = 0; i < points.Length; i++)
        {
            int nIndex = (i == verts_in_down.Count - 1) ? 0 : i + 1;

            WallSegment wlIn = new WallSegment();
            wlIn.verts.Add(verts_in_down[i]);
            wlIn.verts.Add(verts_in_up[i]);
            wlIn.verts.Add(verts_in_up[nIndex]);
            wlIn.verts.Add(verts_in_down[nIndex]);

            WallSegment wlOut = new WallSegment();
            wlOut.verts.Add(verts_out_down[i]);
            wlOut.verts.Add(verts_out_up[i]);
            wlOut.verts.Add(verts_out_up[nIndex]);
            wlOut.verts.Add(verts_out_down[nIndex]);

            WallSegment wlLid = new WallSegment();
            wlLid.verts.Add(verts_lid_down[i]);
            wlLid.verts.Add(verts_lid_up[i]);
            wlLid.verts.Add(verts_lid_up[nIndex]);
            wlLid.verts.Add(verts_lid_down[nIndex]);
            
            wlBottom.verts.Add(points[i]);

            inWalls.Add(wlIn);
            outWalls.Add(wlOut);
            lidWalls.Add(wlLid);
        }

        List<Wall> walls = new List<Wall>();
 
        for (int i = 0; i < points.Length; i++)
        {
            Poly2Mesh.Polygon polOut = new Poly2Mesh.Polygon();
            Poly2Mesh.Polygon polIn = new Poly2Mesh.Polygon();
            Poly2Mesh.Polygon polLid = new Poly2Mesh.Polygon();

            polOut.outside = outWalls[i].verts;
            polIn.outside = inWalls[i].verts;
            polLid.outside = lidWalls[i].verts;

            Vector3 wallPos = inWalls[i].verts[0] + (Vector3.Distance(inWalls[i].verts[0], inWalls[i].verts[3]) * 0.5f) * (outWalls[i].verts[3] - inWalls[i].verts[0]).normalized;
            Vector3 normalizedCenterPoint = Quaternion.Euler(0f, 90f, 0f) * (inWalls[i].verts[3] - inWalls[i].verts[0]).normalized;
            if (isInverted) normalizedCenterPoint *= -1;
            Vector3 cent = normalizedCenterPoint + wallPos;
            GameObject totalWall = Instantiate(emptyReference, wallPos, Quaternion.identity, newRoom.transform);
            totalWall.name = "WALL - " + i.ToString();
            totalWall.transform.LookAt(cent);

            polIn.CalcPlaneNormal(normalizedCenterPoint);
            inWalls[i].normal = normalizedCenterPoint;
            polOut.CalcPlaneNormal(-1 * normalizedCenterPoint);
            outWalls[i].normal = -1 * normalizedCenterPoint;
            polLid.CalcPlaneNormal(transform.up);
            lidWalls[i].normal = transform.up;

            GameObject inOBJ = Instantiate(segmentReference, totalWall.transform);
            inWalls[i].objectInstance = inOBJ;
            inOBJ.GetComponent<MeshFilter>().mesh = Poly2Mesh.CreateMesh(polIn);
            inOBJ.transform.position = inOBJ.transform.position - totalWall.transform.position;
            inOBJ.transform.eulerAngles = inOBJ.transform.eulerAngles - totalWall.transform.eulerAngles;
            inOBJ.GetComponent<MeshCollider>().sharedMesh = inOBJ.GetComponent<MeshFilter>().mesh;
            inOBJ.name = "INSIDE";

            GameObject outOBJ = Instantiate(segmentReference, totalWall.transform);
            outWalls[i].objectInstance = outOBJ;
            outOBJ.GetComponent<MeshFilter>().mesh = Poly2Mesh.CreateMesh(polOut);
            outOBJ.transform.position = outOBJ.transform.position - totalWall.transform.position;
            outOBJ.transform.eulerAngles = outOBJ.transform.eulerAngles - totalWall.transform.eulerAngles;
            outOBJ.GetComponent<MeshCollider>().sharedMesh = outOBJ.GetComponent<MeshFilter>().mesh;
            outOBJ.name = "OUTSIDE";

            GameObject lidOBJ = Instantiate(segmentReference, totalWall.transform);
            lidWalls[i].objectInstance = lidOBJ;
            lidOBJ.GetComponent<MeshFilter>().mesh = Poly2Mesh.CreateMesh(polLid);
            lidOBJ.transform.position = lidOBJ.transform.position - totalWall.transform.position;
            lidOBJ.transform.eulerAngles = lidOBJ.transform.eulerAngles - totalWall.transform.eulerAngles;
            lidOBJ.GetComponent<MeshCollider>().sharedMesh = lidOBJ.GetComponent<MeshFilter>().mesh;
            lidOBJ.name = "TOP";

            Wall wall = new Wall(new List<WallSegment> {outWalls[i], inWalls[i], lidWalls[i]});
            walls.Add(wall);
        }

        GameObject floorOBJ = Instantiate(segmentReference, newRoom.transform);
        wlBottom.objectInstance = floorOBJ;
        Poly2Mesh.Polygon floorPoly = new Poly2Mesh.Polygon();
        floorPoly.outside = wlBottom.verts;
        floorPoly.CalcPlaneNormal(new Vector3(0f, 1f, 0f));
        floorOBJ.GetComponent<MeshFilter>().mesh = Poly2Mesh.CreateMesh(floorPoly);
        floorOBJ.name = "ROOM_FLOOR";

        Room rm = new Room(walls, wlBottom);
        generatedRooms.Add(rm);
    }

    public (WallSegment segment, WallSegment friend) GetWallSegmentAndFriendFromInsID(int instanceID)
    {
        WallSegment found = null;
        WallSegment frnd = null;

        foreach (Room room in generatedRooms)
        {
            foreach (Wall wall in room.walls)
            {
                for (int i = 0; i < wall.segments.Count; i++)
                {
                    if (wall.segments[i].objectInstance.GetInstanceID() == instanceID)
                    {
                        found = wall.segments[i];
                        int friendIndx = (i == 0) ? 1 : 0;
                        frnd = wall.segments[friendIndx];
                        break;
                    } else continue;
                }
                if (found != null) break;
            }
            if (found != null) break;
        }

        return (segment:found, friend: frnd);
    }

    public void CutWall(int roomIndx, int wallIndx, int[] segments)
    {
        if (roomIndx >= generatedRooms.Count || wallIndx >= generatedRooms[roomIndx].walls.Count) return;
    }
}

public class Room
{
    public List<Wall> walls;
    public WallSegment floor;

    public Room(List<Wall> wallsList, WallSegment floorSegment)
    {
        walls = wallsList;
    }

    public Room(Wall[] wallsArray, WallSegment floorSegment)
    {
        walls = new List<Wall>();
        for (int i = 0; i < wallsArray.Length; i++)
        {
            walls.Add(wallsArray[i]);
        }

        floor = floorSegment;
    }

    public void UpdateSegmentsMesh()
    {
        foreach (Wall wall in walls)
        {
            foreach (WallSegment segment in wall.segments)
            {
                segment.UpdateMesh();
            }
        }
    }
}

public class Wall
{
    public List<WallSegment> segments;

    public Wall(List<WallSegment> segmentsList)
    {
        segments = segmentsList;
    }

    public Wall(WallSegment[] segmentsArray)
    {
        segments = new List<WallSegment>();
        for (int i = 0; i < segmentsArray.Length; i++)
        {
            segments.Add(segmentsArray[i]);
        }
    }
}

public class WallSegment
{
    public List<Vector3> verts;
    public List<List<Vector3>> holes;
    public GameObject objectInstance;
    public Vector3 normal;

    public WallSegment()
    {
        verts = new List<Vector3>();
        holes = new List<List<Vector3>>();
        objectInstance = null;
        normal = new Vector3(0f, 0f, 0f);
    }

    public void AddHole(List<Vector3> hole)
    {
        holes.Add(hole);
    }

    public void RemoveHole(int holeIndex)
    {
        holes.RemoveAt(holeIndex);
    }

    public void UpdateMesh()
    {
        Poly2Mesh.Polygon newPoly = new Poly2Mesh.Polygon();
        newPoly.outside = verts;
        newPoly.holes = holes;
        newPoly.CalcPlaneNormal(normal);
        Mesh createdmesh = Poly2Mesh.CreateMesh(newPoly);
        objectInstance.GetComponent<MeshFilter>().mesh = createdmesh;
        objectInstance.GetComponent<MeshCollider>().sharedMesh = createdmesh;
    }
}
