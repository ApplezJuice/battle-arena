using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class playerBase : NetworkBehaviour
{

    public GameObject playerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        playerCanvas.SetActive(true);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
