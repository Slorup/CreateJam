using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemaController : MonoBehaviour
{
    public GameObject Menu;
    private int BagsLeft = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenMenu()
    {
        transform.Find("Menu").gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        transform.Find("Menu").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
