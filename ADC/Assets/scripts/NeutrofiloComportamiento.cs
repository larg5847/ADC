using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutrofiloComportamiento : CelulasBuenasComportamiento
{
    
    // Start is called before the first frame update
    void Start()
    {
        thisAnimtor = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Virus")
        {
            puntaje += 1;
            Score.text = Score.text + " " + puntaje;
        }
    }


}
