using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruyeVirus : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("destruye", 10.0f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monocito" || collision.gameObject.tag == "Neutrofilo")
            destruye();
    }

    //Método que desactiva el objeto
    private void destruye()
    {
        this.gameObject.SetActive(false);
    }
}
