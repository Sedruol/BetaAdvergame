using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseParent : MonoBehaviour
{
    public Button btnCant;//el botón que tiene el contador del loop
    public TMP_Text txtCant;//el text del botón
    [HideInInspector]
    public int cant;//el valor del contador del loop
    // Start is called before the first frame update
    void Start()
    {
        cant = 2;
        btnCant.onClick.AddListener(() => CantLoopFunc());//le asignamos la función al botón
        this.transform.GetChild(0).SetParent(this.transform.parent);//el bloque de tipo "loop end" lo volvemos hijo del objeto algoritmo
    }

    public void CantLoopFunc()
    {//se cambia el contador dependiendo el valor de cant
        if (cant >= 2 && cant < 5)
            cant++;
        else if (cant >= 5)
            cant = 2;
        txtCant.text = cant.ToString();
    }
}
