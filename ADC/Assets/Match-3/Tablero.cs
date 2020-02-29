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

    public Tablero(int _alto, int _ancho)
    {
        _alto = this.alto;
        _ancho = this.ancho;
    }

    private void Start()
    {
        tCelulas = new GameObject[ancho, alto];

        for(int i = 0; i < ancho; i++)
        {
            for(int j = 0; j < alto; j++)
            {
                //Posición en la malla
                Vector2 posicion = new Vector2(i, j);
                //Selección al azar de la célula del arreglo celulas
                int indiceCelula = Random.Range(0, celulas.Length);
                GameObject backgroundTile = Instantiate(tilePrefab, posicion, Quaternion.identity) as GameObject;
                GameObject celula = Instantiate(celulas[indiceCelula], posicion, Quaternion.identity) as GameObject;

                //Para instanciar los tiles como hijos de Tablero
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";
     
                //Para instanciar las células como hijos de Tablero
                celula.transform.parent = this.transform;
                celula.name = "(" + i + "," + j + ")";
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
