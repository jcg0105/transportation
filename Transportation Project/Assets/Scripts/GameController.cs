using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cubePreFab;
    GameObject activeCube;
    Vector3 cubePos;
    int airplaneX, airplaneY;
    //int[] airplanePos = { 0, 8 };
    GameObject[,] grid;
    int gridX, gridY;
    bool airplaneActive;

    // Start is called before the first frame update
    void Start()
    {
        gridX = 16;
        gridY = 9;
        grid = new GameObject[gridX, gridY];
     
        for (int y = 0; y < gridY; y++) {
            for (int x = 0; x < gridX; x++) {

                cubePos = new Vector3(x*2, y*2, 0);
                grid[x,y] = Instantiate(cubePreFab, cubePos, Quaternion.identity);
                grid[x, y].GetComponent<CubeController>().myX = x;
                grid[x, y].GetComponent<CubeController>().myY = y;
            }
        }

        airplaneX = 0;
        airplaneY = 8;
        grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.red;
        airplaneActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessClick(GameObject clickedCube, int x, int y)
    {
        if (x == airplaneX && y == airplaneY)
        {
            if (airplaneActive)
            {
                airplaneActive = false;
                clickedCube.transform.localScale /= 1.5f;
            }
            else
            {
                airplaneActive = true;
                clickedCube.transform.localScale *= 1.5f;
            }
        }
        else
        {
            if (airplaneActive)
            {
                grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.white;
                grid[airplaneX, airplaneY].transform.localScale /= 1.5f;

                airplaneX = x;
                airplaneY = y;
                grid[x, y].GetComponent<Renderer>().material.color = Color.red;
                grid[x, y].transform.localScale *= 1.5f;
            }

        }



        /* if (activeCube != null)
         {
             activeCube.GetComponent<Renderer>().material.color = Color.white;
         }

         //turn clicked cube red
         clickedCube.GetComponent<Renderer>().material.color = Color.red;
         activeCube = clickedCube; */
    }
}
