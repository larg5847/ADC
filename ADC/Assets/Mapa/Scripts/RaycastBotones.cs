using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastBotones : MonoBehaviour
{
    public float rayoLargo;
    public LayerMask capaMascara;

    public GameObject PantallaCarga;
    public GameObject DeshabilitarEntorno;

    private void Update()
    {
     if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit golpe;
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(rayo.origin, rayo.direction * rayoLargo, Color.red);//Ver el rayo
            if (Physics.Raycast(rayo, out golpe, rayoLargo, capaMascara))
            {
                Debug.Log(golpe.collider.name);
                if (golpe.collider.tag == "CargarNivel")
                {
                    golpe.collider.GetComponent<nombreNiveles>().crearInterfazNivel();
                    return;
                }
            }
        }  
    }
    public void cargarNivel(string nombreNivel)
    {
        DeshabilitarEntorno.SetActive(false);
        PantallaCarga.SetActive(true);
        PantallaCarga nivel = PantallaCarga.GetComponent<PantallaCarga>();
        nivel.darEscenaACargar(nombreNivel);
    }
}
