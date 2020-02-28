using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorMiniJuego : MonoBehaviour
{
    SceneFader fader;

    public void Seleccion (string nombreMiniJuego)
    {
        fader = gameObject.GetComponentInParent<SceneFader>();
        fader.FadeTo(nombreMiniJuego);
    }
}
