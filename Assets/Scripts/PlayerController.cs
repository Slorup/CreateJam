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
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public static float HORIZONTALFORCE = 4;
    public static float JUMPFORCE = 145;
    public static float JUMPTIMEDELAY = 0.1f; //Seconds
    public static float MAXHORIZONTALVELOCITY = 5;
    public static float HOLDBEFOREDIG = 0.5f; //Seconds

    public Vector3 CurrentVelocity { get; set; } = Vector3.zero;

    private List<GameObject> blocksUnder = new List<GameObject>();
    private List<GameObject> blocksRight = new List<GameObject>();
    private List<GameObject> blocksLeft = new List<GameObject>();

    //public bool IsGrounded { get; set; } = false;

    private float lastJumpTime;
    private float timeStandStill;
    private float holdDownTime;
    private float holdLeftTime;
    private float holdRightTime;

    private int marble;
    private int gold;

    public Text goldText;
    public Text marbleText;

    // Start is called before the first frame update
    void Start()
    {
        lastJumpTime = Time.time;
        timeStandStill = 0;
        holdDownTime = 0;
        holdLeftTime = 0;
        holdRightTime = 0;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();

        goldText.text = gold.ToString();
        marbleText.text = marble.ToString();
    }

    private void HandlePlayerMovement()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        bool moveRight = Input.GetKey(KeyCode.RightArrow);
        bool moveLeft = Input.GetKey(KeyCode.LeftArrow);
        bool moveUp = Input.GetKey(KeyCode.UpArrow);
        bool moveDown = Input.GetKey(KeyCode.DownArrow);

        //Horizontal movement
        if (moveRight && !moveLeft)
        {
            body.AddForce(new Vector2(HORIZONTALFORCE, 0));
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (!moveRight && moveLeft)
        {
            body.AddForce(new Vector2(-HORIZONTALFORCE, 0));
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        //Jump
        if (moveUp && !moveDown && IsGrounded() && (Time.time - lastJumpTime) > JUMPTIMEDELAY)
        {
            body.AddForce(new Vector2(0, JUMPFORCE));
            lastJumpTime = Time.time;
        }

        //Cap velocity
        if (body.velocity.x > MAXHORIZONTALVELOCITY)
            body.velocity = new Vector2(MAXHORIZONTALVELOCITY, body.velocity.y);
        if (body.velocity.x < -MAXHORIZONTALVELOCITY)
            body.velocity = new Vector2(-MAXHORIZONTALVELOCITY, body.velocity.y);

        //Stand still timer
        if (Math.Abs(body.velocity.x) < 0.001 && Math.Abs(body.velocity.y) < 0.001)
            timeStandStill += Time.deltaTime;
        else 
            timeStandStill = 0;

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
        
        Debug.Log(timeStandStill);
        
        //Debug.Log("Under: " + blocksUnder.Count + " Left: " + blocksLeft.Count + " Right: " + blocksRight.Count);
    }
    
    private bool IsGrounded()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        RaycastHit2D raycastHit2D = Physics2D.Raycast(col.bounds.center, Vector2.down, 0.4f);
        //Debug.DrawRay(col.bounds.center , Vector2.down, Color.red);
        Debug.Log(raycastHit2D.collider.name);
        return raycastHit2D;
    }

}
