using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Public variables
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    // Private variables
    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1;
    bool aggressive = true;

//A.O. ADDED for RubyController
    private PlayerController rubyController;

//D.W. ADDED for Robot Smoke
    public ParticleSystem Smoke; 
    //Not totally necessary, but if I might revamp the system later on to allow you to input a seperate game object



    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;


//A.O. ADDED for RubyController
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");
        if (rubyControllerObject != null)

        {

            rubyController = rubyControllerObject.GetComponent<PlayerController>(); //and this line of code finds the rubyController and then stores it in a variable

            print ("Found the RubyConroller Script!");

        }

        if (rubyController == null)

        {

            print ("Cannot find GameController Script!");

        }

        //D.W. ADDED for Robot Smoke
         Smoke = GetComponent<ParticleSystem>();
        if (aggressive == true)
        {
            Smoke.Play(true);
        }

    }


    // Update is called every frame
    void Update()
    {
        if (!aggressive)
        {
            return;
        }

    }


    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        if (!aggressive)
        {
            return;
        }


        timer -= Time.deltaTime;


        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rigidbody2d.position;

        // D.W Vertical Bool and movement
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }


        rigidbody2d.MovePosition(position);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();


        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }




    public void Fix()
    {
        aggressive = false;
        GetComponent<Rigidbody2D>().simulated = false;
        animator.SetTrigger("Fixed");

        //D.W. ADDED for Robot Smoke
        if (aggressive == false)
        {
            Smoke = GetComponent<ParticleSystem>();
            Smoke.Stop(); 
        }

        //A.O. ADDED for score increase!
        if (rubyController != null)
        {
                rubyController.ChangeScore(+1);
        }
    }


}