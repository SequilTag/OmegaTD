using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform exit;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float navigation;
    [SerializeField] int health;
    [SerializeField] int rewardAmount;

    bool isDead = false;
    int target = 0;
    float navigationTime;
    Animator anim;
    Collider2D enemyCollider;
    Transform enemy;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Manager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints != null && isDead == false)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigation)
            {
                if (target < waypoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
                }

                else
                {
                     enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
        else if (isDead == true)
        {
            Destroy(enemy);
        }
    }

    public bool IsDead 
    {
        get
        {
            return isDead;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PathPoint")
        {
            target += 1;
        }
        else if (collision.tag == "Finish")
        {
            Manager.Instance.RoundEscaped += 1;
            Manager.Instance.TotalEscaped += 1;
            Manager.Instance.BaseHP -= 1;
            Manager.Instance.UnregisterEnemy(this);
            Manager.Instance.IsWaveOver();
        }
        else if (collision.tag == "Projectile")
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackDamage);
            Destroy(collision.gameObject);
        }
    }

    public void EnemyHit(int hitPoints)
    {
        if (health - hitPoints > 0)
        {
            //hurt
            health -= hitPoints;
        }
        else
        {
            //die
            anim.SetTrigger("didDie");
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        Manager.Instance.TotalKilled += 1;
        Manager.Instance.AddMoney(rewardAmount); 
        Manager.Instance.IsWaveOver();
    }
}
