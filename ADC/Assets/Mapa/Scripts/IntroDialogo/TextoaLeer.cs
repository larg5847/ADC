using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoaLeer : MonoBehaviour {

	//Solo para extraer el texto, para no meter un Switch o miles de IF
	public string textoIng;
	public string textoEsp;

	public bool activarMensaje;
	public MostrarTextodeObjetos scripMostrarTexto;//Enlazamos el scrip pa acceder a sus var

	//private SphereCollider esferaCol;

	//private Vector3 inicio;//Pa capturar su posicion

	//Detectar la colicion
	//void OnTriggerEnter2D()
	void OnTriggerEnter()
	{//Deshabilitado x q queremos q choque el mono
	//void OnCollisionEnter2D (){
		activarMensaje = true;
	}
	//void OnTriggerExit2D()
	void OnTriggerExit()
	{//Deshabilitado x q queremos q choque el mono
	//void OnCollisionExit2D (){
		activarMensaje = false;
	}
	void OnGUI () {
		if (activarMensaje && !scripMostrarTexto.escribir) {
			GUI.Label (new Rect (Screen.width-100,Screen.height-25,100,50), "Presiona X");
			//GUI.Label (new Rect (1.63f,-3.28f,100,50), "Presiona X");
			//GUILayout.Label ("Presiona X");
			}
	}

}
