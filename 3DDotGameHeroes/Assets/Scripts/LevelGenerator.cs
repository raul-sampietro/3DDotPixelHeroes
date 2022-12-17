using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D[] levels;
    public ColorToPrefab[] colorMappings;
    public GameObject floor;

    private Vector2 sizeOfImage = new(16, 12);
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
        foreach (Texture2D level in levels)
        {
            GameObject levelObject = new();
            levelObject.name = "Room" + (count + 1);
            for (int x = 0; x < level.width; ++x)
            {
                for (int z = 0; z < level.height; ++z)
                {
                    Color pixelColor = level.GetPixel(x, z);

                    // Calcualte relative position
                    Vector3 offset = new(sizeOfImage.x * (count % 3) * sizeOfImage.x, 0, sizeOfImage.x * (int)(count / 3) * sizeOfImage.y);
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
                                        
                                        Vector3 rotationX = new(0,0,0);
                                        Vector3 torchOffset = new(0, 0, 0);
                                        // Rotate according to position X in the level
                                        if (x == 0) // Left side
                                        {
                                            rotationX = new(0.0f, 90.0f, 0.0f); 
                                            torchOffset = new(13, 0, 0);
                                        } 
                                        else if (x == level.width - 1) // Right side
                                        {
                                            rotationX = new(0.0f, -90.0f, 0.0f); 
                                            torchOffset = new(-13, 0, 0);
                                        } 
                                        obj.transform.Rotate(rotationX, Space.World);  

                                        Vector3 rotationZ = new(0, 0, 0);
                                        // Rotate according to position Y in the level
                                        if (z == 0) // Bottom side (no rotation needed)
                                        {
                                            rotationZ = new(0.0f, 0.0f, 0.0f); 
                                            torchOffset = new(0, 0, 13);
                                        }
                                        else if (z == level.height - 1) // Top side
                                        {
                                            rotationZ = new(0.0f, -180.0f, 0.0f);
                                            torchOffset = new(0, 0, -13);
                                        } 
                                        obj.transform.Rotate(rotationZ, Space.World);

                                        // Check if a torch has to be added, if so, apply the transformations
                                        if (colorPrefab.torch != null)
                                        {
                                            GameObject torch = Instantiate(colorPrefab.torch, position + new Vector3(0, 15, 0) + torchOffset, Quaternion.identity, transform);
                                            torch.transform.Rotate(rotationX, Space.World);
                                            torch.transform.Rotate(rotationZ, Space.World);
                                            
                                            // Set parent object
                                            torch.transform.parent = levelObject.transform;
                                        }

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
            count++;
        }        
    }
}
