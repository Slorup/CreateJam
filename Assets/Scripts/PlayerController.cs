using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

class BagO2
{
    public Transform bag;
    public float o2;

    public BagO2(Transform _bag, float _o2)
    {
        bag = _bag;
        o2 = _o2;
    }
}

public class PlayerController : MonoBehaviour
{
    public static float HORIZONTALFORCE = 4;
    public static float JUMPFORCE = 190;
    public static float JUMPTIMEDELAY = 0.1f; //Seconds
    public static float MAXHORIZONTALVELOCITY = 5;
    public float HOLDBEFOREDIG = 0.5f; //Seconds
    public static float PILLARPLACEMENTCOOLDOWN = 0.2f; //Seconds
    public static int PILLARCOST = 1; //MARBLE

    public Vector3 CurrentVelocity { get; set; } = Vector3.zero;

    private List<GameObject> blocksUnder = new List<GameObject>();
    private List<GameObject> blocksRight = new List<GameObject>();
    private List<GameObject> blocksLeft = new List<GameObject>();

    //public bool IsGrounded { get; set; } = false;

    enum HorizontalMovement
    {
        Left,
        Right
    }

    private HorizontalMovement lastHorizontalMovement = HorizontalMovement.Right; 
    private float lastJumpTime;
    private float lastPillarPlacement;
    private float timeStandStill;
    private float holdDownTime;
    private float holdLeftTime;
    private float holdRightTime;

    public int marble;
    public int gold;

    public Sprite idleSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite downSprite;

    public GameObject pillar;
    private GameObject pillars;

    public Text goldText;
    public Text marbleText;

    public Image HurtImage;
    
    public GameObject bagsOBJ;
    public GameObject bagPrefab;
    private List<BagO2> bags = new List<BagO2>();

    public float oxygenDecay;

