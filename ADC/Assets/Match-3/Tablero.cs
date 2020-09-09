using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estados de juego que sirven para evitar que el jugador
//pueda hacer movimientos mientras aún existan matches en el tablero
//mientras caen nuevas células
//****Queda pendiente porque en algunas ocasiones se queda en espera
/*public enum EstadoJuego
{
    espera,
    mueve
}*/

public class Tablero : MonoBehaviour
{
    public NivelMatch nivelMatch;
    [SerializeField] private int ancho;
    [SerializeField] private int alto;

    public GameObject tilePrefab;
    //Arreglo de prefabs de las distintas células a utilizar
    //public GameObject[] celulas;
    //Diccionario que sustituye al arreglo de los prefabs
    private Dictionary<NivelMatch.TipoPrefab, GameObject> prefabsDic;
    //Para objeto fuera del tablero
    public int offset;

    //Arreglo bidimensional para posición de células
    public GameObject[,] tCelulas;

    //
    BackgroundTile[,] tTiles;

    //public EstadoJuego estadoActual = EstadoJuego.mueve;

    EncuentraMatches encuentraMatches;
    public Celula celulaActual;

    public Tablero(int _alto, int _ancho)
    {
        this.alto = _alto;
        this.ancho = _ancho;
    }

    private void Start()
    {
        alto = nivelMatch.alto;
        ancho = nivelMatch.ancho;

        prefabsDic = new Dictionary<NivelMatch.TipoPrefab, GameObject>();

        //Se agrega al diccionario los tipos de los prefabs y sus
        //prefabs que contiene el nivel
        for (int i = 0; i < nivelMatch.prefabs.Length; i++)
        {
            if (!prefabsDic.ContainsKey(nivelMatch.prefabs[i].tipoPrefab))
            {
                prefabsDic.Add(nivelMatch.prefabs[i].tipoPrefab, nivelMatch.prefabs[i].prefab);
            }
        }

        encuentraMatches = FindObjectOfType<EncuentraMatches>();

        //
        tTiles = new BackgroundTile[ancho, alto];
        tCelulas = new GameObject[ancho, alto];

        llenaTablero();
    }

