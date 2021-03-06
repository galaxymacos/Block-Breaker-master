﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] private int lives = 3;

    [FormerlySerializedAs("ball")] [SerializeField]
    private MoveBall ballScript; // Script is a class in Unity

    [SerializeField] private GameObject rankingBoard;
    [SerializeField] private GameObject pauseScreen;

    public int point = 0;
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
    [SerializeField] private int maxFireballBullets = 3;
    private int fireballBulletLeft = 0;
    public bool inGame = true;

    private Color originalBallColor = Color.white;

    [SerializeField] private AudioSource ding;
    [SerializeField] private AudioSource blockbreak;
    [SerializeField] private AudioSource explosion;
    [SerializeField] private AudioMixer audioM;
    public PlayerInfo[] playersInfo;

    void LoadLvl() {
        ball.GetComponent<SpriteRenderer>().color = originalBallColor;
        if (currBoard) {
            Destroy(currBoard);
        }

        blocks = 0;
        currBoard = Instantiate(levels[currLvl]);
    }

    public void AddBlock() {
        blocks++;
    }

    public void Death() {
        lives--;
        if (lives > 0) {
            ballScript.Init();
        }
        else {
            inGame = false;
            ballScript.Init();
            score.text = "Game over\nScore:" + point.ToString("D8");
            rankingBoard.SetActive(true);
        }
    }

    public void AddPoints() {
        point += 100;
        string preText = "";
        if (point > bestScore) {
            preText = "Best ";
        }

        score.text = preText + "Score:" + point.ToString("D8");
        blocks--;
        if (blocks <= 0) {
            if (currLvl < levels.Length - 1) {
                // We aren't at the last level
                currLvl++;
            }
            else {
                // We are not at the last level
                Debug.Log("Replay the last level");
            }


//            print("ball destoryed");
            ClearAllEffect();
            ballScript.Init();
            LoadLvl();
        }
    }

    private void ClearAllEffect() {
        isFireBall = false;
        DeleteExtraBalls();
        var theOnlyBall = GameObject.FindGameObjectWithTag("ball");
        theOnlyBall.GetComponent<SpriteRenderer>().color = Color.white;
        if (isPaddleBig)
            ResetPaddleSize();
        DeletePowerUpsInScene();
        protectionPanel.SetActive(false);
    }

    private static void DeletePowerUpsInScene() {
        var powerUps = GameObject.FindGameObjectsWithTag("powerup");
        foreach (var powerUp in powerUps) {
            Destroy(powerUp);
        }
    }

    private static void DeleteExtraBalls() {
        var allBalls = GameObject.FindGameObjectsWithTag("ball");
        if (allBalls.Length > 1) {
            for (int i = 1; i < allBalls.Length; i++) {
                Destroy(allBalls[i]);
            }
        }
    }

    private string rankingList;

    public struct PlayerInfo {
        public string Name { get; set; }
        public int Score { get; set; }

        public PlayerInfo(string name, int score) : this() {
            Name = name;
            Score = score;
        }

        public override string ToString() {
            return Name + ":" + Score;
        }
    }


    // Use this for initialization
    void Start() {
        playersInfo = new PlayerInfo[3];
        LoadPlayerInfo();

        bestScore = PlayerPrefs.GetInt("Score", 0);
//        bestScore = PlayerPrefs.GetInt("1stScore", 0);
        LoadLvl();
        score.text = "Score: " + bestScore.ToString("D8"); // 8 decimals
        protectionPanel = GameObject.Find("ProtectionPanel");
        protectionPanel.SetActive(false);
    }

    private void LoadPlayerInfo() {
        PlayerPrefs.DeleteAll();    // TODO delete later
        rankingList = PlayerPrefs.GetString("RankingList", "");
        print(rankingList);
        if (rankingList != "") {
            string[] rankForEachPlayer = rankingList.Split(';');
            for (int i = 0; i < rankForEachPlayer.Length; i++) {
                var _playerInfo = rankForEachPlayer[i].Split(':');
                playersInfo[i] = new PlayerInfo {Name = _playerInfo[0], Score = Convert.ToInt32(_playerInfo[1])};
            }
        }
    }

    public void MakePaddleLonger() {
        GetComponent<AudioSource>().Play();
        if (!isPaddleBig) {
            var paddleSprite = player.GetComponent<SpriteRenderer>();
            var paddleCollider = player.GetComponent<BoxCollider2D>();
            paddleCollider.size += new Vector2(paddleCollider.size.x, 0);
            paddleSprite.size += new Vector2(paddleSprite.size.x, 0);
            isPaddleBig = true;
        }
    }

    public bool isFireBall;

    public void GetFireballEffect() {
        isFireBall = true;
        fireballBulletLeft = maxFireballBullets;
        GetComponent<AudioSource>().Play();
        foreach (var b in GameObject.FindGameObjectsWithTag("ball")) {
            var sr = b.GetComponent<SpriteRenderer>();
            sr.color = Color.red;
        }

        var childrenInCurLvl = GameObject.FindWithTag("level").GetComponentsInChildren(typeof(BoxCollider2D));
        foreach (BoxCollider2D component in childrenInCurLvl) {
            if (!component.CompareTag("unbreakable"))
                component.isTrigger = true;
        }
    }

    public void ResetFireballEffect() {
        isFireBall = false;
        foreach (var b in GameObject.FindGameObjectsWithTag("ball")) {
            var sr = b.GetComponent<SpriteRenderer>();
            sr.color = Color.white;
        }

        var childrenInCurLvl = GameObject.FindWithTag("level").GetComponentsInChildren(typeof(BoxCollider2D));
        foreach (BoxCollider2D component in childrenInCurLvl) {
            if (!component.CompareTag("unbreakable"))
                component.isTrigger = false;
        }
    }

    public void ResetPaddleSize() {
        var paddle = player.GetComponent<SpriteRenderer>();
        paddle.size -= new Vector2(paddle.size.x / 2, 0);
        var paddleCollider = player.GetComponent<BoxCollider2D>();
        paddleCollider.size += new Vector2(paddleCollider.size.x, 0);
        isPaddleBig = false;
        countDownTimer = 10f; // BUG restore timer to original
    }

    // Update is called once per frame
    void Update() {
        if (inGame) {
            if (Input.GetKey(KeyCode.Escape)) {
                print("game paused");

                PauseGame();
            }
        }
        if (isPaddleBig) {
            countDownTimer -= Time.deltaTime;
            if (countDownTimer <= 0.1f) {
                ResetPaddleSize();
            }
        }

        if (protectionPanelDuration > 0f) {
            protectionPanelDuration -= Time.deltaTime;
            if (Mathf.Abs(protectionPanelDuration) < 0.2f) {
                protectionPanel.SetActive(false);
            }
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void StartOver() {
        inGame = true;
        ClearAllEffect();
        GameObject curLvl = GameObject.FindWithTag("level");
        point = 0;
        Destroy(curLvl);
        ballScript.Init();
        LoadLvl();
    }

    public void Retry() {
        SceneManager.LoadScene("MainGame");
        inGame = true;
//        currLvl = 0;
//        LoadLvl();
//        ball.Init();
//        lives = 3;
//        btnExit.SetActive(false);
//        btnRetry.SetActive(false);
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void SpawnProtectionPanel() {
        protectionPanel.SetActive(true);
        protectionPanelDuration = protectionPanelMaxDuration;
    }

    public void ConsumeFireBall() {
        fireballBulletLeft--;
        if (fireballBulletLeft == 0) {
            ResetFireballEffect();
        }
    }

    private float timeScaleInGame = 1.0f;

    public void PauseGame() {
        inGame = false;
        audioM.SetFloat("volPause", 700);
        timeScaleInGame = Time.timeScale;
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void ResumeGame() {
        inGame = true;
        audioM.SetFloat("volPause", 22000);
        Time.timeScale = timeScaleInGame;
    }

    private float gameWidth = 5f;

    public void PlayDingSound(float soundPosition) {
        ding.panStereo = soundPosition / (gameWidth / 2);
        ding.Play();
    }

    public void PlayBlockBreakSound(float soundPosition) {
        blockbreak.panStereo = soundPosition / (gameWidth / 2);

        blockbreak.Play();
    }

    public void PlayExplosionSound(float soundPosition) {
        explosion.panStereo = soundPosition / (gameWidth / 2);

        explosion.Play();
    }
}