using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] GameObject ragdoll;
    [SerializeField] float damageAmount;
    [SerializeField] AudioSource[] attackingSounds;
    enum STATE { IDLE, WANDER, ATTACK, CHASE, DEAD };
    STATE state = STATE.IDLE;

    Animator anim;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        anim.SetBool("isWalking", true);
    }

    void TurnOffTriggers() {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isDead", false);
    }

    public void DamagePlayer() {
        if (target != null)
        {
            target.GetComponent<FPController>().TakeHit(damageAmount);
            PlayAttackingSound();
        }
    }

    void PlayAttackingSound() {
        AudioSource audioSource = new AudioSource();
        int n = Random.Range(1, attackingSounds.Length);
        audioSource = attackingSounds[n];
        audioSource.Play();
        attackingSounds[n] = attackingSounds[0];
        attackingSounds[0] = audioSource;
    }

    float DistanceToPlayer() {
        if (GameStats.gameOver) return Mathf.Infinity;
        return Vector3.Distance(target.transform.position, transform.position);
    }
    bool CanSeePlayer() {
        if (DistanceToPlayer() < 10) {
            return true;
        }
        return false;
    }

    bool ForgotPlayer() {
        if (DistanceToPlayer() > 20)
            return true;
         return false;
    }

    public void OnZombieDead() {
        if (Random.Range(0, 10) < 5)
        {
            GameObject rd = Instantiate(ragdoll, transform.position, transform.rotation);
            rd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10000);
            Destroy(gameObject);
            return;
        }
        else
        {
            TurnOffTriggers();
            anim.SetBool("isDead", true);
            state = STATE.DEAD;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P)) {
            
            if (Random.Range(0,10) < 5)
            {
                GameObject rd = Instantiate(ragdoll, transform.position, transform.rotation);
                rd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10000);
                Destroy(gameObject);
                return;
            }
            else {
                TurnOffTriggers();
                anim.SetBool("isDead", true);
                state = STATE.DEAD;
            }
        }*/
        if (target == null && GameStats.gameOver == false) {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        switch (state) {
            case STATE.IDLE:
                TurnOffTriggers();
                if (CanSeePlayer())
                    state = STATE.CHASE;
                else if(Random.Range(0,5000) < 5)
                    state = STATE.WANDER;
                break;
            case STATE.WANDER:
                if (!agent.hasPath)
                {
                    float newX = transform.position.x + Random.Range(-5f, 5f);
                    float newZ = transform.position.z + Random.Range(-5f, 5f);
                    float newY = Terrain.activeTerrain.SampleHeight(new Vector3(newX, 0, newZ));
                    Vector3 dest = new Vector3(newX, newY, newZ);
                    agent.speed = walkingSpeed;
                    agent.SetDestination(dest);
                    agent.stoppingDistance = 0;
                    TurnOffTriggers();
                    anim.SetBool("isWalking", true);
                }
                if (CanSeePlayer())
                    state = STATE.CHASE;
                if (Random.Range(0, 5000) < 5) {
                    state = STATE.IDLE;
                    TurnOffTriggers();
                    agent.ResetPath();
                }
                break;
            case STATE.ATTACK:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    state = STATE.WANDER;
                    return;
                }
                TurnOffTriggers();
                anim.SetBool("isAttacking", true);
                transform.LookAt(target.transform.position);
                if (DistanceToPlayer() > agent.stoppingDistance + 2)
                    state = STATE.CHASE;
                break;
            case STATE.CHASE:
                if (GameStats.gameOver)
                {
                    TurnOffTriggers();
                    state = STATE.WANDER;
                    return;
                }
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 5;
                agent.speed = runningSpeed;
                TurnOffTriggers();
                anim.SetBool("isRunning", true);

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) {
                    state = STATE.ATTACK;
                }

                if (ForgotPlayer()) {
                    state = STATE.WANDER;
                    agent.ResetPath();
                }

                break;
            case STATE.DEAD:
                Destroy(agent);
                GetComponent<Sink>().StartSink();
                break;
        }

    }
}


/*agent.SetDestination(target.transform.position);

if (agent.remainingDistance > agent.stoppingDistance)
{
    anim.SetBool("isWalking", true);
    anim.SetBool("isAttacking", false);
}
else
{
    anim.SetBool("isWalking", false);
    anim.SetBool("isAttacking", true);
}
if (Input.GetKey(KeyCode.W))
{
    anim.SetBool("isWalking", true);
}
else
    anim.SetBool("isWalking", false);

if (Input.GetKey(KeyCode.R))
{
    anim.SetBool("isRunning", true);
}
else
    anim.SetBool("isRunning", false);

if (Input.GetKey(KeyCode.A))
{
    anim.SetBool("isAttacking", true);
}
else
    anim.SetBool("isAttacking", false);

if (Input.GetKey(KeyCode.D))
{
    anim.SetBool("isDead", true);
}*/