using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigating : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Scores()
    {
        //still working on it 
    }

    public void Settings()
    {
        //still working on it
    }

    public void RestartGame()
    {
        GameManager.level = 0;
        GameManager.Lives = 3;

        GameManager.Score = 0;
        GameManager.scoreText.text = "SCORE : ";

        GameManager.wordsInvading.Clear();

        GameManager.instance.wordsOfficer.transform.DetachChildren();
        Player.InvadersHolder.transform.DetachChildren();

        Player.PlayerIsAlive = true;
        GameManager.running = true;

        GameManager.instance.EndGameScreen.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
