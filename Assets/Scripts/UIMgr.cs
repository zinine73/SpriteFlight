using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;

public class UIMgr : MonoBehaviour
{
    const string HIGHSCORE_KEY = "HIGHSCORE";
    public UIDocument uiDoc;
    public float scoreMultiplier = 10f;
    Label scoreText;
    Label highScoreText;
    Button restartButton;
    Button quitButton;
    float elasedTime = 0f;
    float score = 0f;
    float highScore = 0f;
    bool isGameOver = false;

    void Start()
    {
        VisualElement ve = uiDoc.rootVisualElement;
        //scoreText = uiDoc.rootVisualElement.Q<Label>("ScoreLabel");
        scoreText = ve.Q<Label>("ScoreLabel");
        //highScoreText = uiDoc.rootVisualElement.Q<Label>("HighScoreLabel");
        highScoreText = ve.Q<Label>("HighScoreLabel");
        highScoreText.style.display = DisplayStyle.None;
        //restartButton = uiDoc.rootVisualElement.Q<Button>("RestartButton");
        restartButton = ve.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        quitButton = ve.Q<Button>("QuitButton");
        quitButton.clicked += QuitGame;
    }

    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        if (isGameOver) return;
        elasedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elasedTime * scoreMultiplier);
        scoreText.text = $"Score: {(int)score:d5}";  
    }

    public void GameOver()
    {
        isGameOver = true;
        restartButton.style.display = DisplayStyle.Flex;
        highScore = PlayerPrefs.GetFloat(HIGHSCORE_KEY, 100);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(HIGHSCORE_KEY, score);
            StartCoroutine(MakeBlink());
        }
        highScoreText.text = $"HighScore: {(int)highScore:D5}";
        highScoreText.style.display = DisplayStyle.Flex;
    }

    IEnumerator MakeBlink()
    {
        for (int i = 0; i < 3; i++)
        {
            highScoreText.style.display = DisplayStyle.None;
            yield return new WaitForSeconds(0.3f);
            highScoreText.style.display = DisplayStyle.Flex;
            yield return new WaitForSeconds(0.3f);
        }    
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);//(SceneManager.GetActiveScene().name);
    }

    void QuitGame()
    {
        Application.Quit();
    }

#if UNITY_EDITOR
    [MenuItem("SpriteFlight/Delete HighScore")]
    static void DeleteHighScore()
    {
        PlayerPrefs.DeleteKey(HIGHSCORE_KEY);
    }

    [MenuItem("SpriteFlight/Restart")]
    static void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    //"SpriteFlight/Restart" 이 메뉴명이 같아야 함
    [MenuItem("SpriteFlight/Restart", true)]
    static bool RestartValidate()
    {
        return Application.isPlaying;
    }
#endif
}
