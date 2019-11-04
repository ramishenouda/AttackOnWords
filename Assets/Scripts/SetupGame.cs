using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class SetupGame : MonoBehaviour
{
    public GameObject[] enemiesPrefab;

    int[] numberOfWords = new int[5];
    //int level;

    int wordNumber = 0;
    float x, y;

    Transform wordsOfficer;
    List<string[]> wordsServerResponse;

    public IEnumerator GetWordsFromTheServerAndStartFirstLevel()
    {
        wordsServerResponse = new List<string[]>();
        UnityWebRequest www = null;

        for (int i = 0; i < 5; i++)
        {
            try
            {
                WWWForm form = new WWWForm();
                form.AddField("level", i);
                www = UnityWebRequest.Post("https://aramii.tk/wordsinvadingserver/", form);
            }

            catch
            {
                Debug.LogError("database is offline");
                StopCoroutine("Register");
            }

            yield return www.SendWebRequest();

            wordsServerResponse.Add(www.downloadHandler.text.Split(' '));
        }

        TrimWords();
        GameManager.instance.StartLevel();
    }

    void TrimWords()
    {
        for (int i = 0; i < wordsServerResponse.Count; i++)
        {
            for (int j = 0; j < wordsServerResponse[i].Length; j++)
            {
                wordsServerResponse[i][j] = wordsServerResponse[i][j].Trim();
            }
        }
    }

    //Instantiating the objects
    public void SetupScene(int level)
    {
        //Defining the instantiation positions
        DefineOffsets();
        SetNumberOfWordsForEachEnemyLevel(level);

        int[] currentMaxMinLevelOfWords = new int[2];
        currentMaxMinLevelOfWords = CheckMaxMinLevelOfWords();

        while (currentMaxMinLevelOfWords[1] != -1)
        {
            SpawnObject(currentMaxMinLevelOfWords);
            currentMaxMinLevelOfWords = CheckMaxMinLevelOfWords();
        }
    }

    void SetNumberOfWordsForEachEnemyLevel(int level)
    {
        numberOfWords[0] = (int)Mathf.Log(level, 2f) * 3 + 2;
        numberOfWords[1] = (int)Mathf.Log(level, 3f) * 2;
        numberOfWords[2] = (int)Mathf.Log(level, 4f) * 2;
        numberOfWords[3] = (int)Mathf.Log(level, 6f) * 2;
        numberOfWords[4] = (int)Mathf.Log(level, 8f) * 2;
    }

    void DefineOffsets()
    {
        wordsOfficer = GameObject.FindGameObjectWithTag("WordsOfficer").transform;

        x = (wordsOfficer.localScale.x / 2) - (enemiesPrefab[0].transform.localScale.x / 2);
        y = wordsOfficer.position.y;
    }

    int[] CheckMaxMinLevelOfWords()
    {
        int max = -1;
        int min = 5;

        for (int i = 0; i < 5; i++)
        {
            if (numberOfWords[i] > 0)
            {
                if (i > max)
                    max = i;

                if (i < min)
                    min = i;
            }
        }

        return new int[2] { min, max };
    }

    void SpawnObject(int[] currentMaxMinLevelOfWords)
    {
        int wordLevel = Random.Range(currentMaxMinLevelOfWords[0], currentMaxMinLevelOfWords[1] + 1);

        float randomX = Random.Range(-x, x);
        Vector3 SpawnLoc = new Vector3(randomX, y, 0.0f);

        GameObject enemy = Instantiate(enemiesPrefab[wordLevel], SpawnLoc, Quaternion.identity);
        //Setting EnemyComponents values
        enemy.GetComponent<Enemy>().word = GetWord(wordLevel);
        enemy.GetComponent<Enemy>().EnemyLevel = wordLevel;
        //Setting enemy name, this is important to make the OnTriggerEnter function works in the bullet script
        enemy.name += ++wordNumber;
        enemy.transform.SetParent(wordsOfficer);
    }

    string GetWord(int wordLevel)
    {
        if (!EnemiesExist(wordLevel))
        {
            wordLevel = 0;

            for (int i = 0; i < 4; i++)
            {
                if (!EnemiesExist(wordLevel))
                {
                    wordLevel++;
                    continue;
                }
                break;
            }

            //ToDo catch error
            if (!EnemiesExist(wordLevel))
            {
                Debug.LogError("No enemies and still GetWord function being called");
                Debug.Break();
                return "";
            }
        }

        //int maxLength = Random.Range(0, wordsServerResponse[wordLevel].Length);

        string word = wordsServerResponse[wordLevel][Random.Range(0, wordsServerResponse[wordLevel].Length)].ToUpper();

        --numberOfWords[wordLevel];

        return word;
    }

    bool EnemiesExist(int index)
    {
        if (numberOfWords[index] > 0)
            return true;

        return false;
    }
}