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
    Puntaje puntaje;

    public List<GameObject> matchesActuales = new List<GameObject>();

    void Start()
    {
        tablero = FindObjectOfType<Tablero>();
        puntaje = FindObjectOfType<Puntaje>();
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
        if (!matchesActuales.Contains(celula))
        {
            matchesActuales.Add(celula);
            puntaje.AumentaPuntos(1);
        }
    }

    //Método que verifica si hay match en las células que tiene alrededor,
    //esto lo hace comparando por medio de etiquetas
    IEnumerator encuentraTodosLosMatchesCo()
    {
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
        yield return new WaitForSeconds(0.1f);
    }

    //Encuentra todos los posibles matches en el tablero
    public bool encuentraPosiblesMatches()
    {
        //i = 3; i < tablero._ancho - 3
        for(int i = 0; i < tablero._ancho; i++)
        {
            for(int j = 0; j < tablero._alto; j++)
            {
                GameObject celulaActual = tablero._tCelulas[i, j];

                if(celulaActual != null)
                {
                    //Comparación de células ubicadas en las esquinas
                    //Lado izquierdo
                    //Caso 1:
                    //        *
                    //          *
                    //        *
                    if(i > 0 && j > 0 && j < tablero._alto - 1)
                    {
                        if (tablero._tCelulas[i - 1, j - 1] != null
                            && tablero._tCelulas[i - 1, j + 1] != null)
                            if (tablero._tCelulas[i - 1, j - 1].tag == celulaActual.tag
                                && tablero._tCelulas[i - 1, j + 1].tag == celulaActual.tag)
                                return true;
                    }
                    
                    //Caso 2: 
                    //          *
                    //        *
                    //        *
                    if(i > 0 && j > 1)
                    {
                        if(tablero._tCelulas[i - 1, j - 1] != null
                            &&tablero._tCelulas[i - 1, j - 2] != null)
                            if (tablero._tCelulas[i - 1, j - 1].tag == celulaActual.tag
                                && tablero._tCelulas[i - 1, j - 2].tag == celulaActual.tag)
                                return true;
                    }

                    //Caso 3:
                    //            *
                    //        * *
                    if(i > 1 && j > 0)
                    {
                        if(tablero._tCelulas[i - 1, j - 1] != null
                            && tablero._tCelulas[i - 2, j - 1] != null)
                            if (tablero._tCelulas[i - 1, j - 1].tag == celulaActual.tag
                                && tablero._tCelulas[i - 2, j - 1].tag == celulaActual.tag)
                                return true;
                    }

                    //Caso 4:
                    //        *
                    //        *
                    //          *
                    if(i > 0 && j < tablero._alto - 2)
                    {
                        if (tablero._tCelulas[i - 1, j + 1] != null
                            && tablero._tCelulas[i - 1, j + 2] != null)
                            if (tablero._tCelulas[i - 1, j + 1].tag == celulaActual.tag
                                && tablero._tCelulas[i - 1, j + 2].tag == celulaActual.tag)
                                return true;
                    }
                    
                    //Caso 5:
                    //       * *
                    //           *
                    if(i > 1 && j < tablero._alto - 1)
                    {
                        if(tablero._tCelulas[i - 1, j + 1] != null
                            && tablero._tCelulas[i - 2, j + 1] != null)
                            if (tablero._tCelulas[i - 1, j + 1].tag == celulaActual.tag
                                && tablero._tCelulas[i - 2, j + 1].tag == celulaActual.tag)
                                return true;
                    }

                    //Lado derecho
                    //Caso 6:
                    //          *
                    //        *
                    //          *
                    if(i < tablero._ancho - 1 && j > 0 && j < tablero._alto - 1)
                    {
                        if (tablero._tCelulas[i + 1, j - 1] != null
                            && tablero._tCelulas[i + 1, j + 1] != null)
                            if (tablero._tCelulas[i + 1, j - 1].tag == celulaActual.tag
                                && tablero._tCelulas[i + 1, j + 1].tag == celulaActual.tag)
                                return true;
                    }

                    //Caso 7:
                    //        *
                    //          *
                    //          *
                    if(i < tablero._ancho - 1 && j > 1)
                    {
                        if (tablero._tCelulas[i + 1, j - 1] != null
                            && tablero._tCelulas[i + 1, j - 2] != null)
                            if (tablero._tCelulas[i + 1, j - 1].tag == celulaActual.tag
                                && tablero._tCelulas[i + 1, j - 2].tag == celulaActual.tag)
                                return true;
                    }

                    //Caso 8:
                    //        *
                    //          * *
                    if(i < tablero._ancho - 2 && j > 0)
                    {
                        if (tablero._tCelulas[i + 1, j - 1] != null
                            && tablero._tCelulas[i + 2, j - 1] != null)
                        {
                            if (tablero._tCelulas[i + 1, j - 1].tag == celulaActual.tag
                                && tablero._tCelulas[i + 2, j - 1].tag == celulaActual.tag)
                                return true;
                        }
                    }

                    //Caso 9:
                    //          *
                    //          *
                    //        *
                    if(i < tablero._ancho - 1 && j < tablero._alto - 2)
                    {
                        if (tablero._tCelulas[i + 1, j + 1] != null
                            && tablero._tCelulas[i + 1, j + 2] != null)
                            if (tablero._tCelulas[i + 1, j + 1].tag == celulaActual.tag
                                && tablero._tCelulas[i + 1, j + 2].tag == celulaActual.tag)
                                return true;
                    }
                    
                    //Caso 10:
                    //           * *
                    //         *
                    if(i < tablero._ancho - 2 && j < tablero._alto - 1)
                    {
                        if (tablero._tCelulas[i + 1, j + 1] != null
                            && tablero._tCelulas[i + 2, j + 1] != null)
                            if (tablero._tCelulas[i + 1, j + 1].tag == celulaActual.tag
                                && tablero._tCelulas[i + 2, j + 1].tag == celulaActual.tag)
                                return true;
                    }

                    //Caso 11: Derecha
                    //         *   * *
                    if(i < tablero._ancho - 3)
                    {
                        if (tablero._tCelulas[i + 2, j] != null
                        && tablero._tCelulas[i + 3, j] != null)
                            if (tablero._tCelulas[i + 2, j].tag == celulaActual.tag
                                && tablero._tCelulas[i + 3, j].tag == celulaActual.tag)
                                return true;
                    }
                
                    //Caso 12: Izquierda
                    //          * *   *
                    if(i > 2)
                    {
                        if (tablero._tCelulas[i - 2, j] != null
                        && tablero._tCelulas[i - 3, j] != null)
                            if (tablero._tCelulas[i - 2, j].tag == celulaActual.tag
                                && tablero._tCelulas[i - 3, j].tag == celulaActual.tag)
                                return true;
                    }
                    
                    //Caso 13: Arriba
                    //          *
                    //          *
                    //          
                    //          *
                    if(j < tablero._alto - 3)
                    {
                        if (tablero._tCelulas[i, j + 2] != null
                        && tablero._tCelulas[i, j + 3] != null)
                            if (tablero._tCelulas[i, j + 2].tag == celulaActual.tag
                                && tablero._tCelulas[i, j + 3].tag == celulaActual.tag)
                                return true;
                    }

                    //Caso 14: Abajo
                    //          *
                    //
                    //          *
                    //          *
                    if(j > 2)
                    {
                        if (tablero._tCelulas[i, j - 2] != null
                        && tablero._tCelulas[i, j - 3] != null)
                            if (tablero._tCelulas[i, j - 2].tag == celulaActual.tag
                                && tablero._tCelulas[i, j - 3].tag == celulaActual.tag)
                                return true;
                    }
                }
            }
        }
        return false;
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
    public void CrearBombas()
    {
        //Primero se verifica que se hayan movido las células en el tablero
        //Checa si hizo match la célula seleccionada o la que tiene a un lado
        if (tablero.celulaActual != null)
        {
            if (tablero.celulaActual.matched)
            {
                tablero.celulaActual.matched = false;
                /*
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
                */

                //UPDATE: Para no hacerlo random, y mejor crear
                //las bombas dependiendo de hacia dónde se movió
                //la célula para hacer match
                if ((tablero.celulaActual.anguloDeslizamiento > -15
                    && tablero.celulaActual.anguloDeslizamiento <= 15)
                    || (tablero.celulaActual.anguloDeslizamiento > 165
                    || tablero.celulaActual.anguloDeslizamiento <= -165))
                    tablero.celulaActual.creaBombaFila();

                else
                    tablero.celulaActual.creaBombaColumna();

            }

            //Para la célula de al lado, en caso de que se haya seleccionado la que en
            //sí no hace match pero la que se intercambia sí y no se vea afectado por ello
            else if (tablero.celulaActual._celula != null)
            {
                Celula celula = tablero.celulaActual._celula.GetComponent<Celula>();

                if (celula.matched)
                {
                    celula.matched = false;
                    /*
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
                    */

                    //UPDATE: Para no hacerlo random, y mejor crear
                    //las bombas dependiendo de hacia dónde se movió
                    //la célula para hacer match
                    if ((tablero.celulaActual.anguloDeslizamiento > -15
                    && tablero.celulaActual.anguloDeslizamiento <= 15)
                    || (tablero.celulaActual.anguloDeslizamiento > 165
                    || tablero.celulaActual.anguloDeslizamiento <= -165))
                        celula.creaBombaFila();

                    else
                        celula.creaBombaColumna();
                }
            }
        }
    }

    //Se pasa la etiqueta de la célula para poner la variable
    //match en true de todas las células que se encuentren en el
    //tablero con dicha etiqueta (sería el powerUp que elimina
    //el mismo tipo de gameobject)
    public void CelulasMatchPorTipo(string etiquetaCelula)
    {
        for(int i = 0; i < tablero._ancho; i++)
        {
            for(int j = 0; j < tablero._alto; j++)
            {
                if(tablero._tCelulas[i, j] != null)
                {
                    if(tablero._tCelulas[i, j].tag == etiquetaCelula)
                    {
                        tablero._tCelulas[i, j].GetComponent<Celula>().matched = true;
                    }
                }
            }
        }
    }
}