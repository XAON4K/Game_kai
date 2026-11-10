using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogeController : MonoBehaviour
{
    [SerializeField] private float talkDistance = 5f;

    [Space(6)]

    [SerializeField] private string[] texts;

    int textIndex;
    bool isTalkActive = false;

    [SerializeField] private GameObject eIcon;
    [SerializeField] private GameObject _talkPanel;
    [SerializeField] private Transform _Self;

    [Space(5)]

    [SerializeField] private bool giveSomethingInTheEnd = false;

    Text _talkPanelText;

    Transform player;
    Transform _camera;

    GiveItem _giveItem;

    bool isPlayerNear = false;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        _camera = Camera.main.transform;

        if(giveSomethingInTheEnd)
           _giveItem = GetComponent<GiveItem>();

        if(_talkPanel != null)
            _talkPanelText = _talkPanel.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if(isTalkActive)
        {
            if(Input.GetKeyDown(KeyCode.E))
                NextDialoge();

            return;
        }

        if(Vector3.Distance(player.position, _Self.position) < talkDistance)
        {
            eIcon.SetActive(true);
            isPlayerNear = true;

            if (Input.GetKeyDown(KeyCode.E))
                ReadRayCast();
        }
        else if(isPlayerNear)
        {
            isPlayerNear = false;
            eIcon.SetActive(false);
        }
    }

    private void ReadRayCast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, talkDistance))
        {
            if (hit.collider.gameObject == gameObject)
                Talk();
        }
    }

    public virtual void Talk()
    {
        _talkPanel.SetActive(true);
        isTalkActive = true;
        textIndex = 0;
        _talkPanelText.text = texts[0];
        player.GetComponent<PlayerMovement>().ChangeCanWalkStatus(false);
    }

    private void FinishDialoge()
    {
        _talkPanel.SetActive(false);
        isTalkActive = false;
        eIcon.SetActive(false);
        player.GetComponent<PlayerMovement>().ChangeCanWalkStatus(true);

        if (giveSomethingInTheEnd)
            _giveItem.Give();
    }

    private void NextDialoge()
    {
        textIndex++;
        if(textIndex >= texts.Length)
        {
            FinishDialoge();
            return;
        }

        _talkPanelText.text = texts[textIndex];
    }
}
