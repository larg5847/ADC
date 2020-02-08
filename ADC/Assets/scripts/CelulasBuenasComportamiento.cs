
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class CelulasBuenasComportamiento : MonoBehaviour
{
    protected Animator thisAnimtor;
    protected Text Score;
    protected int puntaje = 0;
    // Start is called before the first frame update
    void Start()
    {
        Score = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
