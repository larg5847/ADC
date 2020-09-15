using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posicioParticulas : MonoBehaviour
{
    RectTransform rectTransform;
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Main Camera"))
        {
            Transform guiaCamara = GameObject.Find("Main Camera").GetComponent<Transform>();
            rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition += new Vector3(0, 0, (guiaCamara.transform.position.z + 17)- rectTransform.localPosition.z);
        }
    }
}
