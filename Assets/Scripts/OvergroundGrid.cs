using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OvergroundGrid : MonoBehaviour
{
    public int width = 32;
    public int height = 128;
    public int startHeight = 20;
    public float cloudChance = 1f;
    public float doubleCloudChance = 0.3f;
    public float cloud1Chance = 0.2f;
    public float cloud2Chance = 0.4f;
    public float cloud3Chance = 0.7f;
    public float cloud4Chance = 1.0f;
    public GameObject cloud1;
    public GameObject cloud2;
    public GameObject cloud3;
    public GameObject cloud4;
    public GameObject cloudTop;
    public GameObject pillar;
    public GameObject rema;
    public GameObject zeus;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(pillar, new Vector3(20,1), Quaternion.identity);
        //Instantiate(pillar, new Vector3(21,1), Quaternion.identity);
        //Instantiate(pillar, new Vector3(21,2), Quaternion.identity);

        Instantiate(cloud3, new Vector3(2, 2), Quaternion.identity);
        //Instantiate(rema, new Vector3(10, 1), Quaternion.identity);


        int firstRemaLocation = 8;// Mathf.RoundToInt(Random.Range(5f, width-6));
        Instantiate(cloud4, new Vector3(firstRemaLocation, startHeight - 5f),
            Quaternion.identity);
        Instantiate(rema, new Vector3(firstRemaLocation, startHeight - 4f),
            Quaternion.identity);

        int lastRema = 25;
        for (int y = startHeight; y < height; y++)
        {
            if (Random.value < cloudChance)
            {
                if (y - lastRema > 25)
                {
                    int xx = Mathf.RoundToInt(Random.Range(5f, width-5f));
                    Instantiate(rema, new Vector3(xx, y+1), Quaternion.identity);
                    Instantiate(cloud4, new Vector3(xx, y), Quaternion.identity);
                    y += 3;
                    lastRema = y;
                    continue;
                }
                
                
                int x = Mathf.RoundToInt(Random.Range(2f, (float)width));
                float cloudSpecifier = Random.value;
                if (cloudSpecifier < cloud1Chance)
                {
                    Instantiate(cloud1, new Vector3(x, y), Quaternion.identity);
                }
                else if (cloudSpecifier < cloud2Chance && x + 1 <= width)
                {
                    Instantiate(cloud2, new Vector3(x, y), Quaternion.identity);
                }
                else if (cloudSpecifier < cloud3Chance && x + 2 <= width)
                {
                    Instantiate(cloud3, new Vector3(x, y), Quaternion.identity);
                }
                else if (cloudSpecifier < cloud4Chance && x + 3 <= width)
                {
                    Instantiate(cloud4, new Vector3(x, y), Quaternion.identity);
                }
                

                y += 2;
            }
        }

        for (int x = 6; x < width; x += 4)
        {
            GameObject c = Instantiate(cloudTop, new Vector3(x-0.5f, height + 1), Quaternion.identity);
            if (Random.value < 0.5)
            {
                c.GetComponent<SpriteRenderer>().flipX = true;
                
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
