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
                    // If the color corresponds to a wall make sure that the orientation is correct
                    // If the color corresponds to a light that has to be attached to a wall, consider its rotation

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
                                        // Rotate according to position X in the level
                                        if (x == 0) obj.transform.Rotate(0.0f, 90.0f, 0.0f, Space.World); // Left side
                                        else if (x == level.width - 1) obj.transform.Rotate(0.0f, -90.0f, 0.0f, Space.World); // Right side

                                        // Rotate according to position Y in the level
                                        if (z == 0) obj.transform.Rotate(0.0f, 0.0f, 0.0f, Space.World); // Bottom side (no rotation needed)
                                        else if (z == level.height - 1) obj.transform.Rotate(0.0f, -180.0f, 0.0f, Space.World); // Top side

                                        break;

                                    default:
                                        break;
                                }

                                // Check for special options
                                //colorPrefab.dropCoin
                                //colorPrefab.dropLife
                                //colorPrefab.movementPattern


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
