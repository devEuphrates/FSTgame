using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderScript : MonoBehaviour
{
    public bool IsColliding;

    private void Awake()
    {
        IsColliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IsColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsColliding = false;
    }
}
