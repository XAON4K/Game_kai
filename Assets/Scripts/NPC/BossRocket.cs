using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocket : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 20;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float lifeTime = 3;
    [SerializeField] private float overlapRadius = 2.4f;
    [SerializeField] private int damage = 35;

    [Space(5)]

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosionSound;

    Transform _player;

    private void Start()
    {
        _player = PlayerController.GetPlayer();

        StartCoroutine(LifeTimer());
    }

    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }

    private void Update()
    {
        Quaternion rotTarget = Quaternion.LookRotation(_player.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, rotationSpeed * Time.deltaTime);

        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Overlap();

        Instantiate(explosionSound, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void Overlap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, overlapRadius);

        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("Player"))
            {
                PlayerController.GiveDamage(damage);
                return;
            }
        }
    }
}
