using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    public NavMeshAgent alien;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
    private Animator animator;
    private int health;
    private float delayDestroy = 30f;
    private bool isDead = false;
    private bool isAttacking = false; // Indica se o alien está atacando
    private bool canDamagePlayer = true ;

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

        if (!isDead) {
            if (!playerInSightRange && !playerInAttackRange) Patrol();
            if (playerInSightRange && !playerInAttackRange) Chase();
            if (playerInSightRange && playerInAttackRange) Attack();
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

            // Se puder causar dano, faz isso e inicia o cooldown
            if (canDamagePlayer)
            {
                // Supondo que o player tenha um método TakeDamage
                AJController playerController = player.GetComponent<AJController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(20); // Aplica o dano de 20 ao player
                    canDamagePlayer = false; // Impede de causar dano repetido imediatamente
                    Invoke(nameof(ResetAttack), timeBetweenAttacks); // Aguarda o cooldown
                }
            }

            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        isAttacking = false; // Desativa o estado de ataque após o ataque ser concluído
        hasAttacked = false;
        canDamagePlayer = true;
        Debug.Log("Alien terminou de atacar.");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && isAttacking) {
            AJController playerController = other.GetComponent<AJController>();
            if (playerController != null) {
                playerController.TakeDamage(20); // Aplica 20 de dano ao player
                Debug.Log("Alien causou dano ao player.");
            }
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

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true;
        }
    }

    private void Die() {
        Debug.Log("O alien morreu");
        animator.SetTrigger("Died");

        isDead = true;
        if (alien.enabled) {
            alien.enabled = false;
        }

        Invoke(nameof(DestroyAlien), delayDestroy);
    }

    private void DestroyAlien() {
        Destroy(alien.gameObject);
    }
}

