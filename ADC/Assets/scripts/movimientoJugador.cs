using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientoJugador : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moverJugador();   
    }

    void moverJugador()
    {
        transform.Translate(Input.acceleration.x + 3 * Time.deltaTime, 0, 0);
    }
}
