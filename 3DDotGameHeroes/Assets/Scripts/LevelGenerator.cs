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
                    if (pixelColor.a != 0) // Pixel not transparent
                    {
                        foreach (ColorToPrefab colorPrefab in colorMappings)
                        {
                            if (colorPrefab.color.Equals(pixelColor))
                            {
                                Vector3 position = new(x, count, z);
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
