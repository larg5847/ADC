using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nombreNiveles : MonoBehaviour
{
    public string nombreNivel;
    public GameObject Interfaz;

    public void crearInterfazNivel()
    {
        Interfaz.SetActive(true);
        BotonInterfazNiveles activar = Interfaz.GetComponent<BotonInterfazNiveles>();
        activar.darNombreNivel(nombreNivel);
    }
}
