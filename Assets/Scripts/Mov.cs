using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log(Mathf.Rad2Deg*transform.eulerAngles.y);
            if (transform.localRotation.eulerAngles.y == 0 /*|| transform.localEulerAngles.y % 180 == 0*/)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
            else
                transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("abajo");
            if (transform.localEulerAngles.y % 180 == 0 || transform.localEulerAngles.y == 0)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
            else
                transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            transform.localEulerAngles = new Vector3(0, -transform.localEulerAngles.y, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("izquierda");
            if (transform.localEulerAngles.y % 180 == 0 || transform.localEulerAngles.y == 0)
                transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y - 90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("derecha");
            if (transform.localEulerAngles.y % 180 == 0 || transform.localEulerAngles.y == 0)
                transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 90, 0);
        }
    }
}
