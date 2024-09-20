using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    int isWalkingHash;
    int isRunningHash;
    int isRetreatingHash;
    int isRunningBackHash;

    public float turnSpeed = 200f; // Velocidade de rotação
    public float moveSpeed = 5f;   // Velocidade de movimento
    public float runSpeed = 10f;    // Velocidade de corrida

    public int maxHealth = 100;    // Vida máxima do personagem
    public int currentHealth;      // Vida atual do personagem
    public int damageAmount = 10;  // Dano causado ao colidir com o objeto

    public int itemCount = 0;      // Contador de itens coletados

    private GameObject nearbyObject;
    
    public HealthBarScript healthBar;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movesAnimatiorStates();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        playerMovements();
        if (Input.GetKeyDown(KeyCode.Q) && nearbyObject != null)
        {
            if(nearbyObject.CompareTag("Item")){
                CollectItem(nearbyObject); 
            }
            
            if(nearbyObject.CompareTag("Porta")){
                UseDoor(nearbyObject); // Abre a porta quando o jogador pressiona Q
            }
        }

    }

    private void movesAnimatiorStates(){
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isRetreatingHash = Animator.StringToHash("isRetreating");
        isRunningBackHash = Animator.StringToHash("isRunningBack");
    }

    private void playerMovements(){
        // Entrada do jogador
        bool forwardPress = Input.GetKey("w");
        bool backPress = Input.GetKey("s");
        bool leftPress = Input.GetKey("a");
        bool rightPress = Input.GetKey("d");
        bool runPress = Input.GetKey("left shift") || Input.GetKey("right shift");

        // Atualizar estado de andar (frente e lados)
        bool isWalking = forwardPress;
        bool isWalkingBack = backPress;

        animator.SetBool(isWalkingHash, isWalking);

        animator.SetBool(isRunningHash, forwardPress && runPress);

        animator.SetBool(isRetreatingHash, isWalkingBack);

        animator.SetBool(isRunningBackHash, backPress && runPress);

         Vector3 movement = Vector3.zero;

        if (leftPress)
        {
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
        if (rightPress)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }

        // Movimento para frente
        if (forwardPress)
        {
            float speed = runPress ? runSpeed : moveSpeed; // Corre se estiver pressionando o shift
            movement = transform.forward * speed * Time.deltaTime;
        }
        // Movimento para trás
        else if (backPress)
        {
            float speed = runPress ? runSpeed : moveSpeed; // Corre para trás se estiver pressionando o shift
            movement = -transform.forward * speed * Time.deltaTime;
        }

        rb.MovePosition(rb.position + movement);

        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("PipeAttack");
            animator.SetTrigger("atack");
        }
    }
     
    private void OnCollisionEnter(Collision collision)
    {
     
        if (collision.gameObject.CompareTag("Bus"))
        {
            Debug.Log("Fase 1 Concluida");
        }
        // Verifica se o objeto com o qual o personagem colidiu tem a tag "Dangerous"
        if (collision.gameObject.CompareTag("Dangerous"))
        {
            TakeDamage(damageAmount); // Aplica o dano
        }
    }
    private void DealDamage(int damage) {

    }
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Personagem tomou dano! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // Chama a função de morte se a vida chegar a 0
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            nearbyObject = other.gameObject; // Armazena o item próximo
            Debug.Log("Pressione 'Q' para coletar o item.");
        }

        if (other.gameObject.CompareTag("Porta"))
        {
            nearbyObject = other.gameObject; // Armazena o item próximo
            Debug.Log("Pressione 'Q' para mover a porta.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("Porta"))
        {
            nearbyObject = null; // Remove a referência ao objeto ao sair do trigger
            Debug.Log("Objeto fora de alcance.");
        }
    }   

    private void UseDoor(GameObject door)
    {
        DoorController doorController = door.GetComponent<DoorController>();
        doorController.useDoor();
        Destroy(door);
    }
    private void CollectItem(GameObject item)
    {
        itemCount++; // Incrementa o contador de itens coletados
        Debug.Log("Item coletado! Total de itens: " + itemCount);

        Destroy(item); // Remove o item do jogo
    }

    // Função de morte do personagem
    private void Die()
    {
        Debug.Log("Personagem morreu!");
        // Aqui você pode implementar lógica para reiniciar o jogo, exibir uma animação de morte, etc.
    }
    
}
