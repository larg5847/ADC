using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celula : MonoBehaviour
{
    public int fila;
    public int columna;
    public int posX;
    public int posY;
    Tablero tablero;
    GameObject celula;

    //Parámetros necesarios para obtener el ángulo de deslizamiento
    Vector2 posicionInicial;
    Vector2 posicionFinal;
    Vector2 posicionTemp;
    public float anguloDeslizamiento;

    private void Start()
    {
        //Encuentra objeto activo del tipo Tablero en escena
        tablero = FindObjectOfType<Tablero>();

        //Posición de la célula
        posX = (int)transform.position.x;
        posY = (int)transform.position.y;

        columna = posX;
        fila = posY;
    }
    
    private void Update()
    {
        posX = columna;
        posY = fila;

        //Mueve X
        if(Mathf.Abs(posX - transform.position.x) > 0.1)
        {
            posicionTemp = new Vector2(posX, transform.position.y);
            //Interpola
            transform.position = Vector2.Lerp(transform.position, posicionTemp, 0.4f);
        }

        else
        {
            posicionTemp = new Vector2(posX, transform.position.y);
            transform.position = posicionTemp;
            tablero.tCelulas[columna, fila] = this.gameObject;
        }

        //Mueve Y
        if (Mathf.Abs(posY - transform.position.y) > 0.1)
        {
            posicionTemp = new Vector2(transform.position.x, posY);
            //Interpola
            transform.position = Vector2.Lerp(transform.position, posicionTemp, 0.4f);
        }

        else
        {
            posicionTemp = new Vector2(transform.position.x, posY);
            transform.position = posicionTemp;
            tablero.tCelulas[columna, fila] = this.gameObject;
        }
    }

    //Se obtienen las posiciones cuando se presiona y suelta una célula,
    //y se convierten esas posiciones de pantalla a espacio global
    private void OnMouseDown()
    {
        posicionInicial = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        posicionFinal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        calculaAngulo();
    }

    //Por trigonometría se calcula el ángulo de deslizamiento (grados) 
    //con el arco tangente, con las distancias de posición inicial y 
    //final
    private void calculaAngulo()
    {
        anguloDeslizamiento = Mathf.Atan2(posicionFinal.y - posicionInicial.y,
            posicionFinal.x - posicionInicial.x) * 180/Mathf.PI;
        Debug.Log(anguloDeslizamiento);

        mueveCelulas();
    }

    //Dependiendo del ángulo de deslizamiento se intercambian las
    //posiciones de dos células
    private void mueveCelulas()
    {
        //Deslizamiento derecha
        if (anguloDeslizamiento > -15 && anguloDeslizamiento <= 15 && columna < tablero._ancho)
        {
            celula = tablero.tCelulas[columna + 1, fila];
            celula.GetComponent<Celula>().columna -= 1;
            columna += 1;
        }

        //Deslizamiento izquierda
        else if (anguloDeslizamiento > 150 || anguloDeslizamiento <= -150 && columna > 0)
        {
            celula = tablero.tCelulas[columna - 1, fila];
            celula.GetComponent<Celula>().columna += 1;
            columna -= 1;
        }

        //Deslizamiento arriba
        else if (anguloDeslizamiento > 60 && anguloDeslizamiento <= 115 && fila < tablero._alto)
        {
            celula = tablero.tCelulas[columna, fila + 1];
            celula.GetComponent<Celula>().fila -= 1;
            fila += 1;
        }

        //Deslizamiento abajo
        else if (anguloDeslizamiento >= -110 && anguloDeslizamiento < -60 && fila > 0)
        {
            celula = tablero.tCelulas[columna, fila - 1];
            celula.GetComponent<Celula>().fila += 1;
            fila -= 1;
        }
    }
}
