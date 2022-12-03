using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource Asource;

    public GameObject Defeat;
    public GameObject BackgroundPause;

    public ImageTimer HarvestTimer;
    public ImageTimer EatingTimer;
    public ImageTimer RaidTimer;
    public Image RaidTimerImg;
    public Image PeasantTimerImg;
    public Image WarriorTimerImg;

    public Button peasantButton;
    public Button warriorButton;

    public Text resources;
    public Text Timer;

    public int peasantCost;
    public int warriorCost;

    public int wheatCount;
    public int peasantCount;
    public int warriorsCount;

    public int wheatPerPeasant;
    public int wheatToWarriors;

    public float peasantCreateTime;
    public float warriorCreateTime;
    public float raidMaxTime;
    public int raidIncrease;
    public int nextRaid;

    private float peasantTimer = 0;
    private float warriorTimer = -2;
    private float raidTimer = -2;

    public bool clickPeasantButton = false;
    public bool clickWarriorButton = false;

    private ImgTimer _peasantTimer;
    private ImgTimer _warriorTimer;

    void Start()
    {
        _peasantTimer = new ImgTimer(PeasantTimerImg,this);
        _warriorTimer = new ImgTimer(WarriorTimerImg,this);

        StartCoroutine(TimerCor());

        UpdateText();

    }

    void Update()
    {
        if (clickPeasantButton)
        {
            _peasantTimer.Update(true);
        }

        if (clickWarriorButton)
        {
            _warriorTimer.Update(false);
        }

        if (RaidTimer.Tick)
        {
            wheatCount -= 10;
            if (warriorsCount + 4 >= 0) 
                wheatCount -= 4;
  
            if (wheatCount < 0)
            {
                Defeat.SetActive(true);
                Time.timeScale= 0;
                wheatCount= 0;
            }
            UpdateText();
        }

        if(HarvestTimer.Tick)
        {
            wheatCount += peasantCount * wheatPerPeasant;
            UpdateText();
        }

        if(EatingTimer.Tick)
        {
            wheatCount -= warriorsCount * wheatToWarriors;
            UpdateText();
        }
    }

    public void CreatePeasant()
    {
        wheatCount -= peasantCost;
        peasantButton.interactable= false;
        clickPeasantButton = true;
        UpdateText();
    }

    public void CreateWarrior()
    {
        wheatCount-= warriorCost;
        warriorTimer = warriorCreateTime;
        warriorButton.interactable= false;
        clickWarriorButton= true;
        UpdateText();
    }

    public void ExitToMenu()
    {
        Time.timeScale= 1;
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool _pause = false;
    public void Pause()
    {
        Time.timeScale = 0;

        if (_pause == false)
        {
            Time.timeScale = 0;
            _pause = true;
            BackgroundPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _pause= false;
            BackgroundPause.SetActive(false);
        }
    }

    private bool _mute = false;
    public void MuteMusic()
    {
        if (_mute == false)
        {
            Asource.mute = true;
            _mute = true;
        }
        else 
        {
            _mute = false;
            Asource.mute = false;
        }
    }

    public void UpdateText()
    {
        resources.text = peasantCount + "\n" + warriorsCount + "\n\n" + wheatCount;
    }

    private IEnumerator TimerCor()
    {
        int seconds = 0;
        int minutes = 0;
        string time;

        while (true)
        {
            seconds++;

            if (seconds > 59)
            {
                seconds = 0;
                minutes++;
            }
            if (minutes < 10)
            {
                time = "0" + minutes;
            }
            else
            {
                time = Convert.ToString(minutes);
            }

            if (seconds < 10)
            {
                time += ":0" + seconds;
            }
            else
            {
                time += ":" + Convert.ToString(seconds);
            }

            Timer.text= time;
            yield return new WaitForSeconds(1);
            
        }
    }
}

public class ImgTimer
{
    public float MaxTime = 5;

    private Image _image;
    private float _currentTime;
    private GameManager _gmManager;
    public ImgTimer(Image img, GameManager gameManager)
    { 
       _image = img;
       _currentTime = 0;
       _gmManager = gameManager;
    }

    public void Update(bool peasant)
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > MaxTime)
        {
            _currentTime = 0;

            if (peasant)
            {
                _gmManager.clickPeasantButton = false;
                _gmManager.peasantButton.interactable = true;
                _gmManager.peasantCount++;
            }
            else 
            {
                _gmManager.clickWarriorButton = false;
                _gmManager.warriorButton.interactable = true;
                _gmManager.warriorsCount++;
            }
            _gmManager.UpdateText();
        }

        _image.fillAmount = _currentTime / MaxTime;
    }
}

