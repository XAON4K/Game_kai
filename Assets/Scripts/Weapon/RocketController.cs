using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject _settings;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float damageRange;

    [SerializeField] private GameObject explosionSound;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRange);

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<EnemyController>() != null)
            {
                collider.GetComponent<EnemyController>().GiveDamage(_settings.damage);
            }
            else if (collider.GetComponent<BossController>() != null)
            {
                collider.GetComponent<BossController>().GiveDamage(_settings.damage);
            }
            else if (collider.GetComponent<OpenDoorButton>() != null)
            {
                collider.GetComponent<OpenDoorButton>().OpenDoor();
            }
            else if (collider.GetComponent<BigBeanController>() != null)
            {
                collider.GetComponent<BigBeanController>().CollectBean();
            }
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Instantiate(explosionSound, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
