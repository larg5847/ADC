using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
