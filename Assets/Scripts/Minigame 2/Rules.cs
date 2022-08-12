using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rules : MonoBehaviour
{
    [SerializeField] private Transform parentTMP;//sera el padre de los nombres de los objetos
    [SerializeField] private Transform parentIMG;//sera el padre de los nombres de los valores de los objetos
    [SerializeField] private GameObject TMP;//prefab utilizado para los nombres de los objetos
    [SerializeField] private GameObject IMG;//prefab utilizado para los nombres de los valores de los objetos
    private LevelsMG2 levelActual;
    [HideInInspector] public static Rules instance;//singleton
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;//preparamos la habilitacion del singleton
        levelActual = ReadTXT.instance.levelActual;//le asignamos el scriptableobject
        for (int i = 0; i < levelActual.GetNamesObjects.Count; i++)
        {//instanciamos la cantidad de nombres de objetos que tiene el nivel
            GameObject t = Instantiate(TMP, parentTMP);
            t.GetComponent<TMP_Text>().text = levelActual.GetNamesObjects[i].ToString();
        }
        for (int i = 0; i < levelActual.GetNamesValues.Count; i++)
        {//instanciamos la cantidad de valores de objetos que tiene el nivel
            GameObject nv = Instantiate(IMG, parentIMG);
            nv.transform.GetChild(0).GetComponent<TMP_Text>().text = levelActual.GetNamesValues[i].ToString();
        }
    }
    public void ChangeObjecteable(int originPosition, int targetPosition)
    {//copiamos los valores del code values
        List<int> temp = ReadTXT.instance.levelActual.GetCodeValues;
        int tempOrigin = temp[originPosition];//le asignamos el valor de la posicion inicial
        int tempTarget = temp[targetPosition];//le asignamos el valor de la posicion final
        for (int x = 0; x < temp.Count; x++)
        {//reemplazamos los valores en la copia
            if (x == originPosition)
                temp[x] = tempTarget;
            else if (x == targetPosition)
                temp[x] = tempOrigin;
        }//reemplazamos los valores en el scriptableobject
        ReadTXT.instance.levelActual.SetCodeValues(temp);
    }
    public void ChangeRule(int originPosition, int targetPosition)
    {//intercambiamos los valores de los botones y les aumentamos un valor extra
        for (int i = 0; i < ReadTXT.instance.matrix.GetLength(0); i++)
        {
            for (int j = 0; j < ReadTXT.instance.matrix.GetLength(1); j++)
            {
                if (ReadTXT.instance.matrix[i, j] == levelActual.GetCodeValues[originPosition])
                {
                    ReadTXT.instance.matrix[i, j] = levelActual.GetCodeValues[targetPosition] + 10;
                }
                else if (ReadTXT.instance.matrix[i, j] == levelActual.GetCodeValues[targetPosition])
                {
                    ReadTXT.instance.matrix[i, j] = levelActual.GetCodeValues[originPosition] + 10;
                }
            }
        }//restamos el valor extra para recalcular el valor y asignarle un valor valido
        for (int i = 0; i < ReadTXT.instance.matrix.GetLength(0); i++)
        {
            for (int j = 0; j < ReadTXT.instance.matrix.GetLength(1); j++)
            {

                if (ReadTXT.instance.matrix[i, j] > 10)
                {
                    ReadTXT.instance.matrix[i, j] -= 10;
                }
            }
        }//cambio su valor en el scriptableoject
        ChangeObjecteable(originPosition, targetPosition);
        /*for (int i = 0; i < ReadTXT.instance.matrix.GetLength(0); i++)
        {
            for (int j = 0; j < ReadTXT.instance.matrix.GetLength(1); j++)
            {
                Debug.Log("matriz[" + i + ", " + j + "] = " + ReadTXT.instance.matrix[i, j]);
            }
        }*/
    }
}