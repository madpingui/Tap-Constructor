using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text hiscoreText;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void EndGame()
    {
        scoreText.text = PlayerPrefs.GetInt("score").ToString();
        hiscoreText.text = "Hiscore: " + PlayerPrefs.GetInt("hiscore").ToString();
    }
}
