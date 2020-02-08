using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSpawner : MonoBehaviour
{
    //Tiempo de aparición de virus
    public float spawnRate = 2.0f;
    //Tiempo desde la última aparición del objeto
    float tiempo = 0f;
    //Objeto para hacer el pool
    public GameObject virus;
    //Lista de objetos
    List<GameObject> lVirus;
    //Tamaño de lista de pool
    public int poolTam = 20;
    //Total de objetos en escena
    public GameObject[] tVirus;
    //Índice de objeto
    public int i = 0;
    //Posición de aparición de objeto sobre el eje X y Y
    public float posicionY;
    public float posicionX;
    //Distancia entre los tres puntos de aparición de virus
    public const float DIST_VIRUS = 1.7f;
    //Variable para selección de carril de virus
    public int j;
    
    void Start()
    {
        //Posición Y tomada del transform del propio gameObject
        posicionY = transform.position.y;

        lVirus = new List<GameObject>();

        //Creación de la lista de objetos
        for (int i = 0; i < poolTam; i++)
        {
            GameObject obj = (GameObject)Instantiate(virus);
            obj.SetActive(false);
            lVirus.Add(obj);
        }
    }
    
    void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo >= spawnRate && i < poolTam)
        {
            tiempo = 0f;

            j = Random.Range(1, 4);

            //Posición para el virus dependiendo del número random dado por j
            switch (j)
            {
                //Carril izquierdo
                case 1:
                    posicionX = transform.position.x - DIST_VIRUS;
                    break;

                //Carril central
                case 2:
                    posicionX = transform.position.x;
                    break;

                //Carril derecho
                case 3:
                    posicionX = transform.position.x + DIST_VIRUS;
                    break;

                default:
                    posicionX = transform.position.x;
                    break;

            }
            
            lVirus[i].transform.position = new Vector2(posicionX, posicionY);
            lVirus[i].transform.rotation = transform.rotation;
            lVirus[i].SetActive(true);
            i++;
        }

        else if (tiempo > spawnRate)
        {
            tiempo = 0f;
            
            tVirus = GameObject.FindGameObjectsWithTag("Virus");

            //Reinicia el iterador para comenzar nuevamente el pool
            if (tVirus.Length == 0)
            {
                i = 0;
            }
        }
    }
}
