using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameStatus 
{
    next, play, gameover, win, pause
}

public class Manager : Loader<Manager>
{
    [SerializeField] int totalWaves = 10;
    [SerializeField] Text totalMoneyLabel;
    [SerializeField] Text currentWaveLabel;
    [SerializeField] Text BaseHealthLabel;
    [SerializeField] Text playBtnLabel;
    [SerializeField] Button playBtn;
    [SerializeField] Button finishBtn;
    [SerializeField] GameObject spawnPoint;

    [SerializeField] GameObject[] enemies;
    [SerializeField] int totalEnemies = 5;
    [SerializeField] int enemiesPerSpawn;

    //[SerializeField] AudioSource soundSourse;

    public Canvas canvas;
    public Canvas menu;
    
    int scoreCount = 0;
    int waveNumber = 0;
    int totalMoney = 25;
    int baseHP = 25;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    int enemiesToSpawn = 0;
    int score = 0;
    
    GameStatus currentStatus = GameStatus.play;

    const float spawnDelay = 0.2f;
    public List<Enemy> EnemyList = new List<Enemy>();

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = TotalMoney.ToString();
        }
    }
    public int BaseHP
    {
        get
        {
            return baseHP;
        }
        set
        {
            baseHP = value;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false); 
        menu.gameObject.SetActive(true);  
        playBtn.gameObject.SetActive(false);
        StartManager.Instance.scoreTable.gameObject.SetActive(false);
        //SoundSourse = GetComponent<AudioSource>();
        ShowMenu();
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    private void Update()
    {
        HandleSpace();
        HandlePause();
        scoreCount = Score();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyEnemies()
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();
    }

    public int Score()
    {
        if (totalKilled > totalEscaped)
        {
            score = totalMoney + 2 * (totalKilled - totalEscaped);
        }
        else
        {
            score = totalMoney + (totalKilled - totalEscaped);
        }

        return score;
    }

    public void WriteScore()
    {
        StartManager.Instance.score.text = "1 - test - " + scoreCount;
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;

    }    
    
    public void SubtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void IsWaveOver()
    {
        BaseHealthLabel.text = "HP " + baseHP + "/25";

        if ((RoundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = 3 * waveNumber;
            }
            SetCurrentGameStatus();
            ShowMenu();
        }
    }

    public void SetCurrentGameStatus()
    {
        if (BaseHP <= 0)
        {
            currentStatus = GameStatus.gameover;
        }
        else if ((waveNumber == 0) && (RoundEscaped + TotalKilled) == 0)
        {
            currentStatus = GameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentStatus = GameStatus.win;
        }
        else
        {
            currentStatus = GameStatus.next;
        }
    }

    public void FinishBtnPressed()
    {
        currentStatus = GameStatus.gameover;
        canvas.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }

    public void PlayBtnPressed()
    {
        switch (currentStatus)
        {
            case GameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;

            default:
                TotalEscaped = 0;
                totalMoney = 25;
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyTowers();
                TowerManager.Instance.RenameTagBuildSite();
                totalMoneyLabel.text = TotalMoney.ToString();
                BaseHealthLabel.text = "HP " + baseHP + "/25";
                //SoundSourse.PlayOneShot(SoundManager.Instance.Background);
                break;
        }
        DestroyEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLabel.text = waveNumber + 1 + " wave";
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        switch (currentStatus)
        {
            case GameStatus.gameover:
                playBtnLabel.text = "Play Again";

                break;

            case GameStatus.next:
                playBtnLabel.text = "Next Wave";

                break;
                
            case GameStatus.play:
                playBtnLabel.text = "Play game";

                break; 
                
            case GameStatus.win:
                playBtnLabel.text = "Play game";

                break;

            case GameStatus.pause:
                playBtnLabel.text = "Paused";

                break;
        }
        playBtn.gameObject.SetActive(true);
    }

    private void HandleSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
    private void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentStatus = GameStatus.pause;
        }
    }
}
