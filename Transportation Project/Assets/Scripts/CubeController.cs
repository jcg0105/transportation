using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    GameController myGameController;
    public int myX, myY;
    // Start is called before the first frame update
    void Start()
    {
        myGameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    { 
        myGameController.ProcessClick(gameObject, myX, myY);
    }
/*
    private void OnMouseOver()
    {
        if (myGameController.airplaneActive)
        {
            transform.localScale = new Vector3(1.2F, 1.2F, 1.2F);
        }
    }

    private void OnMouseExit()
    {
        if (myGameController.airplaneActive)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    } */
}
