using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Button clickButton;
    public int score = 0;
    public int pointsPerClick = 1;

    void Start()
    {
        UpdateScoreUI();
        clickButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        score += pointsPerClick;
        UpdateScoreUI();
        AnimateCaillou();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void AnimateCaillou()
    {
        // Ajoutez ici une animation de lancer si nécessaire
        Debug.Log("Caillou lancé !");
    }
}
