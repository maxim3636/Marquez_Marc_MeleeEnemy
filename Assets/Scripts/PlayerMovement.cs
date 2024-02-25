using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    int jumpCount = 0;
    public int jumplimit = 2;
    private BoxCollider2D boxCollider;
    private float axisH;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        if (body == null)
        {
            Debug.LogError(" Can't find RB of the player"); 
        }
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError(" Can't find animator of the player"); 
        }
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        axisH = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
        if (axisH > 0)
        {
            transform.localScale = Vector3.one;
        }else if (axisH < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (axisH != 0)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumplimit)
        {
            jump();
        }
        
    }

    private void jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetBool("jump", true);
        jumpCount++;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            
            anim.SetBool("jump", false);
            jumpCount = 0;
        }
    }
    
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return isGrounded() && !onWall() &&  axisH== 0;
    }
}
