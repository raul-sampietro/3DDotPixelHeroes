using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Camera cam = null;
    private GameObject knight = null;

    // 0 -> fixed to the actual level
    // 1 -> attached to the player
    private int cameraState = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the camera according to two states, follow the player or fixed to the room

        // Find the camera
        if (cam == null)
            cam = GameObject.Find("OverviewCamera").GetComponent<Camera>();

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
                Vector3 levelPos = new(120, 100, -20);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, levelPos, 1);

                break;

            // Attached to the player
            case 1:
                Vector3 newPos = knight.transform.position;
                // Preserve the height and set an offset
                newPos.y = cam.transform.position.y;
                newPos.z -= 50;
                Vector3 smoothFollow = Vector3.Lerp(cam.transform.position, newPos, 0.1f);
                cam.transform.position = smoothFollow;
                break;

            default:
                break;
        }
        //cam.transform.Translate(new Vector3(1, 0, 0));
    }
}
