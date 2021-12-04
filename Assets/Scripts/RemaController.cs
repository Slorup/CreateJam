using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemaController : MonoBehaviour
{
    public int BagsLeft = 1;
    public PlayerController player;
    
    // Start is called before the first frame update

    public void OpenMenu()
    {
        transform.Find("Canvas").Find("Menu").GetComponent<MenuController>().Player = player;
        transform.Find("Canvas").Find("Menu").gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        transform.Find("Canvas").Find("Menu").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
