using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragMG2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int originPosition;//almacena la pos inicial del boton seleccionado
    private int targetPosition = 0;//almacena la pos final del boton seleccionado
    private Transform parentToReturn = null;//padre que almacena los botones
    private GameObject placeHolder = null;//objeto fantasma que reemplazara al objeto seleccionado
    private GameObject[] player;//almacenara los gameobjects que sean player
    private int newSiblingIndex;//almacena el indice donde se encuentra el botón 

    public void OnBeginDrag(PointerEventData eventData)
    {//se aplica cuando recien empieza el arrastre
        player[0].GetComponent<Swipe>().enabled = false;//deshabilitamos el mov del player
        placeHolder = new GameObject();//instanciamos el objeto fantasma
        placeHolder.transform.SetParent(this.transform.parent);//el objeto fantasma lo volvemos hijo del padre del boton seleccionado
        RectTransform rt = placeHolder.AddComponent<RectTransform>();//le asignamos un rect transform al objeto fantasma
        rt.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;//hacemos q el objeto fantasma tenga el mismo tamaño que el boton seleccionado
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();//le asignamos un layout element al objeto fantasma
        //hacemos que el objeto fantasma tenga los mismos componentes layout element que el boton seleccionado
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
        //el objeto fantasma lo colocamos en la posicion del boton seleccionado
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        parentToReturn = this.transform.parent;//le asignamos el padre del boton seleccionado
        //cambiamos el padre del boton seleccionado
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {//se aplica durante el arrastre
        this.transform.position = eventData.position;//asignamos la posicion del dedo
        newSiblingIndex = parentToReturn.childCount;
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {//se aplica al final de arrastre
        player[0].GetComponent<Swipe>().enabled = true;//habilitamos el mov del player
        this.transform.SetParent(parentToReturn);//el boton seleccionado se vuelve hijo de su padre original
        placeHolder.transform.SetParent(this.transform);//cambiamos el padre del objeto fantasma
        this.transform.SetSiblingIndex(newSiblingIndex);//el boton seleccionado lo posicionamos en la nueva posicion
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (originPosition != this.transform.GetSiblingIndex())//la posicion inicial es diferente a su nueva posicion?
        {//si
            targetPosition = this.transform.GetSiblingIndex();//le asignamos la posicion final
            Rules.instance.ChangeRule(originPosition, targetPosition);//cambiamos la regla de la posicion inicial con la posicion final
            if (this.transform.GetSiblingIndex() - originPosition > 1)//la nueva posicion es mayor a la inicial por mas de 1?
            {
                targetPosition -= 1;//la posicion final se disminuye en 1
                parentToReturn.GetChild(targetPosition).SetSiblingIndex(originPosition);//cambiamos de posicion con el otro boton
            }
            else if (this.transform.GetSiblingIndex() - originPosition < -1)//la nueva posicion es menor a la inicial por mas de 1?
            {
                targetPosition += 1;//la posicion final se incrementa en 1
                parentToReturn.GetChild(targetPosition).SetSiblingIndex(originPosition);//cambiamos de posicion con el otro boton
            }
            for (int i = 0; i < parentToReturn.childCount; i++)
            {//re asignamos el valor de la posicion inicial en base al indice relacionado con su padre
                parentToReturn.GetChild(i).GetComponent<DragMG2>().originPosition = parentToReturn.GetChild(i).GetSiblingIndex();
            }
            ReadTXT.instance.cantUses++;//la cantidad de usos aumenta en 1
            Swipe.instance.ValidatePlayerPosition();//validamos la pos del player
        }
        Destroy(placeHolder);//eliminamos el objeto fantasma
    }
    private void Start()
    {//hago esto porque este script esta dentro de un prefab
        player = GameObject.FindGameObjectsWithTag("Player");//buscamos los objetos con tag player 
        originPosition = this.transform.GetSiblingIndex();//la pos inicial del boton en base a su indice de hijo
    }
}