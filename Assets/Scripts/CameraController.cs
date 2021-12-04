using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 candPos = player.transform.position + offset;
        float nextX = transform.position.x;
        float nextY = transform.position.y;
        
        if (GetComponent<Camera>().rect.xMin >= candPos.x && GetComponent<Camera>().rect.xMax <= candPos.x)
        {
            nextX = candPos.x;
        }
        
        if (GetComponent<Camera>().rect.yMin >= candPos.y && GetComponent<Camera>().rect.yMax <= candPos.y)
        {
            nextX = candPos.y;
        }

        transform.position = new Vector3(nextX, nextY) + offset;
    }
}
