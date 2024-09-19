using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.AI;



public class AlienController : MonoBehaviour {
    // public NavMeshAgent alien;
    // public Transform player;
    // void Start() {
    //     alien = GetComponent<NavMeshAgent>();
    //     player = GameObject.Find("alvo").transform;
    // }

    // void Update() {
    //     alien.SetDestination(player.position);
    // }



    public NavMeshAgent alien;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
    private Animator animator;

    //Patrulha
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    //Atacando
    public float timeBetweenAttacks;
    private bool hasAttacked;

    //Stados
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start(){
        player = GameObject.FindWithTag("Player").transform;
        alien = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        playerInSightRange =  Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange =  Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInSightRange && playerInAttackRange) Attack();
    }

    private void Patrol() {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) {
            alien.SetDestination(walkPoint);
        } 
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
    }

    private void Chase() {
        alien.SetDestination(player.position);
    }

    private void Attack() {
        alien.SetDestination(transform.position); // Faz ele parar exatamente onde está
        transform.LookAt(player);
        if (!hasAttacked) {
            // Codigo do Ataque
            animator.SetTrigger("Attack");
            Debug.Log("Bateu");


            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true;
        }
    }
    private void ResetAttack() {
        Debug.Log("Resetou");
        hasAttacked = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
    }

}
