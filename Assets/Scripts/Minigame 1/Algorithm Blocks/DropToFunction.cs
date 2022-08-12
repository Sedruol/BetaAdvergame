using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropToFunction : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        //es lo quise usar para la funcion
        /*if (eventData.pointerDrag == null)
            return;
        Drag drag = eventData.pointerDrag.GetComponent<Drag>();
        if (drag != null)
        {//si no es loop, se puede ir del algoritmo a la funcion y viceversa
            if (eventData.pointerDrag.GetComponent<Block>().blockType != Main.BlockType.LoopStart &&
                eventData.pointerDrag.GetComponent<Block>().blockType != Main.BlockType.LoopEnd)
            {
                drag.placeholderParent = this.transform;
            }
        }
        Debug.Log(drag.placeholderParent.name);*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //es lo quise usar para la funcion
        /*if (eventData.pointerDrag == null)
            return;
        Drag drag = eventData.pointerDrag.GetComponent<Drag>();
        if (drag != null && drag.placeholderParent == this.transform)
        {//si no es loop, se puede ir del algoritmo a la funcion y viceversa
            if (eventData.pointerDrag.GetComponent<Block>().blockType != Main.BlockType.LoopStart &&
                eventData.pointerDrag.GetComponent<Block>().blockType != Main.BlockType.LoopEnd)
            {
                drag.placeholderParent = drag.parentToReturn;
            }
        }*/
    }
    //se activa cuando un objeto cae encima de este
    public void OnDrop(PointerEventData eventData)
    {//cuando uno de los bloques coliciona con la funcion o el algoritmo
        Drag drag = eventData.pointerDrag.GetComponent<Drag>();
        if (drag != null)
        {//si no es loop, se puede ir del algoritmo a la funcion y viceversa
            if (eventData.pointerDrag.GetComponent<Block>().blockType != Main.BlockType.LoopStart &&
                eventData.pointerDrag.GetComponent<Block>().blockType != Main.BlockType.LoopEnd)
            {
                drag.parentToReturn = this.transform;
            }
        }
    }
}
