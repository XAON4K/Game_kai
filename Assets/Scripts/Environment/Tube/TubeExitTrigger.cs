using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneTransition.ChangeScene(SceneTransition.GetTubeSettings.scene);
        }
    }
}
