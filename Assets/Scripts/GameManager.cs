using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public static bool running = true;

    public GameObject wordsOfficer;

    public Transform sliders;
    public Transform EndGameScreen;

    SetupGame SetupGame;
    GameObject invadersHolder;


    public Text LivesText;
    public Text levelText;
    public static Text scoreText;

    //todo get the level from the database
    public static int level = 1;
    public static int Score = 0;
    public static int Lives = 3;

    public static List<char> wordsInvading;

    public static bool canInvade = false;

    int randomWordIndex;
    int officerChildCount;

    private void Awake()
    {

        SetupGame = GetComponent<SetupGame>();
        SetupGame.StartCoroutine("GetWordsFromTheServerAndStartFirstLevel");

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded()
    {
        level++;
        StartLevel();
    }

    public void StartLevel()
    {
        SetupGame.SetupScene(level);

        LivesText = GameObject.FindGameObjectWithTag("livesText").GetComponent<Text>();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        levelText = GameObject.FindGameObjectWithTag("levelText").GetComponent<Text>();

        levelText.text = "LEVEL : " + level;
        LivesText.text = "Lives : " + Lives;


        wordsInvading = new List<char>();

        invadersHolder = GameObject.FindGameObjectWithTag("InvadersHolder");

        Player.InvadersHolder = invadersHolder.transform;
        randomWordIndex = -1;

        InvokeRepeating("MoveWords", 1.0f, 0.75f);
    }

    void MoveWords()
    {
        if (!running)
            return;

        bool canInvade = true;

        officerChildCount = wordsOfficer.transform.childCount;

        if (officerChildCount < 1)
        {
            StartCoroutine(CheckGameOver());
            CancelInvoke();
            return;
        }

        int randomInt = Random.Range(0, officerChildCount);

        Transform child = wordsOfficer.transform.GetChild(randomInt);
        Enemy enemy = child.GetComponent<Enemy>();

        if (enemy.word.Length < 1)
        {
            enemy.DestroyGameObject();
            return;
        }

        char x = enemy.word[0];

        if (invadersHolder.transform.childCount < 4 || randomInt == randomWordIndex)
        {
            wordsInvading.Add(x);
            enemy.MoveObject();

            child.parent = null;
            child.parent = invadersHolder.transform;

            randomWordIndex = randomInt;

            return;
        }


        for (int i = 0; i < wordsInvading.Count; i++)
        {
            if (x == wordsInvading[i])
                canInvade = false;
        }

        if (canInvade)
        {
            wordsInvading.Add(x);
            enemy.MoveObject();

            child.parent = null;
            child.parent = invadersHolder.transform;
        }

        randomWordIndex = randomInt;
    }

    IEnumerator CheckGameOver()
    {
        if (!Player.PlayerIsAlive)
        {
            //Debug.LogError("Player Is Dead");
            StopAllCoroutines();
            //Debug.LogError("Player Is Dead2");
            yield return null;
        }

        //Debug.LogError("Player Is Active");

        yield return new WaitForSeconds(0.5f);
        if (invadersHolder.transform.childCount < 1)
        {
            yield return new WaitForSeconds(1f);
            if(Player.PlayerIsAlive)
            {
                //Debug.LogError("Changing Scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        else
        {
            StartCoroutine(CheckGameOver());
        }
    }

    public void EndGame()
    {
        running = false;
        Player.PlayerIsAlive = false;
        EndGameScreen.gameObject.SetActive(true);
    }
}