using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private int height = 10;
    private int width = 10;
    private float gridSpacing = 5f;

    [SerializeField] private GameObject stemCell;
    private GameObject[,] gameGrid;

    //attempting to create a static int that will be used for the heights of the arrays
    //[SerializeField] private static int inputHeight;
    //[SerializeField] private static int inputWidth;

    private Camera Cam;

    public int[,] currentTilePositions;
    public int[,] futureTilePositions;

    int count = 0;
    int delay = 100;
    int timeSpeed;
    int prevTimeSpeed;


    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        Cam = Camera.main;
        //Time.timeScale = 0.02f; failure to slow update down so I can see stuff happening
        Application.targetFrameRate = 120;

        timeSpeed = 0;
        prevTimeSpeed = 1;

        //GameObject[] cells = GameObject.FindGameObjectsWithTag("StemCell");

        currentTilePositions = new int[width + 2, height + 2];
        futureTilePositions = new int[width + 2, height + 2];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                currentTilePositions[x, y] = 0;
                futureTilePositions[x, y] = 0;
            }
        }
    }

    //creates the grid when the game starts
    private void CreateGrid()
    {

        gameGrid = new GameObject[height, width];

        //ensure stemCell isn't empty
        if(stemCell == null)
        {
            Debug.LogError("Empty stemCell");
            return;
        }

        //make the grid
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                //create a new grid space for each cell
                GameObject clonedCells = Instantiate(stemCell, new Vector2(x * gridSpacing, y * gridSpacing), Quaternion.identity);
                
                clonedCells.transform.SetParent(transform);
                clonedCells.name = $"Grid {x}, {y}";
                gameGrid[x, y] = clonedCells;

                /*gameGrid[x, y] = Instantiate(stemCell, new Vector3(x * gridSpacing, y * gridSpacing), Quaternion.identity);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = "Grid Spaces (X: " + x.ToString() + ", Y: " + y.ToString() + ")";*/

                /*currentTilePositions[x, y] = 0;
                futureTilePositions[x, y] = 0;*/
            }
        }

        //gameGrid[1, 1].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;

        //Input.GetMouseButtonDown(0); - on left click
        //Input.GetMouseButtonDown(1); - on right click
        //find mouse position in x, y values then convert to cell to change colour
        //Input.GetKeyDown(KeyCode.Escape);
        //for pause menu


    }

    // Update is called once per frame - incorrect (as I found out)
    // Update is called 60 * per second
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0) == true)
        {
            timeSpeed = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) == true)
        {
            timeSpeed = 1;
            prevTimeSpeed = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) == true)
        {
            timeSpeed = 2;
            prevTimeSpeed = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) == true)
        {
            timeSpeed = 3;
            prevTimeSpeed = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) == true)
        {
            timeSpeed = 4;
            prevTimeSpeed = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) == true)
        {
            timeSpeed = 5;
            prevTimeSpeed = 5;
        }
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            if (timeSpeed == 0)
            {
                timeSpeed = prevTimeSpeed;
            }
            else
            {
                timeSpeed = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Debug.Log($"{currentTilePositions[x + 1, y + 1]} is the cell value");
                }
            }
        }

        mouseClicking();

        //basic time counting to iterate through the simulation and rules
        if (count < delay)
        {
            count = count + timeSpeed;
        }
        else
        {
            count = 0;
            CallInUpdate();
        }
    }

    void mouseClicking()
    {
        Vector3 clickPoint = Cam.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log(clickPoint);// testing to see if it logs the location of the mouse: successful
        //Values supplied are in multiples of five so need to round down from 0-4.9999999999999 (or subsequent number)
        //and then divide by 5 to find the x, y values in the array.

        Vector2Int arraySquareHover = new Vector2Int();

        //converting from the mouseposition on screen to the array position
        arraySquareHover.x = Convert.ToInt16(Math.Floor(clickPoint.x / 5));
        arraySquareHover.y = Convert.ToInt16(Math.Floor(clickPoint.y / 5));

        //Debug.Log(arraySquareHover); // testing to see if it logs the location of the mouse: successful
        //next step is to "onclick", change the tile to green that I am hovering over

        if (Input.GetMouseButtonDown(0) != false && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 0)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 1;
        }
        else if (Input.GetMouseButtonDown(0) != false && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 1)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 0;
        }
        else if (Input.GetMouseButtonDown(0) != false && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 2)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 1;
        }

        if (Input.GetMouseButtonDown(1) != false && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 0)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 2;
        }
        else if (Input.GetMouseButtonDown(1) != false && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 1)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 2;
        }
        else if (Input.GetMouseButtonDown(1) != false && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 2)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 0;
        }
    }

    void CallInUpdate()
    {

        /*Vector3 clickPoint = Cam.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log(clickPoint);// testing to see if it logs the location of the mouse: successful
        //Values supplied are in multiples of five so need to round down from 0-4.9999999999999 (or subsequent number)
        //and then divide by 5 to find the x, y values in the array.

        Vector2Int arraySquareHover = new Vector2Int();
        
        //converting from the mouseposition on screen to the array position
        arraySquareHover.x = Convert.ToInt16(Math.Floor(clickPoint.x/5));
        arraySquareHover.y = Convert.ToInt16(Math.Floor(clickPoint.y/5));

        //Debug.Log(arraySquareHover); // testing to see if it logs the location of the mouse: successful
        //next step is to "onclick", change the tile to green that I am hovering over

        if (Input.GetMouseButtonDown(0) == true && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 0)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 1;
        }
        else if (Input.GetMouseButtonDown(0) == true && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 1)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 0;
        }
        else if (Input.GetMouseButtonDown(0) == true && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 2)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 1;
        }

        if (Input.GetMouseButtonDown(1) == true && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 0)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 2;
        }
        else if (Input.GetMouseButtonDown(1) == true && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 1)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 2;
        }
        else if (Input.GetMouseButtonDown(1) == true && currentTilePositions[arraySquareHover.x, arraySquareHover.y] == 2)
        {
            gameGrid[arraySquareHover.x, arraySquareHover.y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            currentTilePositions[arraySquareHover.x + 1, arraySquareHover.y + 1] = 0;
        } */

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourCount = 0;
                //I am using a 2d integer array that is larger than the height and width of the gamegrid by 2
                //(there is a outline of one array position around the entire gamegrid)
                //this means that the actual position of the gameGrid[x, y] == currentTilePostions[x + 1, y + 1]
                //therefore currentTilePositions[x, y] is to the southwest (down and left) of gameGrid[x, y]
                
                //top row:
                //top left
                if (currentTilePositions[x, y+2] == 1)
                    neighbourCount++;

                //top middle
                if (currentTilePositions[x + 1, y + 2] == 1)
                    neighbourCount++;

                //top right
                if (currentTilePositions[x + 2, y + 2] == 1)
                {
                    neighbourCount++;
                    //initial testing of the proximity works correctly
                    //currentTilePositions[x + 1, y + 1] = 1;
                    //gameGrid[x, y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                }

                //middle row
                //middle left
                if (currentTilePositions[x, y + 1] == 1)
                    neighbourCount++;

                //middle right (skip centre cell)
                if (currentTilePositions[x + 2, y + 1] == 1)
                    neighbourCount++;

                //bottom row
                //bottom left
                if (currentTilePositions[x, y] == 1)
                    neighbourCount++;

                //bottom middle
                if (currentTilePositions[x + 1, y] == 1)
                    neighbourCount++;
                
                //bottom right
                if (currentTilePositions[x + 2, y] == 1)
                    neighbourCount++;

                if (neighbourCount == 3 && currentTilePositions[x + 1, y + 1] == 0)
                {
                    currentTilePositions[x + 1, y + 1] = 1;
                    gameGrid[x, y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                    //Debug.Log($"{x}, {y}");
                }

                if (neighbourCount >3 && currentTilePositions[x + 1, y + 1] == 1)
                {
                    currentTilePositions[x + 1, y + 1] = 0;
                    gameGrid[x, y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }

                if (neighbourCount < 2 && currentTilePositions[x + 1,y + 1] == 1)
                {
                    currentTilePositions[x + 1, y + 1] = 0;
                    gameGrid[x, y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

    public void FailedCode()
    {
        /*private int GetNeighbours(int x, int y)
    {
        int lifeCount = 0;

        int[,] neighbourCells = new int[,]
            {
                //bottom row
                {x-1, y-1},
                {x, y-1},
                {x+1, y-1},

                //middle row
                {x-1, y},
                {x+1, y},

                //top row
                {x-1, y+1},
                {x, y+1},
                {x+1, y+1}
            };

        *//*Vector2[] neighbourPositions =
            {
            //bottom row
            Vector2.left + Vector2.down, //bottom left
            Vector2.down, //bottom middle
            Vector2.right + Vector2.down, //bottom right
            
            //middle row
            Vector2.left, //left of centre
            Vector2.right, //right of centre

            //top row
            Vector2.left + Vector2.up, //top left
            Vector2.up, //top middle
            Vector2.right + Vector2.up //top right
            };*//*

        for (int i = 0; i < neighbourCells.Length; i++)
        {
            if (currentTilePositions[i, 1] == 1)
            {
                lifeCount++;
            }
        }

        return lifeCount;
    }*/


        //failed attempt to change colours to green if neighbouring cells are green
        /*for (int y = 1; y <= height; y++)
        {
            for (int x = 1; x <= width; x++)
            {
                int lifeCount = 0;

                if (x != 0 && y!= 1 && currentTilePositions[x-1, y-1] == 1)
                    lifeCount++;

                if (x != 0 && y != 1 && currentTilePositions[x, y-1] == 1)
                    lifeCount++;

                if (x != width && y != 1 && currentTilePositions[x + 1, y - 1] == 1)
                    lifeCount++;

                if (x != 1 && currentTilePositions[x - 1, y] == 1)
                    lifeCount++;

                if (x != width && currentTilePositions[x + 1, y] == 1)
                    lifeCount++;

                if (x != 1 && y != height && currentTilePositions[x - 1, y + 1] == 1)
                    lifeCount++;

                if (y != height && currentTilePositions[x, y + 1] == 1)
                    lifeCount++;

                if (x != width && y != height && currentTilePositions[x + 1, y + 1] == 1)
                    lifeCount++;

                if (lifeCount >= 3)
                {
                    currentTilePositions[x, y] = 1;
                    gameGrid[x, y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                }

            }
        }*/

        /*Vector3 clickPoint = new Vector3();
        Vector2 mousePosition = new Vector2();
        Event clicking = Event.current;

        mousePosition.x = clicking.mousePosition.x;
        mousePosition.y = clicking.mousePosition.y;

        clickPoint = Cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Cam.nearClipPlane));*/

        /*for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Debug.Log(GetNeighbours(x, y));
            }
        }*/
        //if (height > arraySquareHover.x && arraySquareHover.x > -1 && height > arraySquareHover.y && arraySquareHover.y > -1)
        //    Debug.Log(currentTilePositions[arraySquareHover.x, arraySquareHover.y]);

        /*for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                currentTilePositions[x + 1, y + 1] = futureTilePositions[x + 1, y + 1];
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0;y < height; y++)
            {
                if (currentTilePositions[x + 1, y + 1] == 1)
                {
                    gameGrid[x, y].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                }
            }
        }*/

        /* 
        testing mouse input
        if (Input.GetMouseButtonDown(0) == true)
        {
            gameGrid[1, 1].transform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }

        if (Input.GetMouseButtonDown(1) == true)
        {
            gameGrid[1, 1].transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }*/
    }
}
