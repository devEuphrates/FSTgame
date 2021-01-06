using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GridHandler : MonoBehaviour
{
    public static GridHandler Instance;

    public float gridSize = 1f;
    private MeshRenderer meshRenderer;
    private bool gridStatus = false;

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
        meshRenderer.material.SetFloat("_GridSize", sz);
    }

    public Vector3 GetClosestGridPoint(Vector3 pos)
    {
        int cX = Mathf.RoundToInt(pos.x / gridSize);
        int cZ = Mathf.RoundToInt(pos.z / gridSize);

        Vector3 gridPosition = new Vector3((float)cX * gridSize, transform.position.y, (float)cZ * gridSize);
        return gridPosition;
    }
}
