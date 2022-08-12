using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateLoop : MonoBehaviour
{
    public GameObject otherLoop;
    // Update is called once per frame
    void Update()
    {//si se elimina el loop start, eliminamos el loop end y viceversa
        if (otherLoop == null)
            Destroy(this.gameObject);
    }
}
