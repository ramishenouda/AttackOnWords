using UnityEngine;

public class Bullet : MonoBehaviour
{

    public ParticleSystem Explosion;
    public Enemy enemy;
    public string Currency;

    Transform Target;

    readonly float Speed = 4;

    void Update()
    {
        if(enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        if (!GameManager.running)
            return;

        Move();
    }

    void Move()
    {
        Vector3 direction = Target.position - transform.position;
        transform.Translate(direction * Speed * Time.deltaTime, Space.World);

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void SetTarget(Enemy _enemy)
    {
        enemy = _enemy;
        Target = enemy.gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null && enemy != null)
        {
            if (other.transform.parent.name != enemy.gameObject.name)
                return;
        }

        if (enemy == null)
            Destroy(gameObject);

        enemy.text.text = Currency;

        if (Currency.Length < 1)
        {
            GameManager.Score += enemy.EnemyLevel + 1;
            GameManager.scoreText.text = "SCORE : " + GameManager.Score;

            enemy.DestroyGameObject();
            Player.Target = null;
            enemy = null;
        }

        else
        {
            Debug.Log(Currency + " " + Currency.Length);
        }

        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        return;
    }
}
