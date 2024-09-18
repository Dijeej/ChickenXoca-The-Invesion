using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAlienController : MonoBehaviour
{
    // Referência ao Animator
    private Animator alienAnimator;

    // Velocidade de movimento
    public float speed = 5.0f;

    // Inicialização
    void Start()
    {
        // Obtém o componente Animator anexado ao GameObject
        alienAnimator = GetComponent<Animator>();
    }

    // Atualizado a cada frame
    void Update()
    {
        // Movimentação simples com as setas ou WASD
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        transform.Translate(movement * speed * Time.deltaTime);

        // Verifica se há movimento para acionar a animação de caminhada
        if (movement.magnitude > 0)
        {
            alienAnimator.SetBool("isWalking", true);
        }
        else
        {
            alienAnimator.SetBool("isWalking", false);
        }

        // Atribuir animações às teclas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            alienAnimator.SetTrigger("atack");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            alienAnimator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            alienAnimator.SetTrigger("Die");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            alienAnimator.SetTrigger("Wave");
        }
    }
}