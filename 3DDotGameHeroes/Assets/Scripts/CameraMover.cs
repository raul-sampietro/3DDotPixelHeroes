using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private GameObject knight = null;

    private Vector3 initialCamPosition = new(128, 100, -20);
    private Vector3 camOffset = new(0, 0, 0);
    private Vector2 sizeOfRoom = new(265, 176);

    // 0 -> fixed to the actual level
    // 1 -> attached to the player
    private int cameraState = 0;

    public void ChangeCurrentRoom(int roomNumber)
    {
        camOffset.x = (int)initialCamPosition.x + (sizeOfRoom.x/2 * (int)(roomNumber % 3));
        camOffset.z = (int)initialCamPosition.z + (sizeOfRoom.y/2 * (int)(roomNumber / 3));
    }

    // Update is called once per frame
    void Update()
    {
        // Update the camera according to two states, follow the player or fixed to the room

        // Find the player
        if (knight == null)
            knight = GameObject.Find("Knight");

        // Player is outside the dungeon
        if (knight.transform.position.z < 0)
            cameraState = 1;
        // Player is inside the dungeon
        else
            cameraState = 0;

        // Check if we are in the final boss room, if so, cameraState = 1


        switch (cameraState)
        {
            // Fixed to the level
            case 0:
                transform.position = Vector3.MoveTowards(transform.position, initialCamPosition + camOffset, 10);

                break;

            // Attached to the player
            case 1:
                Vector3 newPos = knight.transform.position;
                // Preserve the height and set an offset
                newPos.y = transform.position.y;
                newPos.z -= 50;
                Vector3 smoothFollow = Vector3.Lerp(transform.position, newPos, 0.1f);
                transform.position = smoothFollow;
                break;

            default:
                break;
        }
        //transform.Translate(new Vector3(1, 0, 0));
    }
}
