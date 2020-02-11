using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Cronometro : MonoBehaviour
{
    public int min;
    public int sec;

    int m, s;

    public Text timerText;

    void Start()
    {
        timerText = GetComponent<Text>();
        startTimer();
    }

    public void startTimer()
    {
        m = min;
        s = sec;

        writeTimer(m, s);
        Invoke("updateTimer", 1f);
    }

    public void stopTimer()
    {
        CancelInvoke();
        s = 0;
        m = 0;
        CancelInvoke("updateTimer");
    }

    private void updateTimer()
    {
        s--;
        if (s < 0)
        {
            if (m == 0)
            {
                //Terminar el juego
                stopTimer();


                SceneManager.LoadScene(0);
                //activar aqui a negro


            }
            else
            {
                m--;
                s = 59;
            }
        }

        if (s == 10)
        {
            timerText.color = Color.red;
        }

        writeTimer(m, s);
        Invoke("updateTimer", 1f);
    }

    private void writeTimer(int m, int s)
    {
        if (s < 10)
        {
            timerText.text = m.ToString() + ":0" + s.ToString();
        }
        else
        {
            timerText.text = m.ToString() + ":" + s.ToString();
        }
    }

}
