using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationScript : MonoBehaviour
{
    public static Vector3 myforward = new Vector3(100f, 250f, -250f);
    int i = 0;
    const float min = 0f;
    const float max = 100f;
    Vector3 cubeSize;
    Vector3 offset;

    void Start()
    {
        cubeSize = gameObject.transform.localScale;
        offset = new Vector3(-200f, 250f, -250f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
         
        if (i > 1000)
        {
            float xPos = GetRandomRangeInCube() * cubeSize.x * 5;
            float yPos = GetRandomRangeInCube() * cubeSize.y * 2;
            float zPos = GetRandomRangeInCube() * cubeSize.z * 6;
            Vector3 position = new Vector3(xPos, yPos, zPos) + offset;
            myforward = position;

            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //sphere.transform.position = myforward;
            i = 0;
        }
        i++;
    }

    float GetRandomRangeInCube()
    {
        float randomRange = Random.Range(min, max);
        return randomRange;
    }
}
