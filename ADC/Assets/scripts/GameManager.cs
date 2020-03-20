using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script que activa/desactiva otros scripts de los prefabs, 
//dependiendo de a qué minijuego entra
public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    GameObject virus;
    GameObject monocito;
    GameObject neutrofilo;
    int indiceEscena;

    private void Start()
    {
        //Índice de escena actual buildeada
        indiceEscena = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        //Si se presiona el botón back se carga la escena del minimapa,
        //si está en el minipa regresa a la pantalla inicial
        //pero si está en la pantalla inicial, cierra el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (indiceEscena != 0)
            {
                if(indiceEscena == 1)
                    SceneManager.LoadScene(0);

                else
                    SceneManager.LoadScene(1);
            }
  
            else
                Application.Quit();
        }    
    }

    //Función que es llamada cuando se hace click en los botones del canvas
    //del minimapas
    public void activaScripts(string nombreMiniJuego)
    {
        //Primero busca los gameobjects por su etiqueta y se le
        //asigna a un gameobject temporal
        for (int i = 0; i < gameObjects.Length; i++)
        {
            string nombre;

            nombre = gameObjects[i].tag;

            switch (nombre)
            {
                case "Virus":
                    virus = gameObjects[i];
                    break;

                case "Monocito":
                    monocito = gameObjects[i];
                    break;

                case "Neutrofilo":
                    neutrofilo = gameObjects[i];
                    break;

                default:
                    break;
            }

        }

        //Dependiendo del minijuego a entrar se activa/desactivan los scripts
        switch (nombreMiniJuego)
        {
            case "MinijuegoMonocitoNeutrofilo":
                virus.GetComponent<DestruyeVirus>().enabled = true;
                virus.GetComponent<Celula>().enabled = false;
                virus.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
                virus.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                virus.GetComponent<CircleCollider2D>().isTrigger = true;
                monocito.GetComponent<Celula>().enabled = false;
                neutrofilo.GetComponent<Celula>().enabled = false;
                neutrofilo.GetComponent<NeutrofiloComportamiento>().enabled = true;
                break;

            case "Match-3":
                virus.GetComponent<DestruyeVirus>().enabled = false;
                virus.GetComponent<Celula>().enabled = true;
                virus.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                virus.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                virus.GetComponent<CircleCollider2D>().isTrigger = false;
                monocito.GetComponent<Celula>().enabled = true;
                neutrofilo.GetComponent<Celula>().enabled = true;
                neutrofilo.GetComponent<NeutrofiloComportamiento>().enabled = false;
                break;

            default:
                break;

        }
    }
}
