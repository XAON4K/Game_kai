using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBeanController : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 1.2f;
    [SerializeField] private float spawnYOffset = -.5f;

    [Space(5)]

    [SerializeField] private int beansCount;
    [SerializeField] private int healthBeansCount;

    [Space(5)]

    [SerializeField] private GameObject beanPrefab;
    [SerializeField] private GameObject healthBeanPrefab;

    [Space(5)]

    [SerializeField] private GameObject collectSound;

    public void CollectBean()
    {
        Instantiate(collectSound, transform.position, Quaternion.identity);

        for (int i = 0; i < beansCount; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y + spawnYOffset, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
            Instantiate(beanPrefab, spawnPos, Quaternion.identity);
        }

        for(int i = 0;i < healthBeansCount; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y + spawnYOffset, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
            Instantiate(healthBeanPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
