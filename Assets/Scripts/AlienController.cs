using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    public NavMeshAgent alien;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
    private Animator animator;
    private int health;
    //private int maxHealth = 3;
    private float delayDestroy = 30f;
    private bool isDead = false;

    // Patrulha
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Atacando
    public float timeBetweenAttacks;
    private bool hasAttacked;

    // Estados
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start() {
        player = GameObject.FindWithTag("Player").transform;
        alien = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = 3;
    }

    private void Update() {
        // Verifica se o jogador está dentro do range de visão ou de ataque
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!isDead) {
            if (!playerInSightRange && !playerInAttackRange) Patrol();
            if (playerInSightRange && !playerInAttackRange) Chase();
            if (playerInSightRange && playerInAttackRange) Attack();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colidiu com: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Gun"))
        {
            Debug.Log("Atingido pela arma!");
            TakesDamage(1);
        }
        Debug.Log("Colidiu com: " + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("Gun"))
        {
            TakesDamage(1);
        }

        if (objeto.gameObject.CompareTag("Player"))
        {
            TakesDamage(1);
        }
    }
    private void Patrol() {
        if (!walkPointSet) {
            SearchWalkPoint();
            }

        if (walkPointSet && alien.enabled) {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            alien.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
    }

    private void Chase() {
        if (alien.enabled) {
            alien.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
    }

    private void Attack() {
        if (alien.enabled) {
            alien.SetDestination(transform.position);  // Faz ele parar exatamente onde está
        }
        
        transform.LookAt(player);

        if (!hasAttacked) {
            // Código do Ataque
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

    public void TakesDamage(int damage) {
        animator.SetTrigger("Hitted");
        Debug.Log("Tomou dano");
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        Debug.Log("O alien morreu");
        animator.SetTrigger("Died");

        isDead = true; 
        // Parar o NavMeshAgent
        if (alien.enabled) { 
            // alien.isStopped = true;  
            alien.enabled = false;
        }

        // Destrói o alien após o atraso
        Invoke(nameof(DestroyAlien), delayDestroy);
    }

    private void DestroyAlien() {
        Destroy(alien.gameObject);  // Destrói o objeto alien inteiro
    }
}
