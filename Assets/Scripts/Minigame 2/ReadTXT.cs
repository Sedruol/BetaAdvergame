using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReadTXT : MonoBehaviour
{
    public List<LevelsMG2> listLevelsMG2;//lista con los scriptableobjects
    [SerializeField] private Tile tileWin;//tile usado para ganar
    public List<Tile> tiles;
    [SerializeField] private Tilemap tilemapObjects;//tilemap para objetos
    [HideInInspector] public LevelsMG2 levelActual;
    [HideInInspector] public int[,] matrix = new int[6, 7];//se crea matriz de 6x7
    [HideInInspector] public static ReadTXT instance;//se crea instancia para el singleton
    [HideInInspector] public int cantUses = 0;//almacena cantidad de usos
    private float time = 0;//almacena los segundos transcurridos

    public void ReadFromTheFile()
    {//leemos la matriz del txt
        string[] lines = levelActual.GetTextAsset.text.Split('\n');//almacena las lineas
        for (int i = 0; i < matrix.GetLength(0); i++)
        {//separamos los numeros por espacios
            string[] vector = lines[i].Split(',');
            for (int j = 0; j < matrix.GetLength(1); j++)
            {//convertimos los valores del txt a int y los asignamos a la matriz
                int.TryParse(vector[j], out matrix[i, j]);
                //dependiendo el valor de la matriz le asignamos su tile respectivo
                if (matrix[i, j] == 2)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[0]);
                else if (matrix[i, j] == 3)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[1]);
                else if (matrix[i, j] == 4)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[2]);
                else if (matrix[i, j] == 5)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[3]);
                else if (matrix[i, j] == 6)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[4]);
                else if (matrix[i, j] == 7)
                    tilemapObjects.SetTile(new Vector3Int(-8 + j, 2 - i, 0), tiles[5]);
                //Debug.Log("matriz[" + i + ", " + j + "] = " + matrix[i, j]);
            }
        }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;//preparamos la habilitacion del singleton
        levelActual = listLevelsMG2[PlayerPrefs.GetInt("level", 1) - 1];
        List<int> temp = ReadTXT.instance.levelActual.GetCodeValues;
        for (int i = 0; i < ReadTXT.instance.levelActual.GetNamesValues.Count; i++)
        {//asignamos el code values en base al nombre del valor del objeto editable escogido
            if (ReadTXT.instance.levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Ganar)
                temp[i] = 2;
            else if (ReadTXT.instance.levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Perder)
                temp[i] = 3;
            else if (ReadTXT.instance.levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Detiene)
                temp[i] = 4;
            else if(ReadTXT.instance.levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Empujable)
                temp[i] = 5;
            else if(ReadTXT.instance.levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Flotar)
                temp[i] = 6;
            else if (ReadTXT.instance.levelActual.GetNamesValues[i] == LevelsMG2.RuleObjects.Destruir)
                temp[i] = 7;
        }
        levelActual.SetCodeValues(temp);
        ReadFromTheFile();
        cantUses = 0;
        //segunda opcion del contador
        InvokeRepeating("Contador", 0f, 1f);
    }
    private void Contador()
    {
        time++;
        //Debug.Log("segundos transcurridos: " + time);
    }
    private void Update()
    {
        /*time += Time.deltaTime;
        Debug.Log("segundos transcurridos: " + time.ToString("f0"));*/
        //mostrar cantidad de usos
        //Debug.Log("usos: " + cantUses);
    }
}