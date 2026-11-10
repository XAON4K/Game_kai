using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private float moveTime = 50;
    [SerializeField] private float moveOffset = 30;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool onLeft = true;

    private float elapsedTime;

    private void Awake()
    {
        NextPoint();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float percentage = elapsedTime / moveTime;

        Vector3 pos = Vector3.Lerp(startPos, endPos, percentage);

        transform.localPosition = pos;
    }

    private void NextPoint()
    {
        elapsedTime = 0;
        startPos = transform.localPosition;
        endPos = transform.localPosition + new Vector3(onLeft ? moveOffset : -moveOffset, 0);
    }
}
