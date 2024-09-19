using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;       // Estado da porta
    public float rotationSpeed = 2f;   // Velocidade da rotação
    public float openAngle = 90f;      // Ângulo de abertura da porta

    private Quaternion closedRotation; // Rotação inicial (porta fechada)
    private Quaternion openRotation;   // Rotação final (porta aberta)

    public Collider doorCollider;
    
    void Start()
    {
        if(doorCollider == null){
            doorCollider = GetComponent<Collider>();
        }
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0); // Define a rotação aberta
    }

    public void useDoor()
    {
        if(!isOpen){
            StopAllCoroutines(); // Para qualquer rotação em andamento
            StartCoroutine(RotateDoor(openRotation)); // Inicia a rotação para abrir a porta
            isOpen = true;
            doorCollider.enabled = false;
        }
       else if (isOpen)
        {
            StopAllCoroutines(); // Para qualquer rotação em andamento
            StartCoroutine(RotateDoor(closedRotation)); // Inicia a rotação para fechar a porta
            isOpen = false;
            doorCollider.enabled = true;
        }
    }
    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null; // Espera o próximo frame
        }

        transform.rotation = targetRotation; // Garante que a rotação final seja precisa
        
    }
}