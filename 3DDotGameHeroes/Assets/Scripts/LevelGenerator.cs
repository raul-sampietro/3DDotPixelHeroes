using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D[] levels;
    public ColorToPrefab[] colorMappings;

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

                    if (pixelColor.a >= 0) // Pixel not transparent
                    {
                        foreach (ColorToPrefab colorPrefab in colorMappings)
                        {
                            //Debug.Log("PixelColor: " + pixelColor);
                            //Debug.Log("colorPrefab: " + colorPrefab.color);

                            if (colorPrefab.color.Equals(pixelColor))
                            {
                                Vector3 offset = new(16 * (count%3) * sizeOfImage.x, 0, 16 * (int)(count/3) * sizeOfImage.y);
                                Vector3 position = new(x * 16, 8, z * 16);
                                position += offset;
                                GameObject obj = Instantiate(colorPrefab.prefab, position, Quaternion.identity, transform);
                                obj.transform.parent = levelObject.transform;
                            }
                        }
                    }
                }
            }
            count++;
        }        
    }
}
