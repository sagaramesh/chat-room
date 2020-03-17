using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class IntSync : RealtimeComponent
{
    // Use: This script controls the PositionAvatars script by using the client ID to set the spawnMarker's transform. Also sets new prefab for new avatars. 

    private PositionAvatars _spawnAvatarScript;
    // List of prefabs for both avatars:
    [SerializeField]
    private GameObject[] avatarPrefabs;
    // Normal's realtime player and script attached:
    public GameObject realtimePlayer;
    private RealtimeAvatarManager _realtimeAvatarManager;
    private IntSyncModel _model;

    private void Start()
    {
        // Get a reference to the position avatars script:
        _realtimeAvatarManager = realtimePlayer.GetComponent<RealtimeAvatarManager>();
        _spawnAvatarScript = GetComponent<PositionAvatars>();
    }

    private IntSyncModel model
    {
        set
        {
            if (_model != null)
            {
                // Unregister from events
                _model.numDidChange -= NumDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update to match the new model
                UpdateAvatar();

                // Register for events so we'll know if the int changes later
                _model.numDidChange += NumDidChange;
            }
        }
    }

    private void NumDidChange(IntSyncModel model, int value)
    {
        // Update avatar positions based on client ID (Avatar 1 moves to pos 1, Avatar 2 to 2, etc.)
        UpdateAvatar();
    }

    private void UpdateAvatar()
    {
        _spawnAvatarScript.avatarNum = realtime.clientID;

        // When avatar loads in, change the prefab based on client ID:
        if (realtime.clientID >= 0)
        {
            _realtimeAvatarManager.localAvatarPrefab = avatarPrefabs[realtime.clientID];
        }
    }
}