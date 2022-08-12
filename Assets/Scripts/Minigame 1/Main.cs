using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{
    public enum BlockType
    {
        Function,
        LoopStart,
        LoopEnd,
        Up,
        Down,
        Right,
        Left
    }
    public static Main instance;
    public Button btnExecute;
    public List<Button> ListButtons = new List<Button>(); //se almacenan los botones de la izquierda
    public List<GameObject> ListCode = new List<GameObject>(); //se almacenan los prefabs de los bloques
    public Transform parent; //se almacena el gameobject algorithm para que se vuelva padre de los bloques
    private int timeLoop = 2; //cantidad de repeticiones del loop
    [HideInInspector]
    public List<BlockType> ListBlock = new List<BlockType>(); //se almacenan los tipos de bloques de los hijos de algorithm
    private List<int> ListValues = new List<int>(); //almacena el valor de la cantidad de usos de cada botón
    private int codeDestroyed; //index del objeto destruido
    private float cantAlgorithm; //cantidad de hijos de algorithm
    private bool existsLoop = false;
    private bool failUseLoop = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnExecute.onClick.AddListener(() => ExecuteCode());
        codeDestroyed = 0;
        cantAlgorithm = 0f;
        //le asignamos valores aleatorios a la cantidad de usos de cada botón
        for (int i = 0; i < ListButtons.Count; i++)
        {
            ListValues.Add(Random.Range(1, 4));
            ListButtons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = ListValues[i].ToString();
        }
    }
    public void LengthAlgorithm() //calculamos el tamaño del algoritmo
    {
        cantAlgorithm = 0;
        for (int i = 0; i < parent.childCount; i++)
        {//usando los 2 tipos de bloques
            /*if (parent.transform.GetChild(i).GetComponent<Block>().blockType.Equals(BlockType.LoopStart)
                || parent.transform.GetChild(i).GetComponent<Block>().blockType.Equals(BlockType.LoopEnd))
                cantAlgorithm += 0.5f;*/
            if(parent.transform.GetChild(i).GetComponent<Block>().blockType.ToString().Contains("Loop"))
                cantAlgorithm += 0.5f;//convirtiendo el tipo de bloque a string y ver que tenga loop
            /*if (parent.transform.GetChild(i).gameObject.layer == 7)
                cantAlgorithm += 0.5f;*/ //usando layers
            else
                cantAlgorithm++;
        }
        Debug.Log(cantAlgorithm);
    }
    public void UseButtons(int x)
    {
        //LengthAlgorithm();
        if (ListValues[x] > 0 && cantAlgorithm <= 5)//el botón aún tiene uso y aún hay espacio en el algoritmo
        {
            ListValues[x]--;//el contador del botón se reduce en 1 y se modifica su text
            ListButtons[x].transform.GetChild(0).GetComponent<TMP_Text>().text = ListValues[x].ToString();
            Instantiate(ListCode[x], parent.transform);//se crea el objeto relacionado al botón usado
            LengthAlgorithm();//se calcula el tamaño del algoritmo
            CreateListBlock();//realizamos la lista de bloques
        }
        else if (ListValues[x] <= 0 && cantAlgorithm > 5)//no le quedan usos y no hay espacio en el algoritmo
            Debug.Log("no hay más usos ni espacio en el algoritmo papu ");
        else if (parent.childCount > 5)//no hay más espacio en el algoritmo
            Debug.Log("no hay más espacio para el algoritmo papu");
        else//no hay más usos disponibles por el botón
            Debug.Log("no hay más usos disponibles para ese botón papu");
    }
    public void RecuperateButtons()
    {
        if (Globals.enableDestroy == true)//se ha destruido un bloque?
        {
            Globals.enableDestroy = false;
            codeDestroyed = Globals.indexBlockType;//almacenamos el index del objeto destruido
            cantAlgorithm--;//la cantidad del algoritmo se reduce en 1
            ListValues[codeDestroyed]++;//el contador del botón respectivo aumenta en 1
            ListButtons[codeDestroyed].transform.GetChild(0).GetComponent<TMP_Text>().text =
                ListValues[codeDestroyed].ToString();
            //CreateListBlock(); //no se porque sale error cuando llamó a la función por aquí
            //sale null reference y no se donde falta la referencia, ya que este error no ocurre cuando lo llamo en 
            //otras funciones
        }
    }
    public void CreateListBlock()//se usa para crear la lista de bloques
    {
        Debug.Log("tamaño de la lista 1: " + ListBlock.Count);
        ListBlock.Clear();//se limpia la lista de bloques
        for (int i = 0; i < parent.childCount; i++)//añadimos cada tipo de bloque en la lista de bloques
        {
            ListBlock.Add(parent.GetChild(i).GetComponent<Block>().blockType);
        }
        Debug.Log("tamaño de la lista 2: " + ListBlock.Count);
    }
    public void ExecuteCode()
    {
        CreateListBlock();
        ValidateLoop();//revisamos si hay loop
    }
    public void ValidateLoop()
    {
        for (int i = 0; i < ListBlock.Count; i++)
        {
            for (int j = 0; j < ListBlock.Count; j++)
            {
                if (ListBlock[i].Equals(BlockType.LoopEnd) && ListBlock[j].Equals(BlockType.LoopStart))
                {//se esta usando loops
                    if (i > j)//el loop se está usando correctamente
                    {
                        existsLoop = true;
                        timeLoop = parent.GetChild(j).GetComponent<LoseParent>().cant;//cuantas veces se ejecuta el loop
                        Debug.Log("i:" + i + ", j:" + j);
                        ApplicateCode(i, j);//ejecutamos las reglas de los bloques
                    }
                    else//el loop se está usando incorrectamente
                    {
                        failUseLoop = true;
                        Debug.Log("esta mal tu loop papu");
                    }
                }
            }
        }//valida que no hay loops y si es que no hay, lo ejecuta
        if (failUseLoop == false)
            ApplicateCodeWithoutLoop();
        failUseLoop = false;
    }
    public void ApplicateCodeWithoutLoop()
    {//si es q no hay loop ejecutamos las reglas de cada bloque de forma normal
        if (existsLoop == false)
        {
            for(int x = 0; x < ListBlock.Count; x++)
            {
                RulesCode(x);
            }
        }
        existsLoop = false;
    }
    public void ApplicateCode(int i, int j)
    {
        for (int x = 0; x < ListBlock.Count; x++)
        {
            if (x < j || x > i)
            {
                Debug.Log("1x:" + x);
                RulesCode(x);
            }
            else if (x > j && x < i)
            {
                for (int y = 0; y < timeLoop * (i - j - 1); y++)
                {
                    Debug.Log("dentro de loop" + timeLoop + ", 2x:" + x);
                    RulesCode(x);
                    if (x + 1 < i)
                        x++;
                    else
                        x = j + 1;
                }
                x = i;
            }
        }
    }
    public void RulesCode(int i)//se establece que hace cada bloque
    {
        switch (ListBlock[i])
        {
            case BlockType.Function:
                Debug.Log("este papu esta usando una función");
                break;
            case BlockType.LoopStart:
                Debug.Log("este papu esta usando loops");
                break;
            case BlockType.LoopEnd:
                Debug.Log("aqui esta el fin del loop que usa el papu");
                break;
            case BlockType.Up:
                Debug.Log("este papu se fue arriba");
                break;
            case BlockType.Down:
                Debug.Log("este papu se fue abajo");
                break;
            case BlockType.Right:
                Debug.Log("este papu se fue derecha");
                break;
            case BlockType.Left:
                Debug.Log("este papu se fue izquierda");
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        RecuperateButtons();//se activa cuando un objeto es eliminado
    }
}
