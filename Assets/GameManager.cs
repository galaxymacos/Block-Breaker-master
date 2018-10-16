using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int lives = 3;

    [FormerlySerializedAs("ball")] [SerializeField] private MoveBall ballScript; // Script is a class in Unity
    [SerializeField] private GameObject btnRetry;
    [SerializeField] private GameObject btnExit;

    private int point = 0;
    [SerializeField] private Text score;

    private int blocks = 0;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ball;
    private int currLvl = 0;
    private GameObject currBoard;
    [SerializeField] private float countDownTimer = 10;
    [SerializeField] private string sceneToLoad;

    private GameObject protectionPanel;
    [SerializeField] private float protectionPanelMaxDuration = 15;
    private float protectionPanelDuration;
    
    private bool isPaddleBig = false;

    private int bestScore = 0;

    private Color originalBallColor = Color.white;
    

    void LoadLvl()
    {
        
        ball.GetComponent<SpriteRenderer>().color = originalBallColor;
        if (currBoard)
        {
            Destroy(currBoard);
        }

        blocks = 0;
        currBoard = Instantiate(levels[currLvl]);
    }

    public void AddBlock()
    {
        blocks++;
    }

    public void Death()
    {
        
        lives--;
        if (lives > 0)
        {
            ballScript.Init();
        }
        else
        {
            ballScript.Init();
            score.text = "Game over\nScore:" + point.ToString("D8");
//            ball.gameObject.SetActive(false);
            if (point > bestScore)
            {
                PlayerPrefs.SetInt("Score", point);
                PlayerPrefs.Save();
            }

            btnRetry.SetActive(true);
            btnExit.SetActive(true);
        }
    }

    public void AddPoints()
    {
        point += 100;
        string preText = "";
        if (point > bestScore)
        {
            preText = "Best ";
        }

        score.text = preText + "Score:" + point.ToString("D8");
        blocks--;
        if (blocks <= 0)
        {
            if (currLvl < levels.Length - 1)
            {
                // We aren't at the last level
                currLvl++;
            }
            else
            {
                // We are not at the last level
                Debug.Log("Replay the last level");
            }

            
//            print("ball destoryed");
            ClearAllEffect();
            ballScript.Init();
            LoadLvl();
        }
    }

    private void ClearAllEffect()
    {
        isFireBall = false;
        ball.GetComponent<SpriteRenderer>().color = Color.white;
        isPaddleBig = false;
        DeletePowerUpsInScene();
        protectionPanel.SetActive(false);
        DeleteExtraBalls();
    }

    private static void DeletePowerUpsInScene()
    {
        var powerUps = GameObject.FindGameObjectsWithTag("powerup");
        foreach (var powerUp in powerUps)
        {
            Destroy(powerUp);
        }
    }

    private static void DeleteExtraBalls()
    {
        var allBalls = GameObject.FindGameObjectsWithTag("ball");
        if (allBalls.Length > 1)
        {
            for (int i = 1; i < allBalls.Length; i++)
            {
                Destroy(allBalls[i]);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("Score", 0);
        LoadLvl();
        score.text = "Score: " + bestScore.ToString("D8"); // 8 decimals
        protectionPanel = GameObject.Find("ProtectionPanel");
        protectionPanel.SetActive(false);
    }

    public void MakePaddleLonger()
    {
        GetComponent<AudioSource>().Play();
        if (!isPaddleBig)
        {
            var paddleSprite = player.GetComponent<SpriteRenderer>();
            var paddleCollider = player.GetComponent<BoxCollider2D>();
            paddleCollider.size += new Vector2(paddleCollider.size.x, 0);
            paddleSprite.size += new Vector2(paddleSprite.size.x, 0);
            isPaddleBig = true;
        }
    }

    public bool isFireBall;
    
    public void GetFireballEffect()
    {
        isFireBall = true;
        GetComponent<AudioSource>().Play();
        var sr = ball.GetComponent<SpriteRenderer>();
        originalBallColor = sr.color;
        sr.color = Color.red;
        var childrenInCurLvl = GameObject.FindWithTag("level").GetComponentsInChildren(typeof(BoxCollider2D));
        foreach (BoxCollider2D component in childrenInCurLvl)
        {
            if(!component.CompareTag("unbreakable"))
            component.isTrigger = true;
        }
        print("Fireball effect is ready");
    }

    public void ResetSize()
    {
        var paddle = player.GetComponent<SpriteRenderer>();
        paddle.size -= new Vector2(paddle.size.x / 2, 0);
        var paddleCollider = player.GetComponent<BoxCollider2D>();
        paddleCollider.size += new Vector2(paddleCollider.size.x, 0);
        isPaddleBig = false;
        countDownTimer = 10f; // BUG restore timer to original
    }

    // Update is called once per frame
    void Update()
    {
        print(blocks);
        if (isPaddleBig)
        {
            countDownTimer -= Time.deltaTime;
            if (countDownTimer <= 0.1f)
            {
                ResetSize();
            }
        }

        if (protectionPanelDuration > 0f)
        {
            protectionPanelDuration -= Time.deltaTime;
            if (Mathf.Abs(protectionPanelDuration) < 0.2f)
            {
                protectionPanel.SetActive(false);
            }
        }
        
        
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Retry()
    {
        SceneManager.LoadScene(sceneToLoad);
//        currLvl = 0;
//        LoadLvl();
//        ball.Init();
//        lives = 3;
//        btnExit.SetActive(false);
//        btnRetry.SetActive(false);
    }

    public void SpawnProtectionPanel()
    {
        protectionPanel.SetActive(true);
        protectionPanelDuration = protectionPanelMaxDuration;
    }
    
    
    
    
    
}