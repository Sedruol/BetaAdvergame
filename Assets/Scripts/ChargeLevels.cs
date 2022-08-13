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
    [SerializeField] private Button btnLevel4;
    [SerializeField] private Button btnLevel5;
    [SerializeField] private Button btnLevel6;
    [SerializeField] private Button btnLevel7;
    [SerializeField] private Button btnLevel8;
    [SerializeField] private Button btnLevel9;
    [SerializeField] private Button btnLevel10;
    [SerializeField] private Button btnLevel11;
    private int numberLevel = 1;//usaremos este indice para saber que nivel se esta usando

    // Start is called before the first frame update
    void Start()
    {
        btnLevel1.onClick.AddListener(() => ChargeLevel(numberLevel = 1));//se carga el nivel 1
        btnLevel2.onClick.AddListener(() => ChargeLevel(numberLevel = 2));//se carga el nivel 2
        btnLevel3.onClick.AddListener(() => ChargeLevel(numberLevel = 3));//se carga el nivel 3
        btnLevel4.onClick.AddListener(() => ChargeLevel(numberLevel = 4));//se carga el nivel 4
        btnLevel5.onClick.AddListener(() => ChargeLevel(numberLevel = 5));//se carga el nivel 5
        btnLevel6.onClick.AddListener(() => ChargeLevel(numberLevel = 6));//se carga el nivel 6
        btnLevel7.onClick.AddListener(() => ChargeLevel(numberLevel = 7));//se carga el nivel 7
        btnLevel8.onClick.AddListener(() => ChargeLevel(numberLevel = 8));//se carga el nivel 8
        btnLevel9.onClick.AddListener(() => ChargeLevel(numberLevel = 9));//se carga el nivel 9
        btnLevel10.onClick.AddListener(() => ChargeLevel(numberLevel = 10));//se carga el nivel 10
        btnLevel11.onClick.AddListener(() => ChargeLevel(numberLevel = 11));//se carga el nivel 11
    }
    public void ChargeLevel(int _level)
    {
        PlayerPrefs.SetInt("level", _level);
        SceneManager.LoadScene("MiniGame2");
    }
}