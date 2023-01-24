using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSerial : MonoBehaviour
{
    [SerializeField] private Text _versionText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _comboText;
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private Text _bestComboText;
    [SerializeField] private Text _totalCountGames;
    [SerializeField] private Text _totalCountBlocks;
    [SerializeField] private ChangeSkybox _changeSkybox;
    // [SerializeField] private Button _restartButton;

    public int score;
    public int highestScore;
    public int combo;
    public int highestCombo;
    public int totalCountGames;
    public int totalCountBlocks;
    public int currentSkyboxIdx;

    private const string _savePath = "/GameStats.dat";

    private void Start()
    {
        LoadGameSettings();
        SetSettings();
    }

    public void RestartGame(float delay = 0f)
    {
        Invoke(nameof(Restart), delay);
    }

    public void Restart()
    {
        SaveGameSettings();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetSettings()
    {
        _changeSkybox.ChangeSkyboxToNext(0);
        // _restartButton.gameObject.SetActive(false);
        _versionText.text = $"V {Application.version}";

        _bestScoreText.text = highestScore.ToString();
        _bestComboText.text = highestCombo.ToString();
        _totalCountGames.text = totalCountGames.ToString();
        _totalCountBlocks.text = totalCountBlocks.ToString();
    }

    private void SaveGameSettings()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath + _savePath); 
        GameData data = new GameData();
        data.highestScore = Mathf.Max(highestScore, score);
        data.highestCombo = Mathf.Max(highestCombo, combo);
        data.totalCountGames = totalCountGames + 1;
        data.totalCountBlocks = totalCountBlocks + score;
        data.skyboxIdx = currentSkyboxIdx;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }
    
    private void LoadGameSettings()
    {
        if (File.Exists(Application.persistentDataPath + _savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =  File.Open(Application.persistentDataPath + _savePath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            highestScore = data.highestScore;
            highestCombo = data.highestCombo;
            totalCountGames = data.totalCountGames;
            totalCountBlocks = data.totalCountBlocks;
            currentSkyboxIdx = data.skyboxIdx;
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    public void GetParamsOnClick(int currentScore, int currentCombo)
    {
        score = currentScore;
        combo = currentCombo + 1;
        _scoreText.text = $"{currentScore}";
        if (combo == 1) _comboText.text = "";
        else _comboText.text = $"x {combo}";
    }
    
    public void GetParamsOnEnd(int currentScore)
    {
        // _restartButton.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        score = currentScore;
        SaveGameSettings();
    }
}

[Serializable]
internal class GameData
{
    public int highestScore;
    public int highestCombo;
    public int totalCountGames;
    public int totalCountBlocks;
    public int skyboxIdx;
}
