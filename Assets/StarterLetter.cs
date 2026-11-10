using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterLetter : MonoBehaviour
{
    [SerializeField] private GameObject letter;

    private void Start()
    {
        PlayerController.ActivateLetter();
    }

    public void Close()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        letter.SetActive(false);
    }
}