    // Start is called before the first frame update
    void Start()
    {
        AddBag();
        
        gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
        lastJumpTime = Time.time;
        timeStandStill = 0;
        holdDownTime = 0;
        holdLeftTime = 0;
        holdRightTime = 0;
        pillars = Instantiate(new GameObject("Pillars"));
        Instantiate(pillar, new Vector3(transform.position.x + 10, transform.position.y, 0), Quaternion.identity, pillars.transform);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void AddBag()
    {
        var b = Instantiate(bagPrefab, bagsOBJ.transform);
        b.transform.localPosition = bags.Count * Vector2.left * 100f;
        bags.Add(new BagO2(b.transform.Find("Square"), 0.0f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandlePlayerMovement();

        goldText.text = gold.ToString();
        marbleText.text = marble.ToString();


        var o2loss = oxygenDecay * Mathf.Abs(transform.position.y) * Time.deltaTime;
        foreach (var b in bags)
        {
            b.o2 -= Mathf.Min(o2loss, b.o2);
            o2loss -= Mathf.Min(o2loss, b.o2);

            b.bag.localPosition = new Vector3(b.bag.localPosition.x, -(1.0f - b.o2) * 0.9f - 0.42f, 1.0f);

            if (Mathf.Abs(transform.position.y) < 10f)
                b.o2 = 1.0f;
        }

        if (o2loss > 0f)
        {
            // TODO: DIE
        }

        if (bags.Last().o2 < 0.5f)
        {
            var c = HurtImage.color;
            HurtImage.color = new Color(c.r, c.g, c.b, 0.5f - bags.Last().o2);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && IsGrounded() && (Time.time - lastPillarPlacement) > PILLARPLACEMENTCOOLDOWN)
            TryPlacePillar();

        if (Input.GetKeyDown(KeyCode.T))
            TeleportToHighestPillar();
    }

    private void TeleportToHighestPillar()
    {
        if (pillars.transform.childCount == 0)
        {
            transform.position = new Vector3(1, 1, 0);
            return;
        }
        
        Transform highestPillar = pillars.transform.GetChild(0);
        
        for (int i = 1; i < pillars.transform.childCount; i++)
        {
            Transform current = pillars.transform.GetChild(i);
            if (current.position.y > highestPillar.position.y)
                highestPillar = current;
        }

        transform.position = highestPillar.position + Vector3.up;
    }

    private void TryPlacePillar()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (marble >= PILLARCOST)
        {
            float pillarX = body.position.x;
            float pillarY = body.position.y + 0.15f;
            //float pillarX = body.position.x + 0.5f;
            //float pillarY = body.position.y + 0.1f;
            //if (lastHorizontalMovement == HorizontalMovement.Left) pillarX -= 1;

            Instantiate(pillar, new Vector3(pillarX, pillarY, 0), Quaternion.identity, pillars.transform);
            lastPillarPlacement = Time.time;
            marble -= PILLARCOST;
        }
    }

    private void HandlePlayerMovement()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        bool moveRight = Input.GetKey(KeyCode.RightArrow);
        bool moveLeft = Input.GetKey(KeyCode.LeftArrow);
        bool moveUp = Input.GetKey(KeyCode.UpArrow);
        bool moveDown = Input.GetKey(KeyCode.DownArrow);
        
        //Stand still timer
        if (Math.Abs(body.velocity.x) < 0.001 && Math.Abs(body.velocity.y) < 0.001)
            timeStandStill += Time.deltaTime;
        else 
            timeStandStill = 0;

        if (!moveRight && !moveLeft)
            body.velocity = new Vector2(0, body.velocity.y);
        
        //Horizontal movement
        if (moveRight && !moveLeft)
        {
            body.velocity = new Vector2(HORIZONTALFORCE, body.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().sprite = rightSprite;
            lastHorizontalMovement = HorizontalMovement.Right;
        }
        if (!moveRight && moveLeft)
        {
            body.velocity = new Vector2(-HORIZONTALFORCE, body.velocity.y);
            gameObject.GetComponent<SpriteRenderer>().sprite = leftSprite;
            lastHorizontalMovement = HorizontalMovement.Left;

        }
        
        if(!moveDown && !moveLeft && !moveRight && !moveDown)
            gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
        
        if(IsGrounded() && moveDown && !moveRight && !moveLeft && !moveUp)
            gameObject.GetComponent<SpriteRenderer>().sprite = downSprite;
        
        // Jump through cloud
        Physics2D.IgnoreLayerCollision(2, 6, body.velocity.y > 0);

        //Jump
        if (moveUp && !moveDown && IsGrounded() && (Time.time - lastJumpTime) > JUMPTIMEDELAY && body.velocity.y <= 0.1)
        {
            body.AddForce(new Vector2(0, JUMPFORCE));
            lastJumpTime = Time.time;
        }

        //Cap velocity
        if (body.velocity.x > MAXHORIZONTALVELOCITY)
            body.velocity = new Vector2(MAXHORIZONTALVELOCITY, body.velocity.y);
        if (body.velocity.x < -MAXHORIZONTALVELOCITY)
            body.velocity = new Vector2(-MAXHORIZONTALVELOCITY, body.velocity.y);

        if (moveDown && !moveUp)
            holdDownTime += Time.deltaTime;
        else
            holdDownTime = 0;
        
        if (moveLeft && !moveRight)
            holdLeftTime += Time.deltaTime;
        else
            holdLeftTime = 0;
        
        if (moveRight && !moveLeft)
            holdRightTime += Time.deltaTime;
        else
            holdRightTime = 0;
        
        
        //Dig
        if (timeStandStill > HOLDBEFOREDIG)
        {
            CircleCollider2D col = GetComponent<CircleCollider2D>();
            Vector3 dir = Vector3.zero;
            if (holdDownTime > HOLDBEFOREDIG) {
                dir = Vector3.down;
            } else if (holdRightTime > HOLDBEFOREDIG) {
                dir = Vector3.right;
            } else if (holdLeftTime > HOLDBEFOREDIG) {
                dir = Vector3.left;
            }

            if (dir != Vector3.zero)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(col.bounds.center, dir, 0.4f);
                Block blockInfo;
                if (raycastHit2D && raycastHit2D.collider.gameObject.TryGetComponent<Block>(out blockInfo))
                {
                    if (blockInfo.blockType == BlockType.Gold)
                        gold += Mathf.RoundToInt(Random.Range(4, 13));
                    if (blockInfo.blockType == BlockType.Marble)
                        marble += 2;
                    
                    Destroy(raycastHit2D.collider.gameObject);
                }
            }
        }
        
        //Debug.Log("Under: " + blocksUnder.Count + " Left: " + blocksLeft.Count + " Right: " + blocksRight.Count);
    }
    
    private bool IsGrounded()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        RaycastHit2D raycastHit2D = Physics2D.Raycast(col.bounds.center, Vector2.down, 0.4f);
        //Debug.DrawRay(col.bounds.center , Vector2.down, Color.red);
        //Debug.Log(raycastHit2D.collider.name);
        return raycastHit2D;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RemaController rc;
        if (other.TryGetComponent<RemaController>(out rc))
        {
            rc.OpenMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        RemaController rc;
        if (other.TryGetComponent<RemaController>(out rc))
        {
            rc.CloseMenu();
        }
    }
}
