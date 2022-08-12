using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //se activa cuando un objeto cae encima de este
    public void OnDrop(PointerEventData eventData)
    {//cuando uno de los bloques coliciona con los objetos "EliminatedZone"
        Globals.enableDestroy = true;
        Destroy(eventData.pointerDrag.GetComponent<Drag>().placeHolder);//eliminamos el placeholder
        BlockTypeEliminated(eventData.pointerDrag.GetComponent<Block>().blockType);
        Destroy(eventData.pointerDrag);//estoy eliminando al objeto q cae encima
    }
    public void BlockTypeEliminated(Main.BlockType index)
    {
        //dependiendo del tipo de bloque, se impre un mensaje de que bloque se elimino
        switch (index)
        {
            case Main.BlockType.Function:
                Globals.indexBlockType = 0;
                Debug.Log("se elimino una función");
                break;
            case Main.BlockType.LoopStart:
                Globals.indexBlockType = 1;
                Debug.Log("se elimino el loop start");
                break;
            case Main.BlockType.LoopEnd:
                Globals.indexBlockType = 1;
                Debug.Log("se elimino el loop final");
                break;
            case Main.BlockType.Up:
                Globals.indexBlockType = 2;
                Debug.Log("se elimino un botón arriba");
                break;
            case Main.BlockType.Down:
                Globals.indexBlockType = 3;
                Debug.Log("se elimino un botón abajo");
                break;
            case Main.BlockType.Right:
                Globals.indexBlockType = 4;
                Debug.Log("se elimino un botón derecha");
                break;
            case Main.BlockType.Left:
                Globals.indexBlockType = 5;
                Debug.Log("se elimino un botón izquierda");
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {//no borrar, debe existir para que funcione el OnDrop

    }

    public void OnPointerExit(PointerEventData eventData)
    {//no borrar, debe existir para que funcione el OnDrop

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
