using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntaje : MonoBehaviour
{ 
    int puntaje;
    public Text puntajeText;

    // Start is called before the first frame update
    void Start()
    {
        puntajeText = GetComponent<Text>();
        puntaje = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AumentaPuntos(int puntos)
    {
        Debug.Log("Puntos: " + puntos);

        puntaje += puntos;

        puntajeText.text = puntaje.ToString();
    }
}
