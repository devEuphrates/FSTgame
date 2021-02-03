using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[DisallowMultipleComponent]
public class RoomBuilder : MonoBehaviour
{
    public static RoomBuilder Instance;

    [Header("References")]
    public Transform roomsParent;
    public GameObject emptyReference;
    public GameObject segmentReference;
    public GameObject cornerReference;
    public GameObject pointRef;

    private List<Vector3> pts = new List<Vector3>();

    private GameObject CreatedRoom = null;
    [Header("Attirbutes")]
    [Space]
    [Range(0.01f, 1f)]
    public float wallThickness = 0.1f;
    public float wallHeight = 2f;
    public Material floorMat;
    public Material outMat;

    private List<Room> generatedRooms = new List<Room>();

    private int initId = 0;

#pragma warning disable IDE0051 // Remove unused private members
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        initId = 0;
    }

    private void Update()
#pragma warning restore IDE0051 // Remove unused private members
    {
        for (int i = 0; i < pts.Count; i++)
        {
            int nid = (i == pts.Count - 1) ? 0 : i + 1;
            Debug.DrawLine(pts[i], pts[nid], Color.white);
        }
    }

    public void CreateRoom(Vector3[] points)
    {
        if (points.Length < 3) return;

        if (CreatedRoom != null)
        {
            Destroy(CreatedRoom);
            CreatedRoom = null;
        }

        Room rm = new Room(points, initId++, wallThickness, wallHeight);
        generatedRooms.Add(rm);
    }

    public (WallSegment segment, WallSegment friend) GetWallSegmentAndFriendFromInsID(int instanceID)
    {
        WallSegment found = null;
        WallSegment frnd = null;

        foreach (Room room in generatedRooms)
        {
            foreach (Wall wall in room.outerWalls)
            {
                for (int i = 0; i < wall.segments.Count; i++)
                {
                    if (wall.segments[i].objectInstance.GetInstanceID() == instanceID)
                    {
                        found = wall.segments[i];
                        int friendIndx = (i == 0) ? 1 : 0;
                        frnd = wall.segments[friendIndx];
                        break;
                    }
                    else continue;
                }
                if (found != null) break;
            }
            if (found != null) break;
        }

        return (segment: found, friend: frnd);
    }

    public Room GetRoom(int index)
    {
        return generatedRooms[index];
    }

    public bool TorqueDown(List<Vector3> _points)
    {
        List<Vector3> rights = new List<Vector3>();
        List<Vector3> lefts = new List<Vector3>();

        for (int i = 0; i < _points.Count; i++)
        {
            int nIndx = (i == _points.Count - 1) ? 0 : i + 1;

            Vector3 vtn = (_points[nIndx] - _points[i]).normalized;
            Vector3 vtr = (Quaternion.Euler(0f, 90f, 0f) * vtn).normalized;
            rights.Add(_points[i] + vtr);
            lefts.Add(_points[i] - vtr);
        }

        Bounds br = new Bounds(rights[0], Vector3.zero);
        Bounds bl = new Bounds(lefts[0], Vector3.zero);

        rights.GetRange(1, rights.Count - 1).ForEach(i => br.Encapsulate(i));
        lefts.GetRange(1, lefts.Count - 1).ForEach(i => bl.Encapsulate(i));

        if (br.size.magnitude > bl.size.magnitude) return false;

        return true;
    }
}
public class Room
{
    public int roomID;
    public Vector3[] points;
    public List<GameObject> seperators;
    public List<GameObject> corners;
    public GameObject instance;
    public List<Wall> outerWalls;
    public WallSegment bottom;
    public WallSegment floor;
    public WallSegment ceiling;
    public WallSegment top;
    public float thickness;
    public float height;

    public Room(Vector3[] p_points, int p_id, float p_thickness, float p_height)
    {
        CreateRoom(p_points, p_id, p_thickness, p_height);
    }

