using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Pathfinding;
using UnityEditor;
public class Movement : MonoBehaviour
{
    #region KeyCodes
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode rightKey;
    public KeyCode leftKey;
    #endregion
    #region Variables
    public TextAlignment text;

    public Rigidbody2D player1;
    public Rigidbody2D player2;

    public float speed = 40;

    public GameObject WallPrefab;

    public bool AI = false;

    Collider2D wall;

    public GameObject MenuPanel;
    public Text LosingText;

    Vector2 lastWallEnd;


    #endregion

   //It spawns the wall
    public void SpawnWall()
    {
        lastWallEnd = transform.position;


        GameObject g = Instantiate(WallPrefab, transform.position, Quaternion.identity);
        wall = g.GetComponent<Collider2D>();

  
    }
    //It fixes Walls gaps
    public void SpawnWall(Collider2D Co, Vector2 a, Vector2 b)
    {
        Co.transform.position = a + (b - a) * 0.5f;

        float dist = Vector2.Distance(a, b);

        if (a.x != b.x)
            Co.transform.localScale = new Vector2(dist + 1, 1);
        else
            Co.transform.localScale = new Vector2(1, dist + 1);

    }


    // Start is called before the first frame update
    void Start()
    {
       
        timePassed = 1;        
        startingTime = Time.time;
      
        MoveDown();
       
        
        MenuPanel.SetActive(false);
        
        Time.timeScale = 1;

        randomNum = RandomNumber(1, 3);

       
     //Invoke Repeating reuse Raycast fuction 
        
            InvokeRepeating("Raycast", 0f, .01f);
         
       
        
          

        if (gameObject.CompareTag("Player"))
        {
            Debug.Log(AI);
        }
        
      
    }

    //Struct that describes the way of reycasts
    struct Distance
    {
        public float up;
        public float down;
        public float left;
        public float right;
    }
    //sens raycast towards all directions
    Distance distance;
    void Raycast()
    {
        distance.up =   Physics2D.Raycast(transform.position, Vector2.up).distance;
        distance.down =   Physics2D.Raycast(transform.position, Vector2.down).distance;
        distance.left =   Physics2D.Raycast(transform.position, Vector2.left).distance;
        distance.right =   Physics2D.Raycast(transform.position, Vector2.right).distance;
                
        UpdateMove();
    }
    //analyse of the cases we have.For example if we are going Down and the distance isn't a problem we keep going else we count which direction has more space and we go to that direction,etc.
    private void UpdateMove()
    {
        if (AI)
        {
            switch (direction)
            {
                case Direction.Down:
                    if (distance.down > 2)
                    {
                        return;
                    }

                    if (distance.right > distance.left)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }

                    break;
                case Direction.Left:
                    if (distance.left > 2)
                    {
                        return;
                    }

                    if (distance.up > distance.down)
                    {
                        MoveUp();
                    }
                    else
                    {
                        MoveDown();
                    }
                   
                    break;
                case Direction.Up:
                    if (distance.up > 2)
                    {
                        return;
                    }

                    if (distance.right > distance.left)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }

                    break;
                case Direction.Right:
                    if (distance.right > 2)
                    {
                        return;
                    }
                    
                    if (distance.up > distance.down)
                    {
                        MoveUp();
                    }
                    else
                    {
                        MoveDown();
                    }

                    break;
            }
        }
    }

        // Update is called once per frame
        void Update()
    {
        
        if (AI)
        {
            AiLogic();

        }
        else
        {
            PlayerMovement();
        }
        SpawnWall(wall, lastWallEnd, transform.position);
    }
    #region PlayersMovement
    private void PlayerMovement()
    {

        switch (direction)
        {
            case Direction.Up:
                 if (Input.GetKeyDown(leftKey))
                {
                    MoveLeft();

                }
                else if (Input.GetKeyDown(rightKey))
                {
                    MoveRight();
                }
                break;
            case Direction.Right:
                if (Input.GetKeyDown(upKey))
                {
                    MoveUp();

                }
                else if (Input.GetKeyDown(downKey))
                {
                    MoveDown();

                }


                break;
            case Direction.Left:
                if (Input.GetKeyDown(upKey))
                {
                    MoveUp();

                }
                else if (Input.GetKeyDown(downKey))
                {
                    MoveDown();

                }
                break;
            case Direction.Down:
                if (Input.GetKeyDown(leftKey))
                {
                    MoveLeft();

                }
                else if (Input.GetKeyDown(rightKey))
                {
                    MoveRight();
                }
                break;           
        }       
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D co)
    {
        if (co != wall)
        {
            Debug.Log("You Lost: " + name);
            Destroy(gameObject);

            MenuPanel.SetActive(true);
            if (AI == false)
            {
                LosingText.text = " Game Over , You Lost!";
               
                
            }
            else if (AI == true)
            {
                LosingText.text = "Good Job! Press Restart to play again.";
               
                
            }
            Time.timeScale = 0f;
        }
        
    }


    #region Buttons

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit button got pressed.");

    }
    #endregion
    //Just a movement,and we change the direction to the way we are going while we spawn the wall of our tron
    #region Movement 
    public void MoveUp()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;

        SpawnWall();

        direction = Direction.Up;

        Debug.Log("Moved Up");
        
    }
    public void MoveDown()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;

        SpawnWall();

        direction = Direction.Down;

        Debug.Log("Moved Down");
       
    }
    public void MoveLeft()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;

        SpawnWall();

        direction = Direction.Left;

        Debug.Log("Moved Left");
        
    }
    public void MoveRight()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;

        SpawnWall();

        direction = Direction.Right;

        Debug.Log("Moved Right");
       
    }
    #endregion

    Direction direction;
    
    enum Direction
    {
        Up,
        Right,
        Left,
        Down
    }
    //Not Used
    public void AiLogic()
    {

        
    }

    #region time variables
    private float startingTime;
    private float timePassed;
    private float timeSpan;

    private float timeSlow;
    private float timeFast;
    #endregion

   
    //Not used
    public static int RandomNumber(int min, int max)
    {
        System.Random random = new System.Random();
        return random.Next(min, max);
    }

    public int randomNum;


    
}

    





    
      


       












    



