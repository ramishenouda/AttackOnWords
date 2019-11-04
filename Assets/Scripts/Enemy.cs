using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public ParticleSystem Explosion;

    bool canMove;

    [HideInInspector]
    public Text text;
    [HideInInspector]
    public int EnemyLevel;
    [HideInInspector]
    public string word;

    void Start()
    {
        canMove = false;

        text = GetComponentInChildren<Text>();
        text.text = word;
    }

    public void MoveObject()
    {
        canMove = true;
    }

    private void Update()
    {
        if (!canMove || !GameManager.running)
            return;

        transform.position += Vector3.down * Time.deltaTime * 1.5f;
    }

    public void DestroyGameObject()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        return;
    }
}
