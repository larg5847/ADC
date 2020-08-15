using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Este script equivale al método de encuentraMatches() del script
//Celula, esto para evitar que haga lo mismo para cada célula, y se
//generaliza en un solo gameobject que contiene a todas las células
public class EncuentraMatches : MonoBehaviour
{
    Tablero tablero;

    public List<GameObject> matchesActuales = new List<GameObject>();

    void Start()
    {
        tablero = FindObjectOfType<Tablero>();
    }

    public void encuentraTodosLosMatches()
    {
        StartCoroutine(encuentraTodosLosMatchesCo());
    }

    //Método para agregar a la lista (si no lo está) con la
    //finalidad de saber el total de matches, y posterior a
    //realizar una máquina de estados sobre qué powerups crear
    void agregaMatchesActuales(GameObject celula)
    {
        if(!matchesActuales.Contains(celula))
        {
            matchesActuales.Add(celula);
        }
    }

    //Método que verifica si hay match en las células que tiene alrededor,
    //esto lo hace comparando por medio de etiquetas
    IEnumerator encuentraTodosLosMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);

        for(int i = 0; i < tablero._ancho; i++)
        {
            for(int j = 0; j < tablero._alto; j++)
            {
                GameObject celulaActual = tablero._tCelulas[i, j];

                if(celulaActual != null)
                {
                    if(i > 0 && i < tablero._ancho - 1)
                    {
                        //Células a los lados izquierda y derecha
                        GameObject celulaIzquierda = tablero._tCelulas[i - 1, j];
                        GameObject celulaDerecha = tablero._tCelulas[i + 1, j];

                        //Checa que las células de alrededor no estén vacías
                        if(celulaIzquierda != null && celulaDerecha != null)
                        {
                            if(celulaIzquierda.tag == celulaActual.tag && celulaDerecha.tag == celulaActual.tag)
                            {
                                //Agrega todas las células de la fila a la lista de matches actuales
                                //para luego destruir
                                if(celulaIzquierda.GetComponent<Celula>().bombaFila
                                    || celulaActual.GetComponent<Celula>().bombaFila
                                    || celulaDerecha.GetComponent<Celula>().bombaFila)
                                {
                                    matchesActuales.Union(obtieneCelulasFila(j));
                                }

                                if(celulaIzquierda.GetComponent<Celula>().bombaColumna)
                                    matchesActuales.Union(obtieneCelulasColumna(i - 1));

                                if (celulaActual.GetComponent<Celula>().bombaColumna)
                                    matchesActuales.Union(obtieneCelulasColumna(i));
                                
                                if (celulaDerecha.GetComponent<Celula>().bombaColumna)
                                    matchesActuales.Union(obtieneCelulasColumna(i + 1));

                                agregaMatchesActuales(celulaIzquierda);
                                agregaMatchesActuales(celulaActual);
                                agregaMatchesActuales(celulaDerecha);
                                celulaIzquierda.GetComponent<Celula>().matched = true;
                                celulaActual.GetComponent<Celula>().matched = true;
                                celulaDerecha.GetComponent<Celula>().matched = true;
                            }
                        }
                    }
                }

                if (j > 0 && j < tablero._alto - 1)
                {
                    GameObject celulaAbajo = tablero._tCelulas[i, j - 1];
                    GameObject celulaArriba = tablero._tCelulas[i, j + 1];

                    if (celulaAbajo != null && celulaArriba != null)
                    {
                        if (celulaAbajo.tag == celulaActual.tag && celulaArriba.tag == celulaActual.tag)
                        {
                            //Agrega todas las células de la columna a la lista de matches actuales
                            //para luego destruir
                            if(celulaAbajo.GetComponent<Celula>().bombaColumna
                                || celulaActual.GetComponent<Celula>().bombaColumna
                                || celulaArriba.GetComponent<Celula>().bombaColumna)
                            {
                                matchesActuales.Union(obtieneCelulasColumna(i));
                            }

                            if (celulaAbajo.GetComponent<Celula>().bombaFila)
                                matchesActuales.Union(obtieneCelulasFila(j - 1));

                            if (celulaActual.GetComponent<Celula>().bombaFila)
                                matchesActuales.Union(obtieneCelulasFila(j));

                            if (celulaArriba.GetComponent<Celula>().bombaFila)
                                matchesActuales.Union(obtieneCelulasFila(j + 1));

                            agregaMatchesActuales(celulaAbajo);
                            agregaMatchesActuales(celulaActual);
                            agregaMatchesActuales(celulaArriba);
                            celulaAbajo.GetComponent<Celula>().matched = true;
                            celulaActual.GetComponent<Celula>().matched = true;
                            celulaArriba.GetComponent<Celula>().matched = true;
                        }
                    }
                }
            }
        }
    }

    //Método para la creación de una lista que contenga a todos los
    //gameobjects de una columna
    List<GameObject> obtieneCelulasColumna(int columna)
    {
        List<GameObject> celulas = new List<GameObject>();

        for(int i = 0; i < tablero._alto; i++)
        {
            if(tablero._tCelulas[columna, i] != null)
            {
                celulas.Add(tablero._tCelulas[columna, i]);
                tablero._tCelulas[columna, i].GetComponent<Celula>().matched = true;
            }
        }

        return celulas;
    }

    //Método para la creación de una lista que contenga a todos los
    //gameobjects de una fila
    List<GameObject> obtieneCelulasFila(int fila)
    {
        List<GameObject> celulas = new List<GameObject>();

        for (int i = 0; i < tablero._ancho; i++)
        {
            if (tablero._tCelulas[i, fila] != null)
            {
                celulas.Add(tablero._tCelulas[i, fila]);
                tablero._tCelulas[i, fila].GetComponent<Celula>().matched = true;
            }
        }

        return celulas;
    }

    //Método que checa si existe lo necesario para crear una bomba
    //ya sea para eliminar una fila o columna
    public void verificaBombas()
    {
        //Primero se verifica que se hayan movido las células en el tablero
        //Checa si hizo match la célula seleccionada o la que tiene a un lado
        if(tablero.celulaActual != null)
        {
            if(tablero.celulaActual.matched)
            {
                tablero.celulaActual.matched = false;
                int tipoBomba = Random.Range(0, 1);
                
                //Bomba fila
                if(tipoBomba == 0)
                {
                    tablero.celulaActual.creaBombaFila();
                }

                //Bomba columna
                else
                {
                    tablero.celulaActual.creaBombaColumna();
                }
            }

            //Para la célula de al lado, en caso de que se haya seleccionado la que en
            //sí no hace match pero la que se intercambia sí y no se vea afectado por ello
            else if(tablero.celulaActual._celula != null)
            {
                Celula celula = tablero.celulaActual._celula.GetComponent<Celula>();

                if(celula.matched)
                {
                    celula.matched = false;
                    int tipoBomba = Random.Range(0, 1);

                    //Bomba fila
                    if (tipoBomba == 0)
                    {
                        celula.creaBombaFila();
                    }

                    //Bomba columna
                    else
                    {
                        celula.creaBombaColumna();
                    }
                }
            }
        }
    }
}
