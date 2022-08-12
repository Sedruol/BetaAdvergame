using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChargeLevels : MonoBehaviour
{
    [SerializeField] private Button btnLevel1;
    [SerializeField] private Button btnLevel2;
    [SerializeField] private Button btnLevel3;
    public static ChargeLevels instance;//singleton
    private int numberLevel = 1;//usaremos este indice para saber que nivel se esta usando
    public int GetNumberLevel() { return numberLevel; }//get
    public int SetNumberLevel(int _numberLevel) { return numberLevel = _numberLevel; }//set
    public int NextLevel() { return numberLevel++; }//se usa para avanzar al siguiente nivel de forma consecutiva

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        btnLevel1.onClick.AddListener(() => ChargeLevel(numberLevel = 1));//se carga el nivel 1
        btnLevel2.onClick.AddListener(() => ChargeLevel(numberLevel = 2));//se carga el nivel 2
        btnLevel3.onClick.AddListener(() => ChargeLevel(numberLevel = 3));//se carga el nivel 3
    }
    public void ChargeLevel(int _level)
    {
        //SetNumberLevel(_level);
        SceneManager.LoadScene("MiniGame2");
    }
}