    private void CreateRoom(Vector3[] _points, int _id, float _thickness, float _height)
    {
        seperators = new List<GameObject>();
        corners = new List<GameObject>();
        points = _points;
        roomID = _id;
        thickness = _thickness;
        height = _height;

        GameObject newRoom = GameObject.Instantiate(RoomBuilder.Instance.emptyReference, Vector3.zero, Quaternion.identity, RoomBuilder.Instance.roomsParent);
        newRoom.name = "ROOM." + roomID;
        instance = newRoom;
        

        List<Vector3> verts_1_down = new List<Vector3>();
        List<Vector3> verts_1_up = new List<Vector3>();
        List<Vector3> verts_2_down = new List<Vector3>();
        List<Vector3> verts_2_up = new List<Vector3>();

        List<Vector3> verts_lid_down = new List<Vector3>();
        List<Vector3> verts_lid_up = new List<Vector3>();

        List<WallSegment> inWalls = new List<WallSegment>();
        List<WallSegment> outWalls = new List<WallSegment>();
        List<WallSegment> lidWalls = new List<WallSegment>();

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
            if (mid.magnitude == 0) mid = Vector3.Cross(vtn, new Vector3(0f, 1f, 0f)).normalized;
            float angle = Vector3.Angle(vtp, mid);
            float sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            sin = (sin == 0) ? 1 : sin;
            float ntck = _thickness / sin;
            ntck = (ntck <= 0) ? _thickness : ntck;



            Vector3 vx = (points[nin] - points[pin]).normalized;
            Vector3 vs = (points[i] - points[pin]).normalized;


            Vector3 perp = Vector3.Cross(vx, vs);
            float dir = Vector3.Dot(perp, new Vector3(0f, 1f, 0f));
            bool concave = (dir >= 0f);

            mid = concave ? -1 * mid : mid;

            Vector3 midHighPoint = points[i] + mid * (ntck * 0.5f);
            Vector3 outHighPoint = points[i] - mid * (ntck * 0.5f);
            midHighPoint.y += _height;
            outHighPoint.y += _height;

            verts_1_down.Add(points[i] + mid * (ntck * 0.5f));
            verts_1_up.Add(midHighPoint);
            verts_2_down.Add(points[i] - mid * (ntck * 0.5f));
            verts_2_up.Add(outHighPoint);

            verts_lid_down.Add(outHighPoint);
            verts_lid_up.Add(midHighPoint);
        }

        bool trqDwn = RoomBuilder.Instance.TorqueDown(points.ToList());

        _ = new List<Vector3>();
        List<Vector3> verts_in_down;
        _ = new List<Vector3>();
        List<Vector3> verts_in_up;
        _ = new List<Vector3>();
        List<Vector3> verts_out_down;
        _ = new List<Vector3>();
        List<Vector3> verts_out_up;
        if (!trqDwn)
        {
            verts_out_down = verts_1_down;
            verts_out_up = verts_1_up;
            verts_in_down = verts_2_down;
            verts_in_up = verts_2_up;

            verts_out_down.Reverse();
            verts_out_up.Reverse();
            verts_in_down.Reverse();
            verts_in_up.Reverse();

            verts_lid_down.Reverse();
            verts_lid_up.Reverse();

            Array.Reverse(points);
        }
        else
        {
            verts_out_down = verts_2_down;
            verts_out_up = verts_2_up;
            verts_in_down = verts_1_down;
            verts_in_up = verts_1_up;
        }

        for (int i = 0; i < points.Length; i++)
        {
            GameObject crnr = GameObject.Instantiate(RoomBuilder.Instance.cornerReference, points[i], Quaternion.identity, newRoom.transform);
            crnr.layer = 8;
            crnr.name = "CORNER." + i.ToString();
        }

        WallSegment wlBottom = new WallSegment();
        WallSegment wlFloor = new WallSegment();
        WallSegment wlCeiling = new WallSegment();
        WallSegment wlTop = new WallSegment();
        for (int i = 0; i < points.Length; i++)
        {
            int nIndex = (i == verts_in_down.Count - 1) ? 0 : i + 1;
            float vertDist = 0.005f;

            WallSegment wlIn = new WallSegment();
            wlIn.verts.Add(new Vector3(verts_in_down[i].x, verts_in_down[i].y + vertDist, verts_in_down[i].z));
            wlIn.verts.Add(new Vector3(verts_in_up[i].x, verts_in_up[i].y - vertDist, verts_in_up[i].z));
            wlIn.verts.Add(new Vector3(verts_in_up[nIndex].x, verts_in_up[nIndex].y - vertDist, verts_in_up[nIndex].z));
            wlIn.verts.Add(new Vector3(verts_in_down[nIndex].x, verts_in_down[nIndex].y + vertDist, verts_in_down[nIndex].z));

            WallSegment wlOut = new WallSegment();
            wlOut.verts.Add(verts_out_down[i]);
            wlOut.verts.Add(verts_out_up[i]);
            wlOut.verts.Add(verts_out_up[nIndex]);
            wlOut.verts.Add(verts_out_down[nIndex]);

            WallSegment wlLid = new WallSegment();
            wlLid.verts.Add(new Vector3(verts_lid_down[i].x, verts_lid_down[i].y - vertDist, verts_lid_down[i].z));
            wlLid.verts.Add(new Vector3(verts_lid_up[i].x, verts_lid_up[i].y - vertDist, verts_lid_up[i].z));
            wlLid.verts.Add(new Vector3(verts_lid_up[nIndex].x, verts_lid_up[nIndex].y - vertDist, verts_lid_up[nIndex].z));
            wlLid.verts.Add(new Vector3(verts_lid_down[nIndex].x, verts_lid_down[nIndex].y - vertDist, verts_lid_down[nIndex].z));

            wlBottom.verts.Add(verts_out_down[i]);
            wlFloor.verts.Add(new Vector3(verts_in_down[i].x, verts_in_down[i].y + vertDist, verts_in_down[i].z));
            wlCeiling.verts.Add(new Vector3(verts_in_up[i].x, verts_in_up[i].y - vertDist, verts_in_up[i].z));
            wlTop.verts.Add(verts_out_up[i]);

            inWalls.Add(wlIn);
            outWalls.Add(wlOut);
            lidWalls.Add(wlLid);
        }

