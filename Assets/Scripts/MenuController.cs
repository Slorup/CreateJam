using UnityEngine;

public class MenuController : MonoBehaviour
{

    public Transform Corny;
    public Transform Bag;

    public PlayerController Player;
    public RemaController RemaController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuyBag()
    {
        if (Player.gold >= 15)
        {
            Player.AddBag();
            Player.gold -= 15;
            RemaController.BagsLeft -= 1;
            if (RemaController.BagsLeft <= 0)
            {
                Destroy(transform.Find("BagItem").gameObject);
            }
        }
    }

    public void BuyCorny()
    {
        if (Player.gold >= 5)
        {
            Player.gold -= 5;
            Player.HOLDBEFOREDIG *= 0.9f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
