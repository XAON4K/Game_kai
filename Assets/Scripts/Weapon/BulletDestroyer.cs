using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    private float lifeTime = 1.3f;
    private float starterTime;

    private void Awake()
    {
        starterTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > lifeTime + starterTime)
            Destroy(gameObject);
    }
}
