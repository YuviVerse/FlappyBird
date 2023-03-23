using System;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject tutorialImage;
        [SerializeField] private GameObject gameOverImage;

        private void Start()
        {
            int topScore = (int)PlayerPrefs.GetFloat("Score");
            HandleScoreText(topScore);
            gameOverImage.SetActive(false);
        }

        public void OnStartGame()
        {
            tutorialImage.SetActive(false);
        }
        
        public void OnScoreUpdate(int score)
        {
            HandleScoreText(score);
        }

        private void HandleScoreText(int score)
        {
            string scoreToString = score.ToString();
            string textToPresent = "";
            for (int i = 0; i < scoreToString.Length; i++)
            {
                textToPresent += $"<sprite={scoreToString[i]}>";
            }
            scoreText.text = textToPresent;
        }

        public void OnGameOver()
        {
            gameOverImage.SetActive(true);
        }
    }
}
