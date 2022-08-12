using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Swipe : MonoBehaviour
{
    public enum MoveDirection
    {
        None,
        Up,
        Down,
        Right,
        Left
    }//tipo de mov que puede realizar el player
    //SINGLETON
    public int[,] newMatrix;//creamos una matriz de repuesto
    public static Swipe instance;//creamos el singleton
    //Swipe
    private Vector2 firstPress;//posicion inicial de swipe
    private Vector2 lastPress;//posicion final del swipe
    private Vector2 current_swipe;//valor del swipe
    //Mov
    private bool isMoving;//booleano para saber si se esta realizando un swipe
    private Vector3 originPos;//posicion inicial del player
    private Vector3 targetPos;//posicion final del player
    private float timeToMove = 0.2f;//tiempo q se demora en realizar el mov
    private float posX = -7.5f;//posicion inicial en X del player
    private float posY = 2.5f;//posicion inicial en Y del player
    //Matrix
    private int _row = 0;//fila donde se encuentra el player
    private int _queue = 0;//cola donde se encuentra el player
    private bool up = false;//quiere ir hacia arriba?
    private bool down = false;//quiere ir hacia abajo?
    private bool right = false;//quiere ir hacia la derecha?
    private bool left = false;//quiere ir hacia la izquierda?
    private MoveDirection moveDirection;//valor q almacena la direccion hacia donde quiere ir
    private string scene;//almacena el nombre de la escena
    private bool win = false;//booleano para validar si se gano
    //cambios en matrix con objeto movible
    private Tile ObjectToMove;//tile del objeto que se mueve
    [SerializeField] private Tilemap tilemapObjects;//tilemap donde se pintara
    private IEnumerator MovePlayer(Vector3 direction)
    {//corrutina q realiza el mov del player
        isMoving = true;
        float elapsedTime = 0f;
        originPos = transform.position;//almacena la pos del player
        targetPos = originPos + direction;//almacena la posicion final del player
        while (elapsedTime < timeToMove)
        {//se realiza el movimiento
            transform.position = Vector3.Lerp(originPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;//preparamos la habilitacion del singleton
        scene = SceneManager.GetActiveScene().name;//scene obtine el valor de la escena actual
        SearchPlayer();
    }
    void SearchPlayer()
    {//busca la pos del player en la matriz y lo posiciona en la escena
        newMatrix = ReadTXT.instance.matrix;//una matriz temporal almacena los valores de la matriz original
        for (int i = 0; i < newMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < newMatrix.GetLength(1); j++)
            {//recorremos la matriz
                if (newMatrix[i, j] == 0)//buscamos el valor de 0, ya que alli se ubica el player
                {//asignamos la fila y cola donde se encuentra el player
                    _row = i;
                    _queue = j;
                    transform.position = new Vector3(posX + j, posY - i, 0);//posicionamos el player en el mapa
                }
            }
        }
    }
    void CalculateDirection()
    {//desde aqui hasta antes del else, revisamos si el player se saldra del mapa en caso realizemos el mov
        if (_row <= -1)
        {//revisa si el player se salio del mapa por arriba
            _row = 0;//devolvemos el valor previo
            up = false;
            moveDirection = MoveDirection.None;
            Debug.Log("saliste del mapa");
        }
        else if (_queue <= -1)
        {//revisa si el player se salio del mapa por la izquierda
            _queue = 0;//devolvemos el valor previo
            left = false;
            moveDirection = MoveDirection.None;
            Debug.Log("saliste del mapa");
        }
        else if (_row >= newMatrix.GetLength(0))
        {//revisa si el player se salio del mapa por abajo
            _row = newMatrix.GetLength(0) - 1;//devolvemos el valor previo
            down = false;
            moveDirection = MoveDirection.None;
            Debug.Log("saliste del mapa");
        }
        else if (_queue >= newMatrix.GetLength(1))
        {//revisa si el player se salio del mapa por la derecha
            _queue = newMatrix.GetLength(1) - 1;//devolvemos el valor previo
            right = false;
            moveDirection = MoveDirection.None;
            Debug.Log("saliste del mapa");
        }
        else
        {//la matriz temporal vuelve a almacenar los valores de la matriz original, ya que los valores pudieron cambiar previamente
            newMatrix = ReadTXT.instance.matrix;
            int temp = newMatrix[_row, _queue];//almacenamos el valor en la matriz de la posicion a la q se realizara el mov
            if (temp == 0 || temp == 1 || temp == 7)//la nueva posicion es una zona caminable?
            {//zonas por donde puede caminar
                if (up)
                {//quiere subir
                    up = false;
                    if (newMatrix[_row + 1, _queue] == 0 || newMatrix[_row + 1, _queue] == 1 ||
                        newMatrix[_row + 1, _queue] == 7)//se realiza el mov y la posicion previa tambien era una zona caminable
                        moveDirection = MoveDirection.Up;
                    else
                    {//no se realiza el mov y se regresa al valor previo
                        _row++;
                        moveDirection = MoveDirection.None;
                    }
                }
                else if (down)
                {//quiere bajar
                    down = false;
                    if (newMatrix[_row - 1, _queue] == 0 || newMatrix[_row - 1, _queue] == 1 ||
                        newMatrix[_row - 1, _queue] == 7)//se realiza el mov y la posicion previa tambien era una zona caminable
                        moveDirection = MoveDirection.Down;
                    else
                    {//no se realiza el mov y se regresa al valor previo
                        _row--;
                        moveDirection = MoveDirection.None;
                    }
                }
                else if (right)
                {//quiere ir a la derecha
                    right = false;
                    if (newMatrix[_row, _queue - 1] == 0 || newMatrix[_row, _queue - 1] == 1 ||
                        newMatrix[_row, _queue - 1] == 7)//se realiza el mov y la posicion previa tambien era una zona caminable
                        moveDirection = MoveDirection.Right;
                    else
                    {//no se realiza el mov y se regresa al valor previo
                        _queue--;
                        moveDirection = MoveDirection.None;
                    }
                }
                else if (left)
                {//quiere ir a la izquierda
                    left = false;
                    if (newMatrix[_row, _queue + 1] == 0 || newMatrix[_row, _queue + 1] == 1 ||
                        newMatrix[_row, _queue + 1] == 7)//se realiza el mov y la posicion previa tambien era una zona caminable
                        moveDirection = MoveDirection.Left;
                    else
                    {//no se realiza el mov y se regresa al valor previo
                        _queue++;
                        moveDirection = MoveDirection.None;
                    }
                }
            }
            else if (temp == 2)
            {//zona ganable
                if (up)
                {
                    up = false;
                    moveDirection = MoveDirection.Up;
                }
                else if (down)
                {
                    down = false;
                    moveDirection = MoveDirection.Down;
                }
                else if (right)
                {
                    right = false;
                    moveDirection = MoveDirection.Right;
                }
                else if (left)
                {
                    left = false;
                    moveDirection = MoveDirection.Left;
                }
                win = true;
            }
            else if (temp == 3 || temp == 8)//3 = lava (te quemas); 8 = agua (te ahogas)
            {//zonas q matan
                if (up)
                {
                    up = false;
                    moveDirection = MoveDirection.None;
                }
                else if (down)
                {
                    down = false;
                    moveDirection = MoveDirection.None;
                }
                else if (right)
                {
                    right = false;
                    moveDirection = MoveDirection.None;
                }
                else if (left)
                {
                    left = false;
                    moveDirection = MoveDirection.None;
                }
                Debug.Log("Derrota");
                SceneManager.LoadScene(scene);//volvemos a cargar la escena, se reinicia el nivel
            }
            else if (temp == 4 || temp == 6)//4 = muro; 6 = roca
            {//zonas tipo muro
                if (up)
                {//no se realiza el mov, asi que se regresa al valor previo
                    up = false;
                    moveDirection = MoveDirection.None;
                    _row++;
                }
                else if (down)
                {//no se realiza el mov, asi que se regresa al valor previo
                    down = false;
                    moveDirection = MoveDirection.None;
                    _row--;
                }
                else if (right)
                {//no se realiza el mov, asi que se regresa al valor previo
                    right = false;
                    moveDirection = MoveDirection.None;
                    _queue--;
                }
                else if (left)
                {//no se realiza el mov, asi que se regresa al valor previo
                    left = false;
                    moveDirection = MoveDirection.None;
                    _queue++;
                }
                Debug.Log("No puedes avanzar");
            }
            else if (temp == 5)//5 = caja
            {//zonas movibles
                for (int x = 0; x < ReadTXT.instance.levelActual.GetCodeValues.Count; x++)
                {//asignamos el tile del objeto que se movera
                    if (ReadTXT.instance.levelActual.GetCodeValues[x] == 5)
                        ObjectToMove = ReadTXT.instance.levelActual.GetTiles[x];
                }
                if (up)
                {
                    up = false;
                    if (_row - 1 <= -1)
                    {//si arriba de la caja no hay mapa, no se realiza el mov
                        _row++;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                    else if (newMatrix[_row - 1, _queue] == 0 || newMatrix[_row - 1, _queue] == 1)
                    {//si arriba de la caja hay piso o es la posicion donde empezo el player, puedes empujar la caja
                        Debug.Log("row: " + _row + ", queue: " + _queue);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row + 1, 0), ObjectToMove);
                        moveDirection = MoveDirection.Up;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row - 1, _queue] = 5;//cambiamos lo valores de la matriz
                        Debug.Log("Empujaste");
                    }
                    else if (newMatrix[_row - 1, _queue] == 8)
                    {//si arriba de la caja hay agua, se destruyen ambos
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row + 1, 0), null);
                        moveDirection = MoveDirection.Up;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row - 1, _queue] = 1;//cambiamos lo valores de la matriz
                    }
                    else
                    {//si hay algun otro objeto arriba, no se puede empujar la caja
                        _row++;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                }
                else if (down)
                {
                    down = false;
                    if (_row + 1 >= newMatrix.GetLength(0))
                    {//si abajo de la caja no hay mapa, no se realiza el mov
                        _row--;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                    else if (newMatrix[_row + 1, _queue] == 0 || newMatrix[_row + 1, _queue] == 1)
                    {//si abajo de la caja hay piso o es la posicion donde empezo el player, puedes empujar la caja
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row - 1, 0), ObjectToMove);
                        moveDirection = MoveDirection.Down;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row + 1, _queue] = 5;//cambiamos lo valores de la matriz
                        Debug.Log("Empujaste");
                    }
                    else if (newMatrix[_row + 1, _queue] == 8)
                    {//si abajo de la caja hay agua, se destruyen ambos
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row - 1, 0), null);
                        moveDirection = MoveDirection.Down;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row + 1, _queue] = 1;//cambiamos lo valores de la matriz
                    }
                    else
                    {//si hay algun otro objeto abajo, no se puede empujar la caja
                        _row--;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                }
                else if (right)
                {
                    right = false;
                    if (_queue + 1 >= newMatrix.GetLength(1))
                    {//si a la derecha de la caja no hay mapa, no se realiza el mov
                        _queue--;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                    else if (newMatrix[_row, _queue + 1] == 0 || newMatrix[_row, _queue + 1] == 1)
                    {//si a la derecha de la caja hay piso o es la posicion donde empezo el player, puedes empujar la caja
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue + 1, 2 - _row, 0), ObjectToMove);
                        moveDirection = MoveDirection.Right;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row, _queue + 1] = 5;//cambiamos lo valores de la matriz
                        Debug.Log("Empujaste");
                    }
                    else if (newMatrix[_row, _queue + 1] == 8)
                    {//si a la derecha de la caja hay agua, se destruyen ambos
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue + 1, 2 - _row, 0), null);
                        moveDirection = MoveDirection.Right;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row, _queue + 1] = 1;//cambiamos lo valores de la matriz
                    }
                    else
                    {//si hay algun otro objeto a la derecha, no se puede empujar la caja
                        _queue--;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                }
                else if (left)
                {
                    left = false;
                    if (_queue - 1 <= -1)
                    {//si a la izquierda de la caja no hay mapa, no se realiza el mov
                        _queue++;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                    else if (newMatrix[_row, _queue - 1] == 0 || newMatrix[_row, _queue - 1] == 1)
                    {//si a la izquierda de la caja hay piso o es la posicion donde empezo el player, puedes empujar la caja
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue - 1, 2 - _row, 0), ObjectToMove);
                        moveDirection = MoveDirection.Left;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row, _queue - 1] = 5;//cambiamos lo valores de la matriz
                        Debug.Log("Empujaste");
                    }
                    else if (newMatrix[_row, _queue - 1] == 8)
                    {//si a la izquierda de la caja hay agua, se destruyen ambos
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue, 2 - _row, 0), null);
                        tilemapObjects.SetTile(new Vector3Int(-8 + _queue - 1, 2 - _row, 0), null);
                        moveDirection = MoveDirection.Left;
                        ReadTXT.instance.matrix[_row, _queue] = 1;//cambiamos lo valores de la matriz
                        ReadTXT.instance.matrix[_row, _queue - 1] = 1;//cambiamos lo valores de la matriz
                    }
                    else
                    {//si hay algun otro objeto a la izquierda, no se puede empujar la caja
                        _queue++;
                        moveDirection = MoveDirection.None;
                        Debug.Log("hay otro objeto adelante, no puedes empujar");
                    }
                }
            }
        }
        MakeMove();
    }
    public void MakeMove()
    {//realiza el mov correspondiente
        switch (moveDirection)
        {
            case MoveDirection.Up://se aplica el mov hacia arriba
                StartCoroutine(MovePlayer(Vector3.up));
                moveDirection = MoveDirection.None;
                Debug.Log("movimiento hacia arriba");
                break;
            case MoveDirection.Down://se aplica el mov hacia abajo
                StartCoroutine(MovePlayer(Vector3.down));
                moveDirection = MoveDirection.None;
                Debug.Log("movimiento hacia abajo");
                break;
            case MoveDirection.Right://se aplica el mov hacia la derecha
                StartCoroutine(MovePlayer(Vector3.right));
                moveDirection = MoveDirection.None;
                Debug.Log("movimiento hacia derecha");
                break;
            case MoveDirection.Left://se aplica el mov hacia la izquierda
                StartCoroutine(MovePlayer(Vector3.left));
                moveDirection = MoveDirection.None;
                Debug.Log("movimiento hacia izquierda");
                break;
            case MoveDirection.None:
                break;
        }
        if (win)
        {//ganamos?
            win = false;
            Debug.Log("Victoria");
            if (ChargeLevels.instance.GetNumberLevel() < ReadTXT.instance.listLevelsMG2.Count)
            {//aun quedan niveles?
                ChargeLevels.instance.NextLevel();//pasamos al siguiente nivel
                SceneManager.LoadScene(scene);//volvemos a cargar la escena
            }
            else//si no hay mas niveles, completamos el minijuego
                Debug.Log("Minijuego Completado");
        }
    }
    public void ValidatePlayerPosition()
    {//valido en donde se encuentra el player
        if (newMatrix[_row, _queue] == 3 || newMatrix[_row, _queue] == 4 ||
            newMatrix[_row, _queue] == 5 || newMatrix[_row, _queue] == 8)
        {//el jugador se muere si es que se encuentra en una zona que no es caminable
            SceneManager.LoadScene(scene);
        }
        else if(newMatrix[_row, _queue] == 2)
        {//si el jugador se encuentra en zona ganable, gana
            Debug.Log("Victoria");
        }
    }
    // Update is called once per frame
    void Update()
    {//se revisa si un dedo esta tocando la pantalla
        if (Input.touchCount > 0)
        {//solo usamos el primer dedo que toque la pantalla
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {//almacenamos el valor de la posicion del dedo cuando empieza a tocar la pantalla
                case TouchPhase.Began:
                    firstPress = new Vector2(touch.position.x, touch.position.y);
                    break;
                case TouchPhase.Ended://almacenamos el valor de la posicion del dedo cuando deja de tocar la pantalla
                    lastPress = new Vector2(touch.position.x, touch.position.y);
                    current_swipe = new Vector2(lastPress.x - firstPress.x, lastPress.y - firstPress.y);
                    current_swipe.Normalize();//restamos las 2 posiciones y normalizamos
                    if (current_swipe.y > 0 && current_swipe.x > -0.5f && current_swipe.x < 0.5f && !isMoving)
                    {//el swipe indica que se debe mover hacia arriba
                        _row--;
                        up = true;
                    }
                    else if (current_swipe.y < 0 && current_swipe.x > -0.5f && current_swipe.x < 0.5f && !isMoving)
                    {//el swipe indica que se debe mover hacia abajo
                        _row++;
                        down = true;
                    }
                    else if (current_swipe.x < 0 && current_swipe.y > -0.5f && current_swipe.y < 0.5f && !isMoving)
                    {//el swipe indica que se debe mover hacia la izquierda
                        _queue--;
                        left = true;
                    }
                    else if (current_swipe.x > 0 && current_swipe.y > -0.5f && current_swipe.y < 0.5f && !isMoving)
                    {//el swipe indica que se debe mover hacia la derecha
                        _queue++;
                        right = true;
                    }
                    else
                        moveDirection = MoveDirection.None;
                    CalculateDirection();
                    break;
            }
        }
    }
}