    //Inicializa el tablero llenando cada posición con una célula
    private void llenaTablero()
    {
        Dictionary<NivelMatch.TipoPrefab, GameObject>.KeyCollection tipoPrefabs =
                    prefabsDic.Keys;

        //Se crea una lista de TipoPrefab para poder elegir de forma 
        //random un valor del diccionario
        List<NivelMatch.TipoPrefab> tipoPrefabsList = new List<NivelMatch.TipoPrefab>();

        foreach (NivelMatch.TipoPrefab t in tipoPrefabs)
        {
            tipoPrefabsList.Add(t);
        }

        NivelMatch.TipoPrefab[] previoIzquierda = new NivelMatch.TipoPrefab[alto];
        NivelMatch.TipoPrefab previoAbajo;

        for (int i = 0; i < ancho; i++)
        {
            previoAbajo = NivelMatch.TipoPrefab.VACIO;

            for (int j = 0; j < alto; j++)
            {
                //Posición en la malla
                Vector2 posicion = new Vector2(i, j + offset);
                //Selección al azar de la célula del arreglo celulas
                //int indiceCelula = Random.Range(0, celulas.Length);

                //Para que los prefabs no se repitan se descartan en una lista
                //los que ocasionarían un match
                List<NivelMatch.TipoPrefab> posiblesPrefabs = new List<NivelMatch.TipoPrefab>();
                
                posiblesPrefabs.AddRange(tipoPrefabsList);
                posiblesPrefabs.Remove(previoIzquierda[j]);
                posiblesPrefabs.Remove(previoAbajo);

                //Selección al azar de la lista
                int indiceCelula = Random.Range(0, posiblesPrefabs.Count);

                //Se actualiza el arreglo de previoIzquierda y previoAbajo
                //para no repetir prefabs
                previoIzquierda[j] = posiblesPrefabs[indiceCelula];
                previoAbajo = posiblesPrefabs[indiceCelula];

                //UPDATE: No se necesita para las iteraciones ya que se utiliza otro
                //algoritmo para que no existan matches en el tablero

                //int iteraciones = 0;

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
                GameObject celula = Instantiate(prefabsDic[posiblesPrefabs[indiceCelula]], posicion, Quaternion.identity);

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

    //UPDATE: No se necesita para las iteraciones ya que se utiliza otro
    //algoritmo para que no existan matches en el tablero

    //Función que no deja que existan matches al momento de llenar
    //el tablero, regresa un true si hay match
    /*private bool matches(int columna, int fila, GameObject celula)
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
    }*/

    //Función que destruye la célula que hizo match
    private void destruyeMatches(int columna, int fila)
    {
        if (tCelulas[columna, fila].GetComponent<Celula>().matched)
        {
            //Elimina de la lista, ya que no se necesita para el conteo 
            //total de los matches de las células destruidas
            if (encuentraMatches.matchesActuales.Count >= 4)
            {
                encuentraMatches.CrearBombas();
            }

            encuentraMatches.matchesActuales.Remove(tCelulas[columna, fila]);
            Destroy(tCelulas[columna, fila]);
            tCelulas[columna, fila] = null;
            //Deja vacía la célula actual(después de realizar el
            //movimiento)
            celulaActual = null;
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

        StartCoroutine(disminuyeFila());
    }

    //Corutina que verifica si hay espacios libres entre filas para
    //hacer que desciendan las células
    public IEnumerator disminuyeFila()
    {
        //Contador para los objetos nulos
        int contadorNull = 0;
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                if (tCelulas[i, j] == null)
                    contadorNull++;

                else if (contadorNull > 0)
                {
                    //Actualiza el valor de la fila y el de la fila anterior
                    tCelulas[i, j].GetComponent<Celula>().fila -= contadorNull;
                    tCelulas[i, j].GetComponent<Celula>().filaAnterior =
                        tCelulas[i, j].GetComponent<Celula>().fila;
                    //Y ahora lo que viene siendo su posición anterior se tiene
                    //un objeto vacío
                    tCelulas[i, j] = null;
                }
            }

            contadorNull = 0;
        }
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(rellenaTableroCo());
    }

    //Realiza lo mismo que cuando se llena por primera vez el tablero,
    //aquí no importa si se realiza el match con la aparición de las
    //nuevas células
    private void rellenaTablero()
    {
        Dictionary<NivelMatch.TipoPrefab, GameObject>.KeyCollection tipoPrefabs =
                    prefabsDic.Keys;

        //Se crea una lista de TipoPrefab para poder elegir de forma 
        //random un valor del diccionario
        List<NivelMatch.TipoPrefab> tipoPrefabsList = new List<NivelMatch.TipoPrefab>();

        foreach (NivelMatch.TipoPrefab t in tipoPrefabs)
        {
            tipoPrefabsList.Add(t);
        }

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                if (tCelulas[i, j] == null)
                {
                    Vector2 posicion = new Vector2(i, j + offset);
                    //int indiceCelula = Random.Range(0, celulas.Length);
                    //GameObject celula = Instantiate(celulas[indiceCelula], posicion, Quaternion.identity);
                    int indiceCelula = Random.Range(0, tipoPrefabsList.Count);
                    GameObject celula = Instantiate(prefabsDic[tipoPrefabsList[indiceCelula]], posicion, Quaternion.identity);

                    celula.transform.parent = this.transform;
                    celula.name = "(" + i + "," + j + ")";
                    tCelulas[i, j] = celula;

                    //Actualización de las células en el tablero, debido a que tiene
                    //una posición incorrecta por el offset
                    celula.GetComponent<Celula>().columna = i;
                    celula.GetComponent<Celula>().fila = j;
                }
            }
        }
    }

    //Función auxiliar que sirve si existe match una vez que descienden
    //las células, retorna un 1 si hay match
    private bool matchesActual()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                if (tCelulas[i, j] != null)
                {
                    if (tCelulas[i, j].GetComponent<Celula>().matched)
                        return true;
                }
            }
        }
        return false;
    }

    //Corutina para rellenar tablero y verificar si existe matches
    //para destruir las células
    private IEnumerator rellenaTableroCo()
    {
        rellenaTablero();
        yield return new WaitForSeconds(0.5f);

        //estadoActual = EstadoJuego.espera;

        while (matchesActual())
        {
            yield return new WaitForSeconds(0.5f);
            destruyeMatches();
        }

        //Para borrar de la lista aquellas células que se convirtieron en
        //powerup
        encuentraMatches.matchesActuales.Clear();
        /*
        yield return new WaitForSeconds(0.5f);
        estadoActual = EstadoJuego.mueve;*/
        //if(estadoActual == EstadoJuego.mueve)
        //Debug.Log(encuentraMatches.encuentraPosiblesMatches());
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
}