        outerWalls = new List<Wall>();
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 wallPos = inWalls[i].verts[0] + (Vector3.Distance(inWalls[i].verts[0], inWalls[i].verts[3]) * 0.5f) * (outWalls[i].verts[3] - inWalls[i].verts[0]).normalized;
            Vector3 normalizedCenterPoint = Quaternion.Euler(0f, 90f, 0f) * (inWalls[i].verts[3] - inWalls[i].verts[0]).normalized;
            Vector3 cent = normalizedCenterPoint + wallPos;
            GameObject totalWall = GameObject.Instantiate(RoomBuilder.Instance.emptyReference, wallPos, Quaternion.identity, newRoom.transform);
            totalWall.name = "WALL." + i.ToString();
            totalWall.transform.LookAt(cent);

            GameObject inOBJ = GenerateSegment(totalWall.transform, inWalls[i], new Vector3(0f, 0f, 1f), true);
            inOBJ.transform.position = inWalls[i].verts[0] + (inWalls[i].verts[3] - inWalls[i].verts[0]).normalized * (Vector3.Distance(inWalls[i].verts[0], inWalls[i].verts[3]) * 0.5f);
            inOBJ.name = "INSIDE";

            GameObject outObj = GenerateSegment(totalWall.transform, outWalls[i], new Vector3(0f, 0f, 1f), true);
            outObj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            outObj.transform.position = outWalls[i].verts[0] + (outWalls[i].verts[3] - outWalls[i].verts[0]).normalized * (Vector3.Distance(outWalls[i].verts[0], outWalls[i].verts[3]) * 0.5f);
            outObj.GetComponent<MeshRenderer>().material = RoomBuilder.Instance.outMat;
            outObj.name = "OUTSIDE";

            GameObject lidOBJ = GenerateSegment(totalWall.transform, lidWalls[i], new Vector3(0f, 1f, 0f));
            lidWalls[i].objectInstance = lidOBJ;
            lidOBJ.transform.position = Vector3.zero;
            lidOBJ.transform.eulerAngles = Vector3.zero;
            lidOBJ.name = "TOP";

