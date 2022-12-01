using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D[] levels;

    public ColorToPrefab[] colorMappings;
    // Start is called before the first frame update
    void Start()
    {
        int count = 1;
        foreach (Texture2D level in levels)
        {
            GameObject levelObject = new();
            levelObject.name = "Room" + count;
            for (int x = 0; x < level.width; ++x)
            {
                for (int z = 0; z < level.height; ++z)
                {
                    Color pixelColor = level.GetPixel(x, z);
                    // If the color corresponds to a wall make sure that the orientation is correct
                    // If the color corresponds to a light that has to be attached to a wall, consider its rotation

                    if (pixelColor.a != 0) // Pixel not transparent
                    {
                        foreach (ColorToPrefab colorPrefab in colorMappings)
                        {
                            if (colorPrefab.color.Equals(pixelColor))
                            {
                                Vector3 position = new(x*16, 8, z*16);
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
