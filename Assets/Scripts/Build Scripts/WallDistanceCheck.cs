using UnityEngine;

public class WallDistanceCheck : MonoBehaviour
{
    public bool isColliding;
    int colliding = 0;
    public int connectedWallID = -1;

#pragma warning disable IDE0051 // Remove unused private members
    private void Awake()
#pragma warning restore IDE0051 // Remove unused private members
    {
        isColliding = false;
        colliding = 0;
        connectedWallID = -1;
    }


#pragma warning disable IDE0051 // Remove unused private members
    private void OnTriggerEnter(Collider other)
#pragma warning restore IDE0051 // Remove unused private members
    {
        switch (other.gameObject.layer)
        {
            case 8:
                if (int.Parse(other.transform.parent.gameObject.name.Split('.')[1]) != connectedWallID)
                {
                    colliding++;
                    isColliding = true;
                }
                
                break;

            case 9:
                if (int.Parse(other.transform.parent.gameObject.name) != int.Parse(transform.parent.name) - 1)
                {
                    colliding++;
                    isColliding = true;
                }
                break;

            default:
                break;
        }
    }

#pragma warning disable IDE0051 // Remove unused private members
    private void OnTriggerExit(Collider other)
#pragma warning restore IDE0051 // Remove unused private members
    {
        switch (other.gameObject.layer)
        {
            case 8:
                colliding--;
                if (colliding <= 0)
                {
                    colliding = 0;
                    isColliding = false;
                }
                break;

            case 9:
                if (int.Parse(other.transform.parent.gameObject.name) != int.Parse(transform.parent.name) - 1 && --colliding <= 0)
                {
                    colliding = 0;
                    isColliding = false;
                }
                break;

            default:
                break;
        }
    }
}


