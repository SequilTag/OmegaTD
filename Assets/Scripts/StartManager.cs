using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : Loader<StartManager>
{
    [SerializeField] Button startBtn;
    [SerializeField] Button scoreBtn;
    [SerializeField] Button exit;
    [SerializeField] Button backBtn;

    public GameObject scoreTable;
    public GameObject scoreString;
    public Text score;
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("escape"))  // если нажата клавиша Esc (Escape)
        {
            Application.Quit();    // закрыть приложение
        }
    }

    public void StartBtnPressed()
    {
        Manager.Instance.canvas.gameObject.SetActive(true);
        Manager.Instance.menu.gameObject.SetActive(false);
    }
    public void ScoreBtnPressed()
    {
        Manager.Instance.WriteScore();
        startBtn.gameObject.SetActive(false);
        scoreBtn.gameObject.SetActive(false);
        scoreTable.gameObject.SetActive(true);
    }

    public void BackBtnPressed()
    {
        scoreTable.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(true);
        scoreBtn.gameObject.SetActive(true);
    }

    public void ExitBtnPressed()
    {
        Application.Quit();
    }
}
