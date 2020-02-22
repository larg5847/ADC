using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorMiniJuego : MonoBehaviour
{
    public SceneFader fader;

    public void Seleccion (string nombreMiniJuego)
    {
        fader.FadeTo(nombreMiniJuego);
    }
}
