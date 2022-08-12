using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{   
    public Transform parentToReturn = null;
    //quise usar para la función
    //public Transform placeholderParent = null;
    [HideInInspector]
    public GameObject placeHolder = null;//nunca asignarle un valor en el editor
    private int indexLoopEndOriginal = 0;//utilizaremos este index para verificar que no este arriba del loop start
    private int indexLoopStartOriginal = 0;//este index es para saber la pos del loop start
    public void OnBeginDrag(PointerEventData eventData)
    {//esto ocurre cuando agarro uno de los bloques
        placeHolder = new GameObject(); //se crea un gameobject que ocupe la posición del bloque
        placeHolder.transform.SetParent(this.transform.parent); //el nuevo bloque se vuelve hijo de "Algorithm"
        RectTransform rt = placeHolder.AddComponent<RectTransform>(); //se accede al RT del nuevo bloque
        rt.sizeDelta = this.GetComponent<RectTransform>().sizeDelta; //se le asigna el mismo tamaño del objeto original al RT del nuevo bloque
        LayoutElement le = placeHolder.AddComponent<LayoutElement>(); //el nuevo objeto adquiere el mismo LayoutElement
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
        //cuando el bloque escogido es loop end
        if (eventData.pointerDrag.GetComponent<Block>().blockType.Equals(Main.BlockType.LoopEnd))
            indexLoopEndOriginal = this.transform.GetSiblingIndex();
        //el nuevo objeto ocupa el lugar el objeto previo en el indice
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturn = this.transform.parent;
        //quise usar para la función
        //placeholderParent = parentToReturn;
        this.transform.SetParent(this.transform.parent.parent);
        //es obligatorio xq si no, no funciona el onDrop y esto lo uso para eliminarlos
        GetComponent<CanvasGroup>().blocksRaycasts = false;//apago el raycast para que se active el drop
    }
    public void OnDrag(PointerEventData eventData)//aqui se coloca lo que pasa cuando toco y muevo los bloques
    {//aqui se realiza el cambio de pos
        this.transform.position = eventData.position;
        int newSiblingIndex = parentToReturn.childCount;
        for (int i = 0; i < parentToReturn.childCount; i++)
        {
            if (this.transform.position.y > parentToReturn.GetChild(i).position.y)
            {
                newSiblingIndex = i;
                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                break;
            }
        }
        //sale error
        //Main.instance.CreateListBlock();//queria llamar a la funcion para que se corrija la lista de los
        //bloques cuando los estoy moviendo (aunque creo q lo estaria llamando mucho

        //queria usarlo para guardar el indice donde se encuentra el loop start y funciona pero como la linea 51 da
        //error, no serviria cuando cambie de posición el bloque
        /*for (int j = 0; j < Main.instance.ListBlock.Count; j++)
        {
            if (Main.instance.ListBlock[j].Equals(Main.BlockType.LoopStart))
            {
                indexLoopStartOriginal = j;
                Debug.Log("index" + indexLoopStartOriginal);
            }
        }*/
        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }
    //el ondrag que quise usar para la funcion
    /*public void OnDrag(PointerEventData eventData)//aqui se coloca lo que pasa cuando toco y muevo los bloques
    {//aqui se realiza el cambio de pos
        this.transform.position = eventData.position;
        if (placeHolder.transform.parent != placeholderParent)
            placeHolder.transform.SetParent(placeholderParent);
        int newSiblingIndex = placeholderParent.childCount;
        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.y > placeholderParent.GetChild(i).position.y) 
            {
                newSiblingIndex = i;
                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                break;
            }
        }
        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }*/
    public void OnEndDrag(PointerEventData eventData)
    {//esto ocurre cuando suelto los bloques
        this.transform.SetParent(parentToReturn);
        //Main.instance.CreateListBlock();//mismo error que el escrito arriba
        /*for (int j = 0; j < Main.instance.ListBlock.Count; j++)
        {
            if (Main.instance.ListBlock[j].Equals(Main.BlockType.LoopStart))
            {
                indexLoopStartOriginal = j;
                Debug.Log("index" + indexLoopStartOriginal);
            }
        }*/
        //la idea q tenia era que si lo de arriba funcionaba, aqui comparaba los indices del loop end y el loop start
        //y si el loop end era menor(esta mas arriba) regrese a su pos previa y si no que cambie de posicion como
        //en los otros casos
        /*if (eventData.pointerDrag.GetComponent<Block>().blockType.Equals(Main.BlockType.LoopEnd))
        {
            if (indexLoopEndOriginal < indexLoopStartOriginal)
                this.transform.SetSiblingIndex(indexLoopEndOriginal);
            else
                this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        }
        else*/
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());//el objeto original ocupa el lugar del nuevo indice
        //es obligatorio volver a prenderlos xq si no, no se podrá mover
        GetComponent<CanvasGroup>().blocksRaycasts = true;//prendo el raycast para poder moverlo si asi lo deseo
        Destroy(placeHolder);
    }
}
