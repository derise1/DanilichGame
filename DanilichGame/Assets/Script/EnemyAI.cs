using System;
using UnityEngine;
using System.Collections;
using Random=UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [Header("ENEMY SETTINGS")]
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public LayerMask Ground, Character;
    public HealthAIEnemy health;
    public registerHit regHit;

    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("States")]
    public float sightRange, attackRange, distPlayer;
    public bool playerIsSightRange;
    public bool playerIsAttackRange;
    public bool playerIsClose;

    [Header("Attacking")]
    public GameObject Bullet;
    public GameObject shootPoint;
    public float timeBetweenAttacks = 1f;
    public bool alreadyAttacked;
    private int damage;
    public float distance = 40f;
    public bool die;
    public bool registerHit = false;
    public bool overrideAttack;
    
    [Header("Animations")]
    public Animator animationsEnemy;

    [Header("Muzzle Flash")]
    public ParticleSystem muzzle;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip footStep;

    private void Awake(){
        
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animationsEnemy = GetComponent<Animator>();
    }

    private void Update(){

        //Check for sight and attack range
        playerIsSightRange = Physics.CheckSphere(transform.position, sightRange, Character);
        playerIsAttackRange = Physics.CheckSphere(transform.position, attackRange, Character);
        playerIsClose = Physics.CheckSphere(transform.position, distPlayer, Character);
        
        if(die){
            Die();
        }
        else{
            if(!overrideAttack){
                if(!playerIsSightRange && !playerIsAttackRange) {
                    Patroling();
                }
                if(playerIsSightRange && !playerIsAttackRange) {
                    ChasePlayer();  
                }
                if(playerIsSightRange && playerIsAttackRange) {
                    if(!registerHit){
                        AttackPlayer();
                    }
                }
            }
            else{
                if (!playerIsAttackRange && player != null){
                    if(!registerHit){
                        ChasePlayerAttack();
                    }
                }

                if (playerIsAttackRange)
                    overrideAttack = false;
            }
        }
    }   
    
    private void Patroling(){
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, Ground)){
            walkPointSet = true;
        }
    }

    private void ChasePlayer(){
        agent.SetDestination(player.position);
        animationsEnemy.SetBool("isFire", false);
    }

    public void ChasePlayerAttack(){
        agent.SetDestination(player.position);
        
        AttackPlayer();
    }

    private void AttackPlayer(){
        if(playerIsClose){
            agent.SetDestination(transform.position);
        }
        
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if(!alreadyAttacked && !die){
            RayCastPlayer();
        }
    }

    public void ResetAttack(){
        alreadyAttacked = false;
    }

    private void Die(){
        agent.SetDestination(transform.position);
    }

    public void SpawnBullet(){
        Rigidbody rb = Instantiate(Bullet, shootPoint.transform.position, shootPoint.transform.rotation).GetComponent<Rigidbody>();
        
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(transform.up, ForceMode.Impulse);

        animationsEnemy.SetBool("isFire", true);
        muzzle.Play();
        audioSource.PlayOneShot(fireSound, 0.1f);
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks); 
    }

    void RayCastPlayer(){
        RaycastHit hit;
        if(Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward, out hit, distance)){
            Debug.DrawRay(shootPoint.transform.position, shootPoint.transform.forward * distance, Color.green);
            if(hit.transform.tag == "Player"){
                SpawnBullet();
            }
            else{
                if(!die){
                    ChasePlayer();
                }
            }
        }
    }

    void TakeHitEnter(){
        registerHit = true;
    }

    void TakeHitExit(){
        registerHit = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distPlayer);
    }
}
