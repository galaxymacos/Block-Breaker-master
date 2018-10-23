using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingScreen : MonoBehaviour {
    [SerializeField] private InputField playerName;
    [SerializeField] private TextMeshProUGUI first;
    [SerializeField] private TextMeshProUGUI second;
    [SerializeField] private TextMeshProUGUI third;
    [SerializeField] private GameObject gameOverScreen;

    private GameManager code;

    private bool isConfirmedQuit;

    // Use this for initialization
    void Start() {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (isConfirmedQuit) {
            if (Input.GetKeyDown(KeyCode.I)) {
                gameOverScreen.SetActive(true);
                gameObject.SetActive(false);
            }
            SaveScoreToFile();
        }
        
    }

    public void UploadPlayerScore() {
        var playerScore = new GameManager.PlayerInfo {Name = playerName.text, Score = code.point};
        int index = code.playersInfo.Length - 1;
        if (code.playersInfo[index].Score > code.point) {
            return;
        }

        code.playersInfo[index] = playerScore;
        while (index - 1 >= 0 && code.playersInfo[index - 1].Score < code.playersInfo[index].Score) {
            var temp = code.playersInfo[index - 1];
            code.playersInfo[index - 1] = code.playersInfo[index];
            code.playersInfo[index] = temp;
            index--;
        }

        first.text = code.playersInfo[0].ToString();
        second.text = code.playersInfo[1].ToString();
        third.text = code.playersInfo[2].ToString();
        
        isConfirmedQuit = true;
    }

    private void SaveScoreToFile() {
        string[] _playersInfo = new string[3];
        for (int i = 0; i < _playersInfo.Length; i++) {
            _playersInfo[i] = code.playersInfo[i].ToString();
        }

        var playerLongString = string.Join(";", _playersInfo);

        PlayerPrefs.SetString("RankingList", playerLongString);
    }
}