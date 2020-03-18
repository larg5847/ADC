﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celula : MonoBehaviour
{
    [Header("Variables de tablero")]
    public int fila;
    public int columna;
    public int filaAnterior;
    public int columnaAnterior;
    //Posición objetivo en X y Y
    public int posX;
    public int posY;
    Tablero tablero;
    //Variable para referenciar a la célula de a lado
    GameObject celula;

    //Parámetros necesarios para obtener el ángulo de deslizamiento
    Vector2 posicionInicial;
    Vector2 posicionFinal;
    Vector2 posicionTemp;
    public float anguloDeslizamiento = 0;

    //Variable para saber si hizo match
    public bool matched = false;

    private void Start()
    {
        //Encuentra objeto activo del tipo Tablero en escena
        tablero = FindObjectOfType<Tablero>();

        //Posición de la célula
        posX = (int)transform.position.x;
        posY = (int)transform.position.y;

        columna = posX;
        fila = posY;
        columnaAnterior = columna;
        filaAnterior = fila;
    }
    
    private void Update()
    {
        encuentraMatches();

        if (matched)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(0f, 0f, 0f, 0.2f);
        }

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

    //Corutina que checa si no se hizo match, regresa las células 
    //a su posición original
    public IEnumerator verificaMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if(celula != null)
        {
            if(!matched && !celula.GetComponent<Celula>().matched)
            {
                //La célula de un lado retoma su fila y columna original
                celula.GetComponent<Celula>().fila = fila;
                celula.GetComponent<Celula>().columna = columna;
                //Se actualiza fila y columna de la célula con los valores
                //originales
                fila = filaAnterior;
                columna = columnaAnterior;
            }
            celula = null;
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
        //Utilizo clamp para limitar las distancias, pero no lo veo tan factible
        //porque daría el mismo resultado si solo hago comparaciones con 
        //la diferencia de las posiciones inicial y final
        Vector2 limiteMovimiento = new Vector2(
            Mathf.Clamp((posicionFinal.x - posicionInicial.x), -1.0f, 1.0f),
            Mathf.Clamp((posicionFinal.y - posicionInicial.y), -1.0f, 1.0f));

        if (Mathf.Abs(limiteMovimiento.x) >= 0.8 || Mathf.Abs(limiteMovimiento.y) >= 0.8)
        {
            anguloDeslizamiento = Mathf.Atan2(limiteMovimiento.y,
                limiteMovimiento.x) * 180 / Mathf.PI;

            mueveCelulas();
        }
    }

    //Dependiendo del ángulo de deslizamiento se intercambian las
    //posiciones de dos células
    private void mueveCelulas()
    {
        //Deslizamiento derecha
        if (anguloDeslizamiento > -15 && anguloDeslizamiento <= 15 && columna < tablero._ancho - 1)
        {
            celula = tablero.tCelulas[columna + 1, fila];
            celula.GetComponent<Celula>().columna -= 1;
            columna += 1;
        }

        //Deslizamiento izquierda
        else if ((anguloDeslizamiento > 165 || anguloDeslizamiento <= -165) && columna > 0)
        {
            celula = tablero.tCelulas[columna - 1, fila];
            celula.GetComponent<Celula>().columna += 1;
            columna -= 1;
        }

        //Deslizamiento arriba
        else if (anguloDeslizamiento > 75 && anguloDeslizamiento <= 105 && fila < tablero._alto - 1)
        {
            celula = tablero.tCelulas[columna, fila + 1];
            celula.GetComponent<Celula>().fila -= 1;
            fila += 1;
        }

        //Deslizamiento abajo
        else if (anguloDeslizamiento >= -105 && anguloDeslizamiento < -75 && fila > 0)
        {
            celula = tablero.tCelulas[columna, fila - 1];
            celula.GetComponent<Celula>().fila += 1;
            fila -= 1;
        }

        StartCoroutine(verificaMatch());
    }

    //Función que verifica si hay match en las células que tiene alrededor,
    //esto lo hace comparando por medio de etiquetas
    private void encuentraMatches()
    {
        if(columna > 0 && columna < tablero.ancho - 1)
        {
            //Células a los lados izquierda y derecha
            GameObject celulaIzq = tablero.tCelulas[columna - 1, fila];
            GameObject celulaDer = tablero.tCelulas[columna + 1, fila];
            
            //Checa que las células de alrededor no estén vacías
            if (celulaIzq != null && celulaDer != null)
            {
                if (celulaIzq.tag == this.gameObject.tag && celulaDer.tag == this.gameObject.tag)
                {
                    celulaIzq.GetComponent<Celula>().matched = true;
                    celulaDer.GetComponent<Celula>().matched = true;
                    matched = true;
                }
            }
            
        }

        if(fila > 0 && fila < tablero.alto - 1)
        {
            //Células a los lados arriba y abajo
            GameObject celulaArr = tablero.tCelulas[columna, fila + 1];
            GameObject celulaAba = tablero.tCelulas[columna, fila - 1];

            //Checa que las células de alrededor no estén vacías
            if (celulaArr != null && celulaAba != null)
            {
                if (celulaArr.tag == this.gameObject.tag && celulaAba.tag == this.gameObject.tag)
                {
                    celulaArr.GetComponent<Celula>().matched = true;
                    celulaAba.GetComponent<Celula>().matched = true;
                    matched = true;
                }
            }
        }
    }
}
