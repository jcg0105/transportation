using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //CHALLENGES BY CHOICE: Feedback to the Player (UI timer and score text)

    public GameObject cubePreFab;
    public Text cargoScoreText;
    public Text totalScoreText;
    public Text timerText;
    float gameTimer = 0f;
    GameObject activeCube;
    Vector3 cubePos;
    int airplaneX, airplaneY, planeStartX, planeStartY;
    int depotX, depotY;
    GameObject[,] grid;
    int gridX, gridY;
    public bool airplaneActive;
    float turnLength;
    float turnTimer;
    int planeCargo, planeMax;
    int cargoGain;
    int score;
    int moveX, moveY;
    int targetX, targetY;
    int seconds, minutes, hours;
    string timerString;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        

        turnLength = 1.5f;
        turnTimer = turnLength;

        planeCargo = 0;
        planeMax = 90;

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
        targetX = airplaneX;
        targetY = airplaneY;
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
        gameTimer += Time.deltaTime;
        seconds = (int)(gameTimer % 60);
        minutes = (int)(gameTimer / 60) % 60;
        hours = (int)(gameTimer / 3600) % 24;
        timerString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        timerText.text = timerString;
        

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

   void CalculateDirection()
    {
        moveX = 0;
        moveY = 0;

        if (airplaneY > targetY) //down
        {
            moveY = -1;
        }
        else if (airplaneY < targetY) //up
        {
            moveY = 1;
        } else
        {
            moveY = 0;
        }
        if (airplaneX > targetX) //left
        {
            moveX = -1;
        }
        else if (airplaneX < targetX) //right
        {
            moveX = 1;
        } else
        {
            moveX = 0;
        }

    } 

    void MoveAirplane()
    {
        CalculateDirection();

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
            if (airplaneX >= gridX)
            {
                airplaneX = gridX - 1;
            }
            else if (airplaneX < 0)
            {
                airplaneX = 0;
            }
            if (airplaneY >= gridY)
            {
                airplaneY = gridY - 1;
            }
            else if (airplaneY < 0)
            {
                airplaneY = 0;
            }

            grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.red;
            grid[airplaneX, airplaneY].transform.localScale *= 1.5f;
        }
    }


    public void ProcessClick(GameObject clickedCube, int x, int y)
    {
        if (x == airplaneX  && y == airplaneY)
        {
            if (airplaneActive)
            {
                //deactivate it
                airplaneActive = false;
                clickedCube.transform.localScale /= 1.5f;
                targetX = airplaneX;
                targetY = airplaneY;
            }
            else
            {
                //activate it
                airplaneActive = true;
                clickedCube.transform.localScale *= 1.5f;
            }
        } else if (airplaneActive)
        {
            targetX = x;
            targetY = y;
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
