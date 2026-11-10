using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    private Transform _lastCheckpoint;
    private Transform _player;

    private static CheckpointSystem instance;

    private void Awake()
    {
        instance = this;
    }

    public static void SetLastCheckpoint(Transform checkPoint)
    {
        instance._lastCheckpoint = checkPoint;
    }

    public static void MovePlayerToLastCheckpoint()
    {
        if (instance._player == null) GetPlayer();
        if (instance._lastCheckpoint == null || instance._player == null)
        {
            Debug.LogWarning("Cannot restore the player: no checkpoint has been activated yet.");
            return;
        }

        CharacterController _playerController = instance._player.GetComponent<CharacterController>();
        if (_playerController == null)
        {
            Debug.LogWarning("Cannot restore the player: CharacterController is missing.");
            return;
        }
        _playerController.enabled = false;

        instance._player.position = instance._lastCheckpoint.position;
        instance._player.rotation = instance._lastCheckpoint.rotation;

        _playerController.enabled = true;
    }

    private static void GetPlayer()
    {
        instance._player = PlayerController.GetPlayer();
    }
}
