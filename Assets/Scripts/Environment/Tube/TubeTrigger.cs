using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeTrigger : MonoBehaviour
{
    [SerializeField] private bool setFallSpeedLimit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.GetComponentInParent<PlayerMovement>().SetFallSpeedLimit(setFallSpeedLimit);
    }
}
