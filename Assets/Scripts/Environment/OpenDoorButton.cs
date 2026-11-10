using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorButton : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;

    public void OpenDoor()
    {
        Destroy(doorObject);
        Destroy(gameObject);
    }
}
