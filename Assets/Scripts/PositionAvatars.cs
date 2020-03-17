using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAvatars : MonoBehaviour
{
    // Use: Make empty Transforms for every spawnpoint and make a GameObject for the marker that moves to each transform depending on the client ID.
    // This script is controlled by IntSync (normcore.io script)
    
    // Index for avatar in room:
    public int avatarNum = -1;
    // GameObject that moves avatar to specified spawn point (attach to "Root" of Normal Local Avatar):
    [SerializeField]
    private GameObject spawnMarker;
    // List of transforms for each avatar that spawns in:
    [SerializeField]
    private Transform[] spawnPoints;
    // Track whether avatar has been moved or not: 
    [SerializeField]
    private bool avatarMoved = false;

    private void Start()
    {

    }

    void Update()
    {
        PosAvatar();
    }

    void PosAvatar() {
        // If avatar loads in and the build hasn't opened before:
        if (avatarNum >= 0 && avatarMoved == false)
        {
            spawnMarker.transform.localPosition = spawnPoints[avatarNum].position;
            spawnMarker.transform.localRotation = spawnPoints[avatarNum].rotation;
            avatarMoved = true;
        }           
    }
}
