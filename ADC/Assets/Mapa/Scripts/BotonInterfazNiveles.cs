using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonInterfazNiveles : MonoBehaviour
{
    string NombreNivel;
    public Text nombreInterfazNivel;

    public void darNombreNivel(string nombre)
    {
        nombreInterfazNivel.text = nombre;
        NombreNivel = nombre;
    }
    public void activarCargaNivel()
    {
        if (GameObject.Find("Main Camera"))
        {
            RaycastBotones activar = GameObject.Find("Main Camera").GetComponent<RaycastBotones>();
            activar.cargarNivel(NombreNivel);
        }
    }
}
