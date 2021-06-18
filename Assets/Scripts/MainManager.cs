using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject resetButton;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private string highScoreName;
    private int highScore;

    private int score;

    private bool m_GameOver = false;

    private MainMenuHandler mainMenuHandler;


    // Start is called before the first frame update
    void Start()
    {   
        mainMenuHandler = GameObject.Find("MainMenuHandler").GetComponent<MainMenuHandler>();    
        ShowBestScore();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        score = m_Points;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighScore(score);
    }

    void ShowBestScore()
    {
        LoadHighScore();
        bestScoreText.text = $"Best Score : {highScoreName} : {highScore}";
    }


    [System.Serializable]
    class HighScoreData
    {
        public int highScore;
        public string playerName;
    }
    public void SaveHighScore(int score)
    {        
        HighScoreData data = new HighScoreData();

        if (score >= highScore)
        {           
            data.highScore = m_Points;
            data.playerName = mainMenuHandler.playerName;

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

            resetButton.SetActive(true);
        }       
    }
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);

            highScore = data.highScore;
            highScoreName = data.playerName;
        }
    }
    public void ResetHighScore()
    {
        HighScoreData data = new HighScoreData();

        data.highScore = 0;
        data.playerName = "";

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
