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
        transform.Translate(Input.acceleration.x * Time.deltaTime,0,0);

        if(Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(transform.position.x * 3 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(-transform.position.x * 3 * Time.deltaTime, 0, 0);
        }
    }
}