            Wall wall = new Wall(new List<WallSegment> { outWalls[i], inWalls[i], lidWalls[i] })
            {
                instance = totalWall
            };
            outerWalls.Add(wall);
        }

        GameObject floorOBJ = GenerateSegment(newRoom.transform, wlFloor, new Vector3(0f, 1f, 0f));
        floorOBJ.GetComponent<MeshRenderer>().material = RoomBuilder.Instance.floorMat;
        floorOBJ.name = "ROOM_FLOOR";
        floorOBJ.layer = 10;
        floor = wlFloor;

        GameObject bottomOBJ = GenerateSegment(newRoom.transform, wlBottom, new Vector3(0f, -1f, 0f));
        bottomOBJ.GetComponent<MeshRenderer>().material = RoomBuilder.Instance.outMat;
        bottomOBJ.name = "ROOM_BOTTOM";
        bottomOBJ.layer = 11;
        bottom = wlBottom;

        GameObject ceilingOBJ = GenerateSegment(newRoom.transform, wlCeiling, new Vector3(0f, -1f, 0f));
        ceilingOBJ.GetComponent<MeshRenderer>().material = RoomBuilder.Instance.outMat;
        ceilingOBJ.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        ceilingOBJ.name = "ROOM_CEILING";
        ceilingOBJ.layer = 11;
        ceiling = wlCeiling;

        GameObject topOBJ = GenerateSegment(newRoom.transform, wlTop, new Vector3(0f, 1f, 0f));
        topOBJ.GetComponent<MeshRenderer>().material = RoomBuilder.Instance.outMat;
        topOBJ.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        topOBJ.name = "ROOM_TOP";
        topOBJ.layer = 11;
        top = wlTop;
    }

    public void AddPoint(int _splitedWallIndex, Vector3 _newPoint)
    {

        int indx = _splitedWallIndex + 1;
        List<Vector3> pts = points.ToList();
        pts.Insert(indx, _newPoint);
        this.ClearRoom();
        this.CreateRoom(pts.ToArray(), roomID, thickness, height);
    }

    public void ExpandRoom(int _firstWall, int _secondWall, Vector3 _firstPoint, Vector3 _secondPoint, List<Vector3> _extra, bool _reverseFlow = false)
    {
        List<Vector3> pts = points.ToList();
        if (_reverseFlow)
        {
            Vector3 tmp = _firstPoint;
            _firstPoint = _secondPoint;
            _secondPoint = tmp;

            int wlTmp = _firstWall;
            _firstWall = _secondWall;
            _secondWall = wlTmp;

            _extra.Reverse();
        }
        
        int indx = _firstWall + 1;
        List<Vector3> tbr = new List<Vector3>();

        int i = _firstWall;
        while (i != _secondWall)
        {
            int nIndx = (i + 1 == pts.Count) ? 0 : i + 1;
            i = nIndx;
            tbr.Add(pts[i]);
        }

        pts.Insert(indx++, _firstPoint);
        foreach (Vector3 vec in _extra) pts.Insert(indx++, vec);
        pts.Insert(indx++, _secondPoint);
        pts.RemoveAll(p => tbr.Exists(t => p == t));
        this.ClearRoom();
        this.CreateRoom(pts.ToArray(), roomID, thickness, height);
    }

    public void UpdateSegmentsMesh()
    {
        foreach (Wall wall in outerWalls)
        {
            foreach (WallSegment segment in wall.segments)
            {
                segment.UpdateMesh();
            }
        }
    }

    private GameObject GenerateSegment(Transform parent, WallSegment seg, Vector3 normal, bool s2z = false)
    {
        GameObject segOBJ = GameObject.Instantiate(RoomBuilder.Instance.segmentReference, parent);
        seg.objectInstance = segOBJ;
        Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon
        {
            outside = s2z ? SegmentToZero(seg) : seg.verts
        };
        //poly.holes = s2z ? SegmentToZero(seg) : seg.holes;
        poly.CalcPlaneNormal(normal);
        segOBJ.GetComponent<MeshFilter>().mesh = Poly2Mesh.CreateMesh(poly);
        segOBJ.GetComponent<MeshCollider>().sharedMesh = Poly2Mesh.CreateMesh(poly);
        return segOBJ;
    }

    private List<Vector3> SegmentToZero(WallSegment seg)
    {
        int mult = -1;
        List<Vector3> newV3s = new List<Vector3>();
        float len = Vector3.Distance(seg.verts[0], seg.verts[3]);
        float height = seg.verts[1].y - seg.verts[0].y;
        newV3s.Add(new Vector3(mult * len * 0.5f, 0f, 0f));
        newV3s.Add(new Vector3(mult * len * 0.5f, height, 0f));
        newV3s.Add(new Vector3(-mult * len * 0.5f, height, 0f));
        newV3s.Add(new Vector3(-mult * len * 0.5f, 0f, 0f));
        return newV3s;
    }

    public void ClearRoom()
    {
        outerWalls.ForEach(p => p.DestroySegments());
        outerWalls.Clear();
        GameObject.Destroy(bottom.objectInstance);
        GameObject.Destroy(floor.objectInstance);
        GameObject.Destroy(ceiling.objectInstance);
        GameObject.Destroy(top.objectInstance);
        GameObject.Destroy(instance);
    }
}

public class Wall
{
    public GameObject instance;
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

    public void DestroySegments()
    {
        segments.ForEach(p => GameObject.Destroy(p.objectInstance));
        segments.Clear();
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
        Poly2Mesh.Polygon newPoly = new Poly2Mesh.Polygon
        {
            outside = verts,
            holes = holes
        };
        newPoly.CalcPlaneNormal(normal);
        Mesh createdmesh = Poly2Mesh.CreateMesh(newPoly);
        objectInstance.GetComponent<MeshFilter>().mesh = createdmesh;
        objectInstance.GetComponent<MeshCollider>().sharedMesh = createdmesh;
    }
}
