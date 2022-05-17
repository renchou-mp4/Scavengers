using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    public int level = 1;//当前关卡
    public int food = 100;
    public AudioClip dieClip;
    private bool sleepStep = true;
    private Image dayImage;
    private Text dayText;
    private Text foodText;
    private Text failText;
    private MapManager mapManager;
    [HideInInspector]public bool isEnd = false;
    [HideInInspector] public List<Enemy> enemylist = new List<Enemy>();

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
        //初始化地图
        mapManager = GetComponent<MapManager>();
        mapManager.InitMap();

        //初始化UI
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        failText = GameObject.Find("Die").GetComponent<Text>();
        failText.enabled = false;
        print(3);
        UpdateFoodText();

        //初始化参数
        isEnd = false;
        enemylist.Clear();

        //初始化天数动画
        dayImage = GameObject.Find("DayImage").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "Day " + level;
        Invoke("HideBlack", 1);
    }

    void UpdateFoodText()
    {
        foodText.text = "Food " + food;
    }
	public void AddFood(int num)
    {
        food += num;
        UpdateFoodText();
    }
    public void DecreaseFood(int num)
    {
        food -= num;
        UpdateFoodText();
        if(food<=0)
        {
            failText.enabled = true;
            AudioManager.Instance.RandomPlay(dieClip);
            AudioManager.Instance.StopBgMusic();
        }
    }

    [System.Obsolete]
    public void OnPlayerMove()
    {
        if(sleepStep == true)
        {
            sleepStep = false;
        }
        else
        {
            foreach (var enemy in enemylist)
            {
                enemy.Move();
            }
            sleepStep = true;
        }

        if (isEnd == true)
        {
            Application.LoadLevel(Application.loadedLevel);
            print(5);
        }
    }

    void OnLevelWasLoaded(int sceneLevel)
    {
        level++;
        InitGame();
    }

    void HideBlack()
    {
        dayImage.gameObject.SetActive(false);
    }
}
