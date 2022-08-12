using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [Header("Menu de Pausa")]
    [SerializeField] private Button btnPause;
    [SerializeField] private Button btnRestart;//pertenece al menu de pausa
    [SerializeField] private Button btnPlay;//pertenece al menu de pausa
    [SerializeField] private Button btnHome;//pertenece al menu de pausa
    [SerializeField] private GameObject panelPause;//menu de pausa
    [Header("Menu de Ayuda")]
    [SerializeField] private Button btnHelp;
    [SerializeField] private Button btnExitHelp;//pertenece al menu de ayuda
    [SerializeField] private Button btnBeforePage;//pertenece al menu de ayuda
    [SerializeField] private Button btnNextPage;//pertenece al menu de ayuda
    [SerializeField] private TextMeshProUGUI TMP_Page;//pertenece al menu de ayuda
    [SerializeField] private GameObject panelHelp;//menu de ayuda
    [SerializeField] private GameObject pages;
    private bool enablePause;//variable usada para validar si esta activo el menu de pausa
    private bool enableHelp;//variable usada para validar si esta activo el menu de ayuda
    private string scene;//variable que almacena el nombre de la escena del minijuego
    private int cantPages;//almacena la cantidad de paginas que tiene el menu de ayuda
    private int numPage;//almacena el numero actual de pagina del menu de ayuda
    private void Awake()
    {//desactivamos los menus
        panelPause.SetActive(false);
        panelHelp.SetActive(false);
        btnBeforePage.gameObject.SetActive(false);
        btnNextPage.gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        enablePause = true;
        enableHelp = true;
        numPage = 1;//el menu de ayuda siempre empieza en la pag 1
        cantPages = pages.transform.childCount;//le asignamos la cantidad de paginas del menu de ayuda
        scene = SceneManager.GetActiveScene().name;//le asignamos el nombre de la escena del minijuego
        btnPause.onClick.AddListener(() => ActivatePause());
        btnRestart.onClick.AddListener(() => ActivateRestart());
        btnPlay.onClick.AddListener(() => ActivatePause());
        btnHome.onClick.AddListener(() => ActivateHome());
        btnHelp.onClick.AddListener(() => ActivateHelp());
        btnExitHelp.onClick.AddListener(() => ActivateHelp());
        btnBeforePage.onClick.AddListener(() => BeforePage());
        btnNextPage.onClick.AddListener(() => NextPage());
        TMP_Page.text = numPage + "/" + cantPages;
        pages.transform.GetChild(0).gameObject.SetActive(true);//activamos la primera pag
        for (int i = 1; i < cantPages; i++)//desactivamos el resto de pags
            pages.transform.GetChild(i).gameObject.SetActive(false);
    }
    private void ActivatePause()
    {
        if (enablePause)//es posible abrir el menu?
        {//si
            player.GetComponent<Swipe>().enabled = false;//deshabilitamos el mov del player
            panelPause.SetActive(enablePause);//activamos el menu
            enablePause = false;//ya no es posible abrir el menu
        }
        else
        {//no
            player.GetComponent<Swipe>().enabled = true;//habilitamos el mov del player
            panelPause.SetActive(enablePause);//desactivamos el menu
            enablePause = true;//es posible abrir el menu
        }
    }
    private void ActivateHelp()
    {
        if (enableHelp)//es posible abrir el menu?
        {//si
            player.GetComponent<Swipe>().enabled = false;//deshabilitamos el mov del player
            panelHelp.SetActive(enableHelp);//activamos el menu
            enableHelp = false;//ya no es posible abrir el menu
        }
        else
        {//no
            player.GetComponent<Swipe>().enabled = true;//habilitamos el mov del player
            panelHelp.SetActive(enableHelp);//desactivamos el menu
            enableHelp = true;//es posible abrir el menu
        }
    }
    private void ActivateRestart()
    {
        player.GetComponent<Swipe>().enabled = true;//habilitamos el mov del player
        SceneManager.LoadScene(scene);//reiniciamos el nivel
    }
    private void ActivateHome()
    {
        player.GetComponent<Swipe>().enabled = true;//habilitamos el mov del player
        SceneManager.LoadScene("prueba");//volvemos a la escena previa
    }
    private void BeforePage()
    {//estamos en una pagina superior a la 1?
        if (numPage > 1)
        {//si
            numPage--;//reducimos el numero de pag actual en 1
            TMP_Page.text = numPage + "/" + cantPages;//actualizamos la pagina en la que nos encontramos
            if (numPage == 1)//es la primera pag?
            {//si
                btnBeforePage.gameObject.SetActive(false);//desactivamos la flecha de la izquierda
            }
            else if (numPage == cantPages - 1)//es la pag previa a la ultima?
            {//si
                btnNextPage.gameObject.SetActive(true);//activamos la flecha de la derecha
            }
            pages.transform.GetChild(numPage).gameObject.SetActive(false);//desactivamos la pag previa
            pages.transform.GetChild(numPage - 1).gameObject.SetActive(true);//activamos la pag correspondiente
        }
    }
    private void NextPage()
    {//estamos en una pagina inferior a la ultima?
        if (numPage < cantPages)
        {//si
            numPage++;//aumentamos el numero de pag actual en 1
            TMP_Page.text = numPage + "/" + cantPages;//actualizamos la pagina en la que nos encontramos
            if (numPage == 2)//es la 2da pag?
            {//si
                btnBeforePage.gameObject.SetActive(true);//activamos la flecha de la izquierda
            }
            else if (numPage == cantPages)//es la ultima pag?
            {//si
                btnNextPage.gameObject.SetActive(false);//desactivamos la flecha de la derecha
            }
            pages.transform.GetChild(numPage - 2).gameObject.SetActive(false);//desactivamos la pag previa
            pages.transform.GetChild(numPage - 1).gameObject.SetActive(true);//activamos la pag correspondiente
        }
    }
}