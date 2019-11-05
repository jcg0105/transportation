using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject cubePreFab;
    public Text cargoScoreText;
    public Text totalScoreText;
    GameObject activeCube;
    Vector3 cubePos;
    int airplaneX, airplaneY, planeStartX, planeStartY;
    int depotX, depotY;
    GameObject[,] grid;
    int gridX, gridY;
    bool airplaneActive;
    float turnLength;
    float turnTimer;
    int planeCargo, planeMax;
    int cargoGain;
    //int trainCargo, trainMax;
    //int boatCargo, boatMax;
    int score;
    int moveX, moveY;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        

        turnLength = 1.5f;
        turnTimer = turnLength;

        planeCargo = 0;
        planeMax = 90;
        /*trainCargo = 0;
        trainMax = 200;
        boatCargo = 0;
        boatMax = 550;
        */

        cargoGain = 10;

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

        //plane starts up left
        planeStartX = 0;
        planeStartY = gridY - 1;
        airplaneX = planeStartX;
        airplaneY = planeStartY;
        grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.red;
        airplaneActive = false;
        depotX = gridX - 1;
        depotY = 0;
        grid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;

        moveX = 0;
        moveY = 0;

    }

    // Update is called once per frame
    void Update()
    {
        DetectKeyboardInput();

        if (Time.time > turnTimer)
        {
            MoveAirplane();
            AddCargo();
            DeliverCargo();
            cargoScoreText.text = "Cargo: " + planeCargo;
            totalScoreText.text = "Score: " + score;
            turnTimer += turnLength;
        }
    }

    void DetectKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveY = -1;
            moveX = 0;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveY = 1;
            moveX = 0;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveY = 0;
            moveX = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveY = 0;
            moveX = 1;
        }

    }

    void MoveAirplane()
    {

        if (airplaneActive)
        {
            //remove plane from old spot
            if (airplaneX == depotX && airplaneY == depotY)
            {
                grid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.white;
            }
            grid[airplaneX, airplaneY].transform.localScale /= 1.5f;

            //put plane in new spot
            airplaneX += moveX;
            airplaneY += moveY;

            //make sure plane doesn't go out of bounds
            if (airplaneX >= gridX){
                airplaneX = gridX - 1;
            } else if (airplaneX < 0) {
                airplaneX = 0;
            }
            if (airplaneY >= gridY) {
                airplaneY = gridY - 1;
            } else if (airplaneY < 0) {
                airplaneY = 0;
            }

            grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.red;
            grid[airplaneX, airplaneY].transform.localScale *= 1.5f;
        }
        //reset movement
        moveX = 0;
        moveY = 0;
    }


    public void ProcessClick(GameObject clickedCube, int x, int y)
    {
        if (x == airplaneX && y == airplaneY)
        {
            if (airplaneActive)
            {
                //deactivate it
                airplaneActive = false;
                clickedCube.transform.localScale /= 1.5f;
            }
            else
            {
                //activate it
                airplaneActive = true;
                clickedCube.transform.localScale *= 1.5f;
            }
        }
        
    }

    void AddCargo()
    {
        if (airplaneX == planeStartX && airplaneY == planeStartY)
        {
            planeCargo += cargoGain;

            if (planeCargo > planeMax)
            {
                planeCargo = planeMax;
            }
        }
    }

    void DeliverCargo()
    {
        if (airplaneX == depotX && airplaneY == depotY)
        {
            score += planeCargo;
            planeCargo = 0;
        }
    }

}
