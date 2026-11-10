using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;

    [SerializeField] private Transform playerBody;

    [SerializeField] private float defaultRotation = 0;

    float xRotation = 0f;

    bool isRotationEnabled = false;

    private void Awake()
    {
        xRotation = defaultRotation;

        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(EnableRotation());

        UpdateSens();
    }

    private void LateUpdate()
    {
        if(!isRotationEnabled) return; 

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    IEnumerator EnableRotation()
    {
        for(int i = 0; i < 6; i++)
            yield return new WaitForEndOfFrame();

        isRotationEnabled = true;
    }

    private float MapSensetivity(float value)
    {
        return value * 340 + 0;
    }

    public void UpdateSens()
    {
        float s = PlayerPrefs.GetFloat("Sensetivity");
        mouseSensitivity = MapSensetivity(s > 0 ? s : .6f);
        Debug.Log($"new sens - {mouseSensitivity}");
    }
}
