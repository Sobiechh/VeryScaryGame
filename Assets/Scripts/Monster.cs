using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public GameObject player;
    public AudioClip[] footsounds;
    public Transform eyes;
    public AudioSource growl;
    public float walkSpeed;
    public float runSpeed;

    private NavMeshAgent nav;
    private AudioSource sound;
    private Animator anim;
    private string state = "idle";
    private float wait = 0.0f;
    private bool highAlert = false;
    private float alertness = 20f;

    // Start is called before the first frame update
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        sound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public void footstep(int _num)
    {
        sound.clip = footsounds[_num];
        sound.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        anim.SetFloat("velocity", nav.velocity.magnitude);
        if (state == "idle")
        {
            print("idle");
            //pick a random place to walk to
            Vector3 randomPos = Random.insideUnitSphere * alertness;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);

            //go near the player
            if (highAlert)
            {
                NavMesh.SamplePosition(player.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
                alertness += 5f;

                if (alertness > 20f)
                {
                    highAlert = false;
                    nav.speed = walkSpeed;
                    anim.speed = walkSpeed;
                }
            }
            nav.SetDestination(navHit.position);
            print(navHit.position);
            nav.speed = walkSpeed;
            anim.speed = walkSpeed;
            state = "walk";
        }
        if (state == "walk")
        {
            print("walk");
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                state = "search";
                wait = 5f;
            }
        }
        if (state == "search")
        {
            print("search");
            if (wait < 0f)
            {
                wait -= Time.deltaTime;
                transform.Rotate(0f, 120f * Time.deltaTime, 0f);
            }
            else
            {
                state = "idle";
            }
        }
        if (state == "chase")
        {
            print("chase");
            nav.destination = player.transform.position;

            //lose sight of player
            float distance = Vector3.Distance(transform.position, player.GetComponent<CharacterController>().bounds.center);
            //print(distance);
            if (distance > 15f)
            {
                state = "hunt";
            }
            else if (nav.remainingDistance <= nav.stoppingDistance + 1f && !nav.pathPending)
            {
                player.GetComponent<PlayerMovement>().TakeDamage(5);
            }
        }

        if (state == "hunt")
        {
            print("hunt");
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                state = "search";
                wait = 5f;
                highAlert = true;
                alertness = 5f;
                checkSight();
            }
        }
        //nav.SetDestination(player.transform.position);
    }

    public void checkSight()
    {
        RaycastHit rayHit;
        if (Physics.Linecast(eyes.transform.position, player.GetComponent<CharacterController>().bounds.center, out rayHit))
        {
            if (rayHit.collider.CompareTag("Player"))
            {
                print("zaraz szuka");
                if (state != "kill")
                {
                    state = "chase";
                    nav.speed = runSpeed;
                    anim.speed = runSpeed;
                    growl.pitch = 1.2f;
                    growl.Play();
                }
            }
        }
    }
}