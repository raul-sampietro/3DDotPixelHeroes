using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraMovement
{
    ATTACHED_TO_ROOM,
    ATTACHED_TO_PLAYER
}

public class CameraMover : MonoBehaviour
{
    private GameObject knight = null;

    private Vector3 camPosition = new(0, 0, 0);
    public Vector3 camOffset;
    private Vector2 sizeOfRoom = new(256, 192);

    // 0 -> fixed to the actual level
    // 1 -> attached to the player
    private CameraMovement cameraState = CameraMovement.ATTACHED_TO_ROOM;
    private Vector2 bossRoomBottomLeft;
    private Vector2 bossRoomTopRight;

    public void SetBossRooms(Vector2 bottomLeft, Vector2 topRight)
    {
        bossRoomBottomLeft = bottomLeft;
        bossRoomTopRight = topRight;
        Debug.Log("" + bottomLeft + topRight);
    }

    public void ChangeCurrentRoom(Vector2 roomNumber)
    {
        if (roomNumber.x >= bossRoomBottomLeft.x && roomNumber.x <= bossRoomTopRight.x &&
            roomNumber.y >= bossRoomBottomLeft.y && roomNumber.y <= bossRoomTopRight.y)
        {
            // It is into a bossRoom
            Debug.Log("Boss Room");
            cameraState = CameraMovement.ATTACHED_TO_PLAYER;
        }
        else
        {
            // It is not into a bossRoom
            cameraState = CameraMovement.ATTACHED_TO_ROOM;
            camPosition.x = (sizeOfRoom.x * roomNumber.x);
            camPosition.z = (sizeOfRoom.y * roomNumber.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player
        if (knight == null) knight = GameObject.FindGameObjectWithTag("Player");

        switch (cameraState)
        {
            case CameraMovement.ATTACHED_TO_ROOM:
                transform.position = Vector3.MoveTowards(transform.position, camPosition + camOffset, 5);
                break;

            case CameraMovement.ATTACHED_TO_PLAYER:
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
