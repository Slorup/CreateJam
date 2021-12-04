using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public Transform Corny;
    public Transform Bag;

    public PlayerController Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuyBag()
    {
        if (Player.gold >= 5)
        {
            Player.AddBag();
        }
    }

    public void BuyCorny()
    {
        if (Player.gold >= 3)
        {
            Player.HOLDBEFOREDIG *= 0.9f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
