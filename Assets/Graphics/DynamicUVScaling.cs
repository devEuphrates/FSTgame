using UnityEngine;

public class DynamicUVScaling : MonoBehaviour
{
    Vector3 savedLossy = new Vector3(0f, 0f, 0f);
    MeshRenderer mr = null;
    [SerializeField]private bool isStatic;
    private void Awake()
    {
        isStatic = false;
        savedLossy = transform.lossyScale;
        mr = transform.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            Debug.LogError("Could not find a mesh renderer on same object. Destroying script instance...");
            Destroy(this);
        }
        else mr.material.SetVector("Vector2_7b9129be19564dec94fcbd6b70468c59", new Vector4(transform.lossyScale.z, 1f, transform.lossyScale.x, 0f));
    }
    private void Update()
    {
        if (isStatic) return;

        if (transform.lossyScale != savedLossy)
        {
            mr.material.SetVector("Vector2_7b9129be19564dec94fcbd6b70468c59", new Vector4(transform.lossyScale.z, 1f, transform.lossyScale.x, 0f));
            savedLossy = transform.lossyScale;
        }
    }

    public void SetStaticTileSize(Vector3 p1, Vector3 p2, bool changeY = false)
    {
        isStatic = true;
        Vector3 p3 = new Vector3(p2.x, p1.y, p2.z);
        p1.y = p2.y;
        float yAmt = changeY ? p1.y - p2.y : 1f;
        mr.material.SetVector("Vector2_7b9129be19564dec94fcbd6b70468c59", new Vector4(Vector3.Distance(p1, p3), yAmt, 1f, 0f));
    }

    public void ConvertToDynamic()
    {
        isStatic = false;
    }
}
