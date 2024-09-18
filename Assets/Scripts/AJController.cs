using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isRetreatingHash;
    int isRunningBackHash;

    public float turnSpeed = 200f; // Velocidade de rotação
    public float moveSpeed = 2f;   // Velocidade de movimento
    public float runSpeed = 4f;    // Velocidade de corrida

    public int maxHealth = 100;    // Vida máxima do personagem
    public int currentHealth;      // Vida atual do personagem
    public int damageAmount = 10;  // Dano causado ao colidir com o objeto

    void Start()
    {
        movesAnimatiorStates();
        currentHealth = maxHealth;
    }

    void Update()
    {
        playerMovements();
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
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        // Movimento para trás
        else if (backPress)
        {
            float speed = runPress ? runSpeed : moveSpeed; // Corre para trás se estiver pressionando o shift
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
     
     private void OnCollisionEnter(Collision collision)
    {
        // Verifica se o objeto com o qual o personagem colidiu tem a tag "Dangerous"
        if (collision.gameObject.CompareTag("Dangerous"))
        {
            TakeDamage(damageAmount); // Aplica o dano
        }
    }
        private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Personagem tomou dano! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // Chama a função de morte se a vida chegar a 0
        }
    }

    // Função de morte do personagem
    private void Die()
    {
        Debug.Log("Personagem morreu!");
        // Aqui você pode implementar lógica para reiniciar o jogo, exibir uma animação de morte, etc.
    }
    
}
