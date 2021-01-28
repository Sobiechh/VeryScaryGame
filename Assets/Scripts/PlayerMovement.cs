using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;

    public GameObject MonsterToBeSpawned;

    public AudioClip[] footsounds;
    private AudioSource sound;

    public HealthBarScript healthBar;
    public float jumpHeight = 3f;
    public float speed = 1.3f;
    public float gravity = -6f;
    public float turnSmoothTime = 0.1f;
    public float groundDistance = 0.4f;
    private float turnSmoothVelocity;
    private bool isGrounded;

    private Vector3 grav;
    private Animator animator;

    private float speedRun = 3.3f;
    private int playerHP = 100;
    private int currentHealth;

    private GameObject infoFirstHelmetObject;
    private GameObject infoFirstMedkitObject;
    private GameObject infosMainObject;

    private GameObject gameOverObject;

    public void footstep(int _num)
    {
        sound.clip = footsounds[_num];
        sound.Play();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public void AddHealth(int hp)
    {
        currentHealth += hp;

        healthBar.SetHealth(currentHealth);
    }

    private void Start()
    {
        //health
        currentHealth = 80;
        healthBar.SetHealth(currentHealth);
        //health bar
        healthBar.SetMaxHealth(playerHP);

        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();

        //Infos Main disactive
        infosMainObject = GameObject.FindGameObjectWithTag("InfosMainTag");
        infoFirstHelmetObject = GameObject.FindGameObjectWithTag("InfoFirstHelmet");
        infoFirstMedkitObject = GameObject.FindGameObjectWithTag("InfoFirstMedkit");

        gameOverObject = GameObject.FindGameObjectWithTag("GameOverTag");

        gameOverObject.SetActive(false);
        infosMainObject.SetActive(false);
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && grav.y < 0)
        {
            grav.y = -0.5f;
        }

        if (currentHealth <= 0){
            gameOverObject.SetActive(true);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

        grav.y += gravity * Time.deltaTime;
        controller.Move(grav * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetFloat("Gravitation", 1);
            grav.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            // TakeDamage(200);
        }
        animator.SetFloat("Gravitation", grav.y);

        if (direction.magnitude >= 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
            {
                animator.SetBool("isRunning", true);
                speed = speedRun;
            }
            else
            {
                animator.SetBool("isRunning", false);
                speed = 1.3f;
            }

            animator.SetBool("isWalking", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            grav.y += gravity * Time.deltaTime;
            controller.Move(grav * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //first Helmet touch
        if (other.gameObject.tag == "FirstHelmetPickUp")
        {
            other.gameObject.SetActive(false);

            infosMainObject.SetActive(true);
            infoFirstHelmetObject.SetActive(true);
            infoFirstMedkitObject.SetActive(false);
        }

        if (other.gameObject.tag == "DeadArea")
        {
            TakeDamage(100);
        }

        //first Medkit touch
        if (other.gameObject.tag == "FirstKidPickUp")
        {
            AddHealth(20);
            other.gameObject.SetActive(false);

            infosMainObject.SetActive(true);
            infoFirstHelmetObject.SetActive(false);
            infoFirstMedkitObject.SetActive(true);
        }

        if (other.gameObject.tag == "NormalHelmet")
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.name == "eyes")
        {
            other.transform.parent.GetComponent<Monster>().checkSight();
        }
    }
}