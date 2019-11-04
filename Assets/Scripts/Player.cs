using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject ShotSpawnLocation;

    public static Transform InvadersHolder;

    public static Transform Target = null;
    public static Enemy enemy = null;

    AudioSource audioSource;

    public static bool PlayerIsAlive = true;

    string key;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Check if the player is dead
        if (!PlayerIsAlive)
            return;

        //Always moving the object towards the target, even if he is not attacking
        Move();

        //Check if a key has been pressed in this current frame
        if (!KeyPressed())
            return;

        //Check if the key pressed is Escape then we pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.sliders.gameObject.SetActive(GameManager.running);
            GameManager.running = !GameManager.running;
        }

        //Check if the game is paused, this 2 lines should be after we checking if escape key was pressed to pause or resume the game immediately
        if (!GameManager.running)
            return;

        //Check if the key was Space, If it was then we leave current target
        if(key == " " && Target != null)
        {
            LeaveTarget();
            return;
        }

        //If the target is null and the keypressed is the first letter in any word then this function will get a target
        GetTarget();

        //Attack if we have a target and the keypressed is the first letter in the target's word
        Attack();
    }

    void LeaveTarget()
    {
        enemy.text.color = Color.white;
        enemy = null;
        Target = null;
    }

    bool KeyPressed()
    {
        key = Input.inputString;
        
        if (key == "")
            return false;

        key = key.ToUpper();

        return true;
    }

    void Move()
    {
        if (Target != null)
            transform.position = Vector3.Lerp(transform.position, new Vector3(Target.position.x - 0.1f, transform.position.y, transform.position.z), Time.deltaTime * 5);
    }

    void GetTarget()
    {
        if (Target != null)
            return;

        //Destory any enemy With a null or empty word
        FilterEnemies();

        //Finds a target with a word starts with a key pressed this frame
        FindTarget();
    }

    void FilterEnemies()
    {
        foreach (Transform invader in InvadersHolder)
        {
            string word = invader.GetComponent<Enemy>().word;

            if (word == null)
            {
                invader.parent = null;
                continue;
            }

            if (word.Length < 1)
                invader.parent = null;
        }
    }

    void FindTarget()
    {
        foreach(Transform invader in InvadersHolder)
        {
            char index = invader.GetComponent<Enemy>().word[0];
            char _key = key[0];

            if(index == _key)
            {
                Target = invader;
                enemy = Target.GetComponent<Enemy>();
                enemy.text.color = Color.red;

                Attack();
                return;
            }
        }
    }

    void Attack()
    {
        if (Target == null)
            return;

        if(enemy.word == null)
        {
            Debug.LogError("NULL");
            Debug.Break();
        }

        if (enemy.word != null)
        {
            if (enemy.word.Length < 1)
            {
                Debug.LogError("ATTACK LENGTH THAN 1");
                Debug.Break();
                GetTarget();
                return;
            }
        }

        if (key[0] == enemy.word[0])
        {
            GameObject bullet = Instantiate(Bullet, ShotSpawnLocation.transform.position, Quaternion.identity);

            enemy.word = enemy.word.Substring(1);

            bullet.GetComponent<Bullet>().SetTarget(enemy);
            bullet.GetComponent<Bullet>().Currency = enemy.word;

            audioSource.Play();

            if (enemy.word.Length < 1)
                Target = null;

        }
    }
}