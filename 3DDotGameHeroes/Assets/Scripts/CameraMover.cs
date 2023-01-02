using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private GameObject knight = null;

    private Vector3 camPosition = new(0, 0, 0);
    public Vector3 camOffset;
    private Vector2 sizeOfRoom = new(256, 192);

    // 0 -> fixed to the actual level
    // 1 -> attached to the player
    private int cameraState = 0;

    public void ChangeCurrentRoom(Vector2 roomNumber)
    {
        camPosition.x = (sizeOfRoom.x * roomNumber.x);
        camPosition.z = (sizeOfRoom.y * roomNumber.y);
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null)
            knight = GameObject.FindGameObjectWithTag("Player");

        if (knight.transform.position.z < 0) // Player is outside the dungeon
            cameraState = 1;
        else // Player is inside the dungeon
            cameraState = 0;

        // Check if we are in the final boss room, if so, cameraState = 1


        switch (cameraState)
        {
            case 0: // Fixed to the level
                transform.position = Vector3.MoveTowards(transform.position, camPosition + camOffset, 5);

                break;

            case 1: // Attached to the player
                // Preserve the height and set an offset
                Vector3 newPos = knight.transform.position;
                newPos.y = transform.position.y;
                newPos.z -= 50;
                Vector3 smoothFollow = Vector3.Lerp(transform.position, newPos, 0.1f);
                transform.position = smoothFollow;
                break;

            default:
                break;
        }
    }
}
