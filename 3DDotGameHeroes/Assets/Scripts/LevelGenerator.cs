using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class LevelGenerator : MonoBehaviour
{
    public Array2DInt levelsList;
    public Texture2D[] levelsMappings;
    public ColorToPrefab[] colorMappings;
    public GameObject floor;

    private Vector2 sizeOfImage = new(16, 12);
    private bool[,] activeRooms = new bool[10, 10];

    // Entry point to manage the rooms that have to be instanciated
    public void InstanciateRoom(int i, int j)
    {
        // Instanciate this room and those around it
        for (int x = i - 1; x <= i + 1; ++x)
            for (int y = j - 1; y <= j + 1; ++y)
                InstRoomByCords(x, y);
    }

    private void InstRoomByCords(int i, int j)
    {
        Vector2Int boundaries = levelsList.GridSize;
        // Check that we are not outside the grid range
        if (!(i >= 0 && j >= 0 && i < boundaries.x && j < boundaries.y)) return;
       
        int roomNumber = levelsList.GetCell(i, boundaries.y - 1 - j) - 1;
        // Check that we are not outside the array range
        if (roomNumber >= levelsMappings.Length || roomNumber < 0) return;
        Texture2D level = levelsMappings[roomNumber];

        // Check if it is already instanciated, otherwise mark it as so
        if (activeRooms[i,j]) return;
        activeRooms[i, j] = true;

        GameObject levelObject = new();
        levelObject.name = "Room" + i + j;
        
        for (int x = 0; x < level.width; ++x)
        {
            for (int z = 0; z < level.height; ++z)
            {
                Color pixelColor = level.GetPixel(x, z);

                // Calcualte relative position
                Vector3 offset = new(sizeOfImage.x * i * sizeOfImage.x, 0, sizeOfImage.x * j * sizeOfImage.y);
                Vector3 position = new(x * sizeOfImage.x, 0, z * sizeOfImage.x);
                position += offset;

                if (pixelColor.a > 0) // Pixel not transparent
                {
                    foreach (ColorToPrefab colorPrefab in colorMappings)
                    {
                        //Debug.Log("PixelColor: " + pixelColor);
                        //Debug.Log("colorPrefab: " + colorPrefab.color);
                        //Debug.Log("X: " + x);
                        //Debug.Log("Z: " + z);

                        // Color matches and we are not in the corners
                        if (colorPrefab.color.Equals(pixelColor) &
                            !(x == 0 & z == 0) & // Bottom-left corner
                            !(x == 0 & z == level.height - 1) & // Top-left corner
                            !(x == level.width - 1 & z == 0) & // Bottom-right corner
                            !(x == level.width - 1 & z == level.height - 1)) // Top-right corner
                        {
                            GameObject obj = Instantiate(colorPrefab.prefab, position, Quaternion.identity, transform);

                            // Scale, rotate and move the asset
                            switch (colorPrefab.prefab.name)
                            {
                                case "wall":

                                    Vector3 rotationX = new(0, 0, 0);
                                    Vector3 torchOffset = new(0, 15, 0);
                                    // Rotate according to position X in the level
                                    if (x == 0) // Left side
                                    {
                                        rotationX = new(0.0f, 90.0f, 0.0f);
                                        torchOffset += new Vector3(13, 0, 0);
                                    }
                                    else if (x == level.width - 1) // Right side
                                    {
                                        rotationX = new(0.0f, -90.0f, 0.0f);
                                        torchOffset += new Vector3(-13, 0, 0);
                                    }
                                    obj.transform.Rotate(rotationX, Space.World);

                                    Vector3 rotationZ = new(0, 0, 0);
                                    // Rotate according to position Y in the level
                                    if (z == 0) // Bottom side
                                    {
                                        rotationZ = new(0.0f, 0.0f, 0.0f);
                                        torchOffset += new Vector3(0, 0, 13);
                                    }
                                    else if (z == level.height - 1) // Top side
                                    {
                                        rotationZ = new(0.0f, -180.0f, 0.0f);
                                        torchOffset += new Vector3(0, 0, -13);
                                    }
                                    obj.transform.Rotate(rotationZ, Space.World);

                                    // Check if a torch has to be added, if so, apply the transformations
                                    if (colorPrefab.torch != null)
                                    {
                                        GameObject torch = Instantiate(colorPrefab.torch, position + torchOffset, Quaternion.identity, transform);
                                        torch.transform.Rotate(rotationX, Space.World);
                                        torch.transform.Rotate(rotationZ, Space.World);

                                        // Set parent object
                                        torch.transform.parent = levelObject.transform;
                                    }

                                    break;

                                case "batFlying":
                                    transform.Translate(new Vector3(0, 7, 0), Space.World);
                                    break;

                                default:
                                    break;
                            }

                            // Check for special options
                            if (colorPrefab.dropCoin)
                            {
                                // Assing this property to the gameObject

                            }



                            if (colorPrefab.dropLife)
                            {
                                // Assing this property to the gameObject

                            }

                            if (colorPrefab.movementPattern != null)
                            {
                                // Assing this property to the gameObject depending on its type
                                // The gameObject movement script should implement the method
                                // TODO establecer capas y comprobar que esta en la capa que toca (enemies, obstacles, etc...)
                                switch (obj.name)
                                {
                                    case "AnubisIdle(Clone)":
                                    case "SkeletonIdle(Clone)":
                                    case "batFlying(Clone)":
                                    case "scorpionIdle(Clone)":
                                        obj.BroadcastMessage("SetMovementPattern", colorPrefab.movementPattern);
                                        break;

                                    default:
                                        break;
                                }
                            }

                            // Set parent object
                            obj.transform.parent = levelObject.transform;

                            if (colorPrefab.prefab.name != "wall" &
                                colorPrefab.prefab.name != "BigBox")
                            {
                                // Instanciate the floor
                                GameObject floorObject = Instantiate(floor, position, Quaternion.identity, transform);
                                floorObject.transform.parent = levelObject.transform;
                            }
                        }
                    }
                }
                else
                {
                    // Instanciate the floor wherever there is a blank pixel
                    GameObject floorObject = Instantiate(floor, position, Quaternion.identity, transform);
                    floorObject.transform.parent = levelObject.transform;
                }
            }
        }
    }

    void Start()
    {
        for (int i = 0; i < 10; ++i)
            for (int j = 0; j < 10; ++j)
                InstRoomByCords(i, j);

        //InstanciateRoom(3, 3);

        // TODO manage the rooms that have to be "deinstanciated"
    }
}
