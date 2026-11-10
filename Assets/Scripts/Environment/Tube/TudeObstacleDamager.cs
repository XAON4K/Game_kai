using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TudeObstacleDamager : MonoBehaviour
{
    [SerializeField] private int damage = 100;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnPlayerEntered();
        }
    }

    private void OnPlayerEntered()
    {
        PlayerController.GiveDamage(damage);
    }
}
