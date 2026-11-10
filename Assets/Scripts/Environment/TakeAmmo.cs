using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeAmmo : MonoBehaviour
{
    [SerializeField] private int ammoCount = 30;
    [SerializeField] private AmmoType ammoType;

    [Space(5)]

    [SerializeField] private GameObject collectSound;

    public enum AmmoType
    {
        Autogun,
        Rocket_Launcher,
        Shotgun,
        Key,
        Health,
        Money
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            try
            {
                Instantiate(collectSound, transform.position, Quaternion.identity);
                PlayerController.AddAmmo(ammoCount, ammoType);
                Destroy(gameObject);
            }
            catch { }
        }
    }
}
