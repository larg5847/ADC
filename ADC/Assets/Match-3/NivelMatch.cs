using System;
using UnityEngine;

//Para definir los distintos escenarios, en un futuro para crear
//nuevos niveles
[CreateAssetMenu(fileName = "Nivel", menuName = "ScriptableObject/NivelMatch", order = 1)]
public class NivelMatch : ScriptableObject
{
    public enum TipoPrefab { VACIO, MONOCITO, NEUTROFILO, ADENOVIRUS}

    [Serializable] public struct Prefabs
    {
        public TipoPrefab tipoPrefab;
        public GameObject prefab;
    }

    public string nombreNivel;
    public int ancho;
    public int alto;
    public Prefabs[] prefabs;
}
