using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    NewControls controls;

    [SerializeField]
    private float moveSpeed;
    private Vector2 dir;

    [SerializeField]
    private Rigidbody2D rb;
    private float activeMoveSpeed;
    [SerializeField]
    private float dashSpeed = 25f;
    [SerializeField]
    private float dashlength = 0.2f;
    [SerializeField]
    private float dashCool = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    private bool isDash = false;

    public TrailRenderer tr;
    Vector2 move;
    int[] roll = new int[2] { 4, 4 };

    [SerializeField] private GameObject Player;
    private SpriteRenderer FaceUp;
    [SerializeField] private Sprite[] DiceFaces;

    [SerializeField] private GameObject range;
    [SerializeField] private float rangeAdjust;

    /*[SerializeField] Animator animator;*/

    private void Awake()
    {
        if(controls == null)
        {
            controls = new NewControls();
            controls.Enable();
            controls.Gameplay.Movement.performed += ctrls =>
            {
                dir = ctrls.ReadValue<Vector2>();
                _movement(dir);
            };

            controls.Gameplay.Dash.performed += ctrls => Dash(dir);
        }
    }
    private void Start()
    {
        activeMoveSpeed = moveSpeed;
        range.transform.localScale = new Vector3(rangeAdjust * roll[1], rangeAdjust * roll[1], rangeAdjust * roll[1]);
        FaceUp = Player.GetComponent<SpriteRenderer>();
        /*DiceFaces = Resources.LoadAll<Sprite>("DiceFaces/");*/
        FaceUp.sprite = DiceFaces[3];
    }
    /*void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        move.Normalize();

        rb.velocity = move * activeMoveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            *//*animator.SetBool("IsRoll", true);*//*


            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                dieRoll();
                tr.emitting = true;
                activeMoveSpeed = dashSpeed;
                dashCounter = dashlength * roll[0];

                isDash = true;
            }
            *//*animator.SetBool("IsRoll", false);*//*
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCool;
                tr.emitting = false;
                isDash = false;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }*/

    void Dash(Vector2 _dash)
    {
        rb.velocity = new Vector2(rb.velocity.x * dashSpeed, rb.velocity.y * dashSpeed);
    }

    void _movement(Vector2 direct)
    {
        rb.velocity = direct * moveSpeed;
    }
    private void dieRoll()
    {

        roll[0] = roll[1];
        roll[1] = Random.Range(1, 7);
        /*Debug.Log(roll[1]);*/
        FaceUp.sprite = DiceFaces[roll[1] - 1];
        range.transform.localScale = new Vector3(rangeAdjust * roll[1], rangeAdjust * roll[1], rangeAdjust * roll[1]);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDash)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                /*if (!enemyKilled)
                    return;*/

                killStreak += 1;
                Debug.Log(killStreak);
                StopCoroutine("ResetStreak");
                StartCoroutine("ResetStreak");
            }
        }
        else
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
                SceneManager.LoadScene("Game Over");
            }
        }


    }

    public int killStreak = 0;
    int resetTime = 4;

    IEnumerator ResetStreak()
    {
        yield return new WaitForSeconds(resetTime);
        killStreak = 0;
    }
}
