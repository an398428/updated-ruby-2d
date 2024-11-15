using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;


    // Variables related to the health system
    public int maxHealth = 3;
    int currentHealth;
    public int health { get { return currentHealth; } }
    

//A.O. ADDED Variables related to the health system!
    public GameObject psHealthDOWNPrefab;
    public GameObject psHealthUPPrefab;


    // Variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;


    // Variables related to animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);


    // Variables related to projectiles
    public GameObject projectilePrefab;
    public InputAction launchAction;

//A.O. ADDED Variables related to projectile damage
    public int score;

    //Talk Actions/Interact
    public InputAction talkAction;

    //Audio Source Collection
    AudioSource audioSource;

//A.O. ADDED for restart!
    public GameObject gameOverText;
    bool gameOver;


    void Start()
    {
        MoveAction.Enable();
        launchAction.Enable();
        launchAction.performed += Launch;

        talkAction.Enable();
        talkAction.performed += FindFriend;


        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();


        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
                isInvincible = false;
        }
    //A.O. ADDED for activating game over 
        if (currentHealth == 0)
        {
            gameOverText.SetActive(true);
        }

        if (score == 3)
        {
            gameOverText.SetActive(true);
        }
    }

   void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            isInvincible = true;
            damageCooldown = timeInvincible;
//A.O. ADDED for damage burst effect!
            GameObject psHealthDOWN = Instantiate(psHealthDOWNPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            animator.SetTrigger("Hit");
        }

//A.O. ADDED for healthup burst effect!
        if (amount > 0)
        {
            GameObject psHealthUP = Instantiate(psHealthUPPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }        
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }


//A.O. ADDED for tracking Ruby's score!
    public void ChangeScore(int scoreAmount)
    {
        
        score = score + scoreAmount;
        
    }



   


    void Launch(InputAction.CallbackContext context)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);


        animator.SetTrigger("Launch");
    }



    //Lets Interact
    void FindFriend(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));


        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }
        }
    }

    //Audio Sources
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}


