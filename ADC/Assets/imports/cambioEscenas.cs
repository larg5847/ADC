using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class cambioEscenas : MonoBehaviour
{

    //public GameObject pantallaCarga;
    //public Slider barrita;

    public void CargarNivel(int indexDeEscena)
    {
        StartCoroutine(cargaAsincrona(indexDeEscena));
    }

    IEnumerator cargaAsincrona(int numeroEscena)
    { 
        AsyncOperation operacion = SceneManager.LoadSceneAsync(numeroEscena);

        //pantallaCarga.SetActive(true);


        /*while(!operacion.isDone)
        {

            float progreso = Mathf.Clamp01(operacion.progress / .9f);
            barrita.value = progreso;
            
        }*/
        yield return null;
    }
}
