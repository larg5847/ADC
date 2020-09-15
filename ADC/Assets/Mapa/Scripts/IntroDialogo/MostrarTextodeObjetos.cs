using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarTextodeObjetos : MonoBehaviour {

	// El component Text
	public Text cajaTexto;
	public bool escribir=false;
	public Animator anim;
	//public JugadorControles scripJugadorControles;
	bool presTecla;

	//public GameObject panel1;//Enlaza al panel 1
	//public GameObject panel2;//Este al dos
	
	//Solo para extraer el texto, para no meter un Switch o miles de IF
	public string textoEsp;
	public string textoIng;

	//private PolygonCollider2D Pc2d;//Rdtr pal poligono d colision
	//private CircleCollider2D Cc2d;
	void Start()
    {
        //anim["Walk"].enabled = false;
        //anim["Walk"].weight = 1.0f;
		anim.enabled = false;
		DialogoIntro();
    }

	//Checamos colisiones
	//void OnCollisionStay2D (Collision2D other)
	//void OnTriggerStay2D(Collider2D other)//Esta funcion lee la colision cuadro x cuadro
	//void OnTriggerStay(Collider other)//Uso en 3D
	public void DialogoIntro()
	{
		//TextoaLeer temp = other.gameObject.GetComponent<TextoaLeer> ();//almacena el codigo del scrip TextoaLeer

		//Checamos si con lo que colisionamos es un ObjetoConTexto
		//if (other.gameObject.tag == "ObjetoConTexto" && !escribir && temp.activarMensaje) 
		//{
			
			if (escribir == false) {//Si pulsamos X o la tecla asignada corre el codigo
			escribir = true;//Pa q no cambie de texto asta q acabe
			//scripJugadorControles.movimientos = false; //Desactivamos mov
				//escalaPanel () ;//Llamamos al que escale los paneles
			//Mandamos el string escribir Texto a nuestra funcion
			//Selecciona el idioma del dispositivo y es el que muestra
				switch (Application.systemLanguage) {
				case SystemLanguage.Spanish:
					StartCoroutine (AnimateText (textoEsp));
					break;
				case SystemLanguage.English:
					StartCoroutine (AnimateText (textoIng));
					break;
				default://Pondre el ingles x defaul, esto pa los extranjeros q se les facilita mas el ingles
					StartCoroutine (AnimateText (textoIng));
					break;
				}
			}
			//Destroy (other.gameObject);
		//}
	}
	//Animacion de escritura del texto a mostrar
	IEnumerator AnimateText(string strComplete)
	{
		//Definimos el limite del loop While, lo ponemos en 0
		int i = 0;
		//Nuestro string lo resetamos, este es el que va a cambiar el texto
		string str = "";
		char letraXletra = 'i';
		//Un loop que va sumando de 1 a uno la letra del texto completo
		while (i < strComplete.Length) 
		{	
			//La variable str va agnadiendo una letra a la vez
			//str += strComplete [i++];
			letraXletra = strComplete [i++];
			str += letraXletra;
			//Modificamos el texto de la interfaz de usuario y lo asignamos a la variables str
			cajaTexto.text = str;
			if (letraXletra == '.'){anim.enabled = true;}
			//Esperamos para dar el efecto
			yield return new WaitForSeconds (0.1F);
		}

		//Esperamos un momento mas para resetear la UI
		yield return new WaitForSecondsRealtime (1);
		//cajaTexto.text = "";

		escribir = false;
		Destroy (gameObject);
		//scripJugadorControles.movimientos = true; //Desactivamos mov
		//desescalaPanel ();//Restaura la escala de los panels
	}
	/*void escalaPanel () {
		//GUI.Label (new Rect (0,Screen.height-25,100,50), "Panel jalaperro");
		panel1.transform.localScale = new Vector3 (1, 1, 1);
		panel2.transform.localScale = new Vector3 (1, 1, 1);
	}
	void desescalaPanel () {
		//GUI.Label (new Rect (0,Screen.height-25,100,50), "Panel jalaperro");
		panel1.transform.localScale = new Vector3 (0, 0, 1);
		panel2.transform.localScale = new Vector3 (0, 0, 1);
	}*/
}
