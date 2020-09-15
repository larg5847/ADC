using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaCarga : MonoBehaviour
{
    //Esta es la forma correcta de mostrar variables privadas en el inspector. 
	//No se deben hacer public variables que no queremos sean accesibles desde otras clases-
	[SerializeField]
	private string EscenaACargar;

	[SerializeField]
	private Text porcentajeText;

	[SerializeField]
	private Image progresoImagen;

	public void darEscenaACargar(string escena)
    {
		EscenaACargar = escena;
    }
	// En cuanto se active el objeto, se inciará el cambio de escena
	void Start () {
		//Iniciamos una corrutina, que es un método que se ejecuta 
		//en una línea de tiempo diferente al flujo principal del programa
		StartCoroutine(LoadScene());
	}

	//Corrutina
	IEnumerator LoadScene()
	{
		AsyncOperation cargando;

		//Iniciamos la carga asíncrona de la escena y guardamos el proceso en 'cargando'
		cargando = SceneManager.LoadSceneAsync(EscenaACargar, LoadSceneMode.Single);

		//Bloqueamos el salto automático entre escenas para asegurarnos el control durante el proceso
		cargando.allowSceneActivation = false;

		//Cuando la escena llega al 90% de carga, se produce el cambio de escena
		while (cargando.progress < 0.9f) {
			
			//Actualizamos el % de carga de una forma optima 
			//(concatenar con + tiene un alto coste en el rendimiento)
			porcentajeText.text = string.Format ("{0}%", cargando.progress * 100);

			//Actualizamos la barra de carga
			progresoImagen.fillAmount = cargando.progress;

			//Esperamos un frame
			yield return null;
		}

		//Mostramos la carga como finalizada
		porcentajeText.text = "100%";
		progresoImagen.fillAmount = 1;

		//Activamos el salto de escena.
		cargando.allowSceneActivation = true;


	}
}
