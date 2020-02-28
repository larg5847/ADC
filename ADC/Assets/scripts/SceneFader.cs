using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    //public Image img;
    //public AnimationCurve curva;

    /*void Start()
    {
        StartCoroutine(FadeIn());
    }*/

    public void FadeTo(string escena)
    {
        StartCoroutine(FadeOut(escena));
    }

    /*
    IEnumerator FadeIn()
    {
        float t = 1f;

        //Animación de entrada
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curva.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }
    */

    IEnumerator FadeOut(string escena)
    {
        /*float t = 0f;

        //Animación de salida
        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curva.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }*/
        yield return 0;

        SceneManager.LoadScene(escena);
    }
}
