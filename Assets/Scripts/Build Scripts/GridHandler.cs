using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GridHandler : MonoBehaviour
{
    public static GridHandler Instance;

    public float gridSize = 1f;
    private MeshRenderer meshRenderer;
    public bool gridStatus = false;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        gridStatus = true;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        gridSize = 1f;
    }

    public void ToggleGrid()
    {
        gridStatus = !gridStatus;
        meshRenderer.enabled = gridStatus;
    }

    public void ChangeGridSize(float newSize)
    {
        float sz = newSize >= 0f ? newSize : 0f;
        gridSize = sz;
        meshRenderer.material.SetFloat("_Tiling", 1f / sz);
    }

    public void MultiplyGridSize(float multiplier)
    {
        gridSize *= multiplier;
        meshRenderer.material.SetFloat("_Tiling", 1f / gridSize);
    }

    public Vector3 GetClosestGridPoint(Vector3 pos)
    {
        int cX = Mathf.RoundToInt(pos.x / gridSize);
        int cZ = Mathf.RoundToInt(pos.z / gridSize);

        Vector3 gridPosition = new Vector3((float)cX * gridSize, transform.position.y, (float)cZ * gridSize);
        return gridPosition;
    }

    public Vector3 PointOnGridHeight(Vector3 pos)
    {
        Vector3 newPos = pos;
        newPos.y = transform.position.y;
        return newPos;
    }
}
