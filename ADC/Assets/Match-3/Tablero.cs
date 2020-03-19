using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    public int ancho;
    public int alto;
    public GameObject tilePrefab;
    //Arreglo de prefabs de las distintas células a utilizar
    public GameObject[] celulas;

    //Arreglo bidimensional para posición de células
    public GameObject[,] tCelulas;

    //
    BackgroundTile[,] tTiles;

    public Tablero(int _alto, int _ancho)
    {
        _alto = this.alto;
        _ancho = this.ancho;
    }

    private void Start()
    {
        //
        tTiles = new BackgroundTile[ancho, alto];
        tCelulas = new GameObject[ancho, alto];

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                //Posición en la malla
                Vector2 posicion = new Vector2(i, j);
                //Selección al azar de la célula del arreglo celulas
                int indiceCelula = Random.Range(0, celulas.Length);
                int iteraciones = 0;

                //Iteración que cambia la célula a poner para que no exista
                //match en el tablero al momento de llenarlo
                //Máximo de 100 iteraciones para que no entre en un loop
                //infinito
                while (matches(i, j, celulas[indiceCelula]) && iteraciones < 100)
                {
                    indiceCelula = Random.Range(0, celulas.Length);
                    iteraciones++;
                }

                GameObject backgroundTile = Instantiate(tilePrefab, posicion, Quaternion.identity) as GameObject;
                GameObject celula = Instantiate(celulas[indiceCelula], posicion, Quaternion.identity);

                //Para instanciar los tiles como hijos de Tablero
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";

                //Para instanciar las células como hijos de Tablero
                celula.transform.parent = this.transform;
                celula.name = "(" + i + "," + j + ")";

                tCelulas[i, j] = celula;
            }
        }
    }

    //Función que no deja que existan matches al momento de llenar
    //el tablero, regresa un true si hay match
    private bool matches(int columna, int fila, GameObject celula)
    {
        //Comparación de la tercera fila y columna en adelante
        if (columna > 1 && fila > 1)
        {
            if (tCelulas[columna - 1, fila].tag == celula.tag &&
                tCelulas[columna - 2, fila].tag == celula.tag)
                return true;

            if (tCelulas[columna, fila - 1].tag == celula.tag &&
                tCelulas[columna, fila - 2].tag == celula.tag)
                return true;
        }

        //Comparación para casos de la primera o segunda fila o columna 
        else if (columna <= 1 || fila <= 1)
        {
            if (columna > 1)
            {
                if ((tCelulas[columna - 1, fila].tag == celula.tag &&
                tCelulas[columna - 2, fila].tag == celula.tag))
                    return true;
            }

            if (fila > 1)
            {
                if ((tCelulas[columna, fila - 1].tag == celula.tag &&
                tCelulas[columna, fila - 2].tag == celula.tag))
                    return true;
            }
        }
        return false;
    }

    //Función que destruye la célula que hizo match
    private void destruyeMatches(int columna, int fila)
    {
        if (tCelulas[columna, fila].GetComponent<Celula>().matched)
        {
            Destroy(tCelulas[columna, fila]);
            tCelulas[columna, fila] = null;
        }
    }

    //Función que destruye todos los matches del tablero
    public void destruyeMatches()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                if (tCelulas[i, j] != null)
                    destruyeMatches(i, j);
            }
        }
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
}

