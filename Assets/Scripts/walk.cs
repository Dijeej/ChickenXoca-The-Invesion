using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MonoBehaviour 
{
    public float turnSpeed = 200f; // Velocidade de rotação
    public float moveSpeed = 10f;   // Velocidade de movimento
    public float runSpeed = 40f;    // Velocidade de corrida
    void Start() {
        
    }
    void Update() {
        bool forwardPress = Input.GetKey("w");
        bool backPress = Input.GetKey("s");
        bool leftPress = Input.GetKey("a");
        bool rightPress = Input.GetKey("d");
        bool runPress = Input.GetKey("left shift") || Input.GetKey("right shift");

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
}