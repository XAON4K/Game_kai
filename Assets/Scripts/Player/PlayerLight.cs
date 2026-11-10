using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    Transform _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        transform.position = _player.position;
    }
}
