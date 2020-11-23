using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower;
    public float jumpPower;
    public float dashPower;

    private bool LcanDash = false;
    private bool RcanDash = false;
    private float CheckDashtime = 0.4f;
    private float dashTime;
    public float startDashTime;
    private int direction;


    Rigidbody2D rb;

    public float horizontalMove;
    Vector3 movement;
    public bool isJump = false;
    public bool isPush = false;
    public bool isPushing = false;

    private bool isGround = true;

    public GameManager gmaeMgr;
    public GameObject scanObject;

    Animator animator;

    // DontDestroyOnLoad 중복 생성 확인
    public static Player Instance;

    private  void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashTime = startDashTime;
    }

    // Update is called once per frame
    // 그래픽과 입력은 Update() 부분에서 실행
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && scanObject != null)
        {
            gmaeMgr.Action(scanObject);
        }

        // 카메라 밖으로 못나가게 고정
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        AnimationUpdate();
    }

    void AnimationUpdate()
    {
      //if (isJump)
            animator.SetTrigger("isJump");

       // if (isGround && !isJump)
            animator.SetBool("isJump", false);

        if (horizontalMove == 0)
        {
            animator.SetBool("isWalk", false);
            animator.SetFloat("MoveSpeed", 0);
        }
        
        else
        {
            if (isPush)
                animator.SetBool("isPush", true);

            if (isPush == true && isPushing == true)
            {
                animator.SetFloat("PushCheck", 2.0f);
                animator.SetBool("isPushing", true);               
            }

            else
            {
                animator.SetBool("isWalk", true);
                animator.SetFloat("MoveSpeed", movePower);
            }
        }

    }

    // 물리엔진 사용은 FixedUpdate() 부분에서 실행
    void FixedUpdate()
    {
        Move();
        Jump();
        Dash();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(horizontalMove < 0)
        {
            moveVelocity = Vector3.left;

            // 이미지 플립
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        else if(horizontalMove > 0)
        {
            moveVelocity = Vector3.right;

            // 이미지 플립
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJump)
            return;

        rb.velocity = Vector2.zero;

        //if (isGround == true)
        //{
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

            isGround = false;
        //}      
        isJump = false;
    }
    void Dash()
    {
        if(direction ==0)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
                LcanDash = true;
            if (Input.GetKeyUp(KeyCode.RightArrow))
                RcanDash = true;

            if(LcanDash ==true)
            {
                CheckDashtime -= Time.deltaTime;
                if(CheckDashtime <= 0)
                {
                    LcanDash = false;
                    CheckDashtime = 0.4f;
                }
            }
            if (RcanDash == true)
            {
                CheckDashtime -= Time.deltaTime;
                if (CheckDashtime <= 0)
                {
                    RcanDash = false;
                    CheckDashtime = 0.4f;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow) && LcanDash == true)
                direction = 1;
            if (Input.GetKey(KeyCode.RightArrow) && RcanDash == true)
                direction = 2;
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;                
                if (direction == 1)
                {
                    animator.Play("Dash");
                    rb.velocity = Vector2.left * dashPower;
                    LcanDash = false;
                }
                else if (direction == 2)
                {
                    animator.Play("Dash");
                    rb.velocity = Vector2.right * dashPower;
                    RcanDash = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            Debug.Log("Talk Npc");
            scanObject = collision.gameObject;
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            Debug.Log("Talk Npc");
            scanObject = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Obstacle")
        {
            isPush = true;
            isPushing = true;
        }

        //isGround = true;
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Obstacle")
        {
            animator.SetFloat("PushCheck", 0);
            isPush = false;
            isPushing = false;
            animator.SetBool("isPush", isPush);
            animator.SetBool("isPushing", isPushing);

        }
    }
}
