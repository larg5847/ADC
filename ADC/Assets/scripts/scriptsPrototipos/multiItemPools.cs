using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiItemPools : MonoBehaviour
{

    [SerializeField]
    GameObject[] prefabs;

    List<GameObject>[] pools;
    int tamPools = 10;


    // Start is called before the first frame update
    void Start()
    {
        createPools();
        createObjects();
    }

    // Update is called once per frame
    void Update()
    {
        spawnDestruct();
    }

    void createPools()
    {
        pools = new List<GameObject>[prefabs.Length];
        for(int i =0;i<pools.Length;i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    void createObjects()
    {
        int j = 0;
        foreach (List<GameObject> k in pools)
        {
            for (int i = 0; i < tamPools; i++)
            {
                GameObject obj = (GameObject)Instantiate(prefabs[j]);
                obj.SetActive(false);
                pools[j].Add(obj);
            }
            j++;
        }
    }

    void spawnDestruct()
    {
        int elecionDePool = 0;
        elecionDePool = Random.Range(0, 2);  //el pool de objetos del cual aparecera
        //el transform de posicion del objeto




    }
}
