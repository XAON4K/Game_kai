using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyDoor : MonoBehaviour
{
    [SerializeField] private int keyIndex;
    [SerializeField] private GameObject soundPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (PlayerController.ReadKey(keyIndex))
            {
                Instantiate(soundPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
