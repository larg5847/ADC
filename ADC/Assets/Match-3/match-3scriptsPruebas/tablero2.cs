using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tablero2 : MonoBehaviour
{

    public int ancho;
    public int alto;

    public GameObject tilePrefab;
    //Arreglo de prefabs de las distintas células a utilizar
    public GameObject[] celulas;
    //Para objeto fuera del tablero
    public int offset;

    //Arreglo bidimensional para posición de células
    public GameObject[,] tCelulas;

    //
    BackgroundTile[,] tTiles;

    //public EstadoJuego estadoActual = EstadoJuego.mueve;

    EncuentraMatches encuentraMatches;
    public Celula celulaActual;

    public tablero2(int _alto, int _ancho)
    {
        this.alto = _alto;
        this.ancho = _ancho;
    }




    public int _alto
    {
        get => alto;
        set { alto = value; }
    }

    public int _ancho
    {
        get => ancho;
        set { alto = value; }
    }

    public GameObject[,] _tCelulas
    {
        get => tCelulas;
        set { tCelulas = value; }
    }






    private void Start()
    {
        //encuentraMatches = FindObjectOfType<EncuentraMatches>();

        //
        tTiles = new BackgroundTile[ancho, alto];
        tCelulas = new GameObject[ancho, alto];

        llenaTablero();
    }

    private void llenaTablero()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                //Posición en la malla
                Vector2 posicion = new Vector2(i, j + offset);
                //Selección al azar de la célula del arreglo celulas
                int indiceCelula = Random.Range(0, celulas.Length);
                int iteraciones = 0;

                //Iteración que cambia la célula a poner para que no exista
                //match en el tablero al momento de llenarlo
                //Máximo de 100 iteraciones para que no entre en un loop
                //infinito
                /*while (matches(i, j, celulas[indiceCelula]) && iteraciones < 100)
                {
                    indiceCelula = Random.Range(0, celulas.Length);
                    iteraciones++;
                }*/

                GameObject backgroundTile = Instantiate(tilePrefab, new Vector2(posicion.x, posicion.y - offset),
                    Quaternion.identity) as GameObject;
                GameObject celula = Instantiate(celulas[indiceCelula], posicion, Quaternion.identity);

                //Actualización de las células y tiles en el tablero, 
                //debido a que tienen una posición incorrecta por el 
                //offset
                celula.GetComponent<Celula>().columna = i;
                celula.GetComponent<Celula>().fila = j;

                //Para instanciar los tiles como hijos de Tablero
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";

                //Para instanciar las células como hijos de Tablero
                celula.transform.parent = this.transform;
                celula.name = "(" + i + "," + j + ")";

                tCelulas[i, j] = celula;
            }
        }

        Debug.Log(encuentraMatches.encuentraPosiblesMatches());
    }

}
