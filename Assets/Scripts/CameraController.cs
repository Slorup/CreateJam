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
        if (Camera.main == null)
        {
            return;
        }

        float newX;
        if (player.transform.position.x < ((Camera.main.pixelWidth/100) / 2) || player.transform.position.x > 31-((Camera.main.pixelWidth/100) / 2))
        {
            newX = transform.position.x;
        }
        else
        {
            newX = player.transform.position.x + offset.x;
        }
        
        
        transform.position = new Vector3(newX, player.transform.position.y + offset.y, -10f);
    }
}
