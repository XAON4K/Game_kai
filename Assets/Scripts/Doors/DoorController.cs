using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float talkDistance = 5f;

    [SerializeField] private GameObject eIcon;
    [SerializeField] private Transform _Self;

    [SerializeField] private GameObject doorSoundPrefab;

    Transform player;
    Transform _camera;

    bool isPlayerNear = false;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        if (Vector3.Distance(player.position, _Self.position) < talkDistance)
        {
            eIcon.SetActive(true);
            isPlayerNear = true;

            if (Input.GetKeyDown(KeyCode.E))
                ReadRayCast();
        }
        else if (isPlayerNear)
        {
            isPlayerNear = false;
            eIcon.SetActive(false);
        }
    }

    private void ReadRayCast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, talkDistance))
        {
            if (hit.collider.gameObject == gameObject)
                Open();
        }
    }

    private void Open()
    {
        eIcon.SetActive(false);
        Instantiate(doorSoundPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
