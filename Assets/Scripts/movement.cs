using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Rigidbody2D rb;
    private float activeMoveSpeed;
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashlength = 0.2f;
    [SerializeField] private float dashCool = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    private bool isDash=false;

    public TrailRenderer tr;
    Vector2 move;
    int[] roll = new int[2] {4,4};

    [SerializeField] private GameObject Player;
    private SpriteRenderer FaceUp;
    [SerializeField] private Sprite[] DiceFaces;

    [SerializeField] private GameObject range;
    [SerializeField] private float rangeAdjust;

    /*[SerializeField] Animator animator;*/
    private void Start()
    {
        activeMoveSpeed = moveSpeed;
        range.transform.localScale = new Vector3(rangeAdjust * roll[1], rangeAdjust * roll[1], rangeAdjust * roll[1]);
        FaceUp = Player.GetComponent<SpriteRenderer>();
        /*DiceFaces = Resources.LoadAll<Sprite>("DiceFaces/");*/
        FaceUp.sprite = DiceFaces[3];
    }
    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        move.Normalize();

        rb.velocity = move * activeMoveSpeed;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            /*animator.SetBool("IsRoll", true);*/
            
            
            if (dashCoolCounter <=0 && dashCounter <=0)
            {
                dieRoll();
                tr.emitting = true;
                activeMoveSpeed = dashSpeed;
                dashCounter = dashlength * roll[0];
                
                isDash = true;
            }
            /*animator.SetBool("IsRoll", false);*/
        }

        if(dashCounter >0)
        {
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCool;
                tr.emitting = false;
                isDash=false;
            }
        }

        if(dashCoolCounter >0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
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
        if(isDash)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Destroy(collision.gameObject);
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
}
