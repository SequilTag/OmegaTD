using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float attackRadius;
    [SerializeField] Projectile projectile;
    [SerializeField] TowerManager tower;

    Enemy targetEnemy = null;
    float attackCounter;

    bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<TowerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;

        if (targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null && 
                Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if(attackCounter <= 0) 
            {
                isAttacking = true;

                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttacking = false;
            }

            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }        
    }

    public void FixedUpdate()
    {
        if (isAttacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttacking = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;

        //if (newProjectile.PType == projectileType.fire)
        //{
        //    Manager.Instance.SoundSourse.PlayOneShot(SoundManager.Instance.Shot1);
        //}
        //else if (newProjectile.PType == projectileType.rocket)
        //{
        //    Manager.Instance.SoundSourse.PlayOneShot(SoundManager.Instance.Shot2);
        //}

        if (targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            //move projectile to enemy
            
            StartCoroutine(MoveProjectile(newProjectile));
            StartCoroutine(MoveTower(tower));
        }
    }

    IEnumerator MoveProjectile(Projectile projectile)
    {
        while (GetTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);

            yield return null;
        }

        if ((projectile != null) || (targetEnemy == null))
        {
            Destroy(projectile);
        }
    }

    IEnumerator MoveTower(TowerManager tower)
    {
        while (GetTargetDistance(targetEnemy) > 0.20f && tower != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            tower.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            tower.transform.localPosition = Vector2.MoveTowards(tower.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);

            yield return null;
        }
    }

    private float GetTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearestEnemy();
            if (thisEnemy == null)
            {
                return 0f;
            }
        }

        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Enemy enemy in Manager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    private Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
