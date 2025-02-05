using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject retryPanel, winPanel, gamePanel;
    public int[] initialScoreVal;

    public float countdownTime; // The initial countdown time in seconds
    public Text countdownText;
    private float currentTime;
    internal bool isTimerRunning = true;
    [SerializeField]internal bool win;

    // Sound Btn
    bool soundToggle = true;
    public Image audioBtnImage;
    public Sprite audioOn, audioOff;

    public GameObject[] bg;
    
    [Header("Pause")]
    [SerializeField] internal GameObject pauseBoxPrefab;
    [SerializeField] internal Button  pauseBtn;
    
    
    [Space]
    [SerializeField] LeaderboardItem[] leaderboardItems;
    
    
    [Space]
    public int playerHighestRank = -1;


    [Space] public GameObject gameEnd;

    [Space] public Button useSword;
     public PlayerController playerController;
     
     [Space]
     public Text timerText; // Assign a UI Text component in the Inspector
     private int countdownValue = 10;


     public bool isplayerDead;
     
     IEnumerator CountdownCoroutine()
     {
         while (countdownValue >= 0)
         {
             timerText.text = countdownValue.ToString();
             yield return new WaitForSeconds(1);
             
             countdownValue--;
         }

        timerText.gameObject.SetActive(false);
        playerController.fish.mouth.SetActive(true);
        playerController.fish.sword.SetActive(false);
        playerController.fish.speed = 8;
     }



     void OnClickUseSword()
     {
         playerController.fish.mouth.SetActive(false);
         playerController.fish.sword.SetActive(true);
         playerController.fish.speed *= 1.5f;
         useSword.interactable = false;
         timerText.gameObject.SetActive(true);
         StartCoroutine(CountdownCoroutine());
     }

     public bool isShowd = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        currentTime = countdownTime;

        if (PlayerPrefs.HasKey("Sound"))
            SoundToggle();

        bg[Random.Range(0, bg.Length)].SetActive(true);
    }

    private void Start()
    {
        pauseBtn.AddCustomListner(ShowPause);
        useSword.AddCustomListner(OnClickUseSword);
    }


    void ShowPause()
    {
        Time.timeScale = 0;
        PauseBox.Setup();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayerPrefs.DeleteAll();

        if (Input.GetKeyDown(KeyCode.T))
            Time.timeScale = 1;

        if(isTimerRunning)
            CountDown();
    }

    private void CountDown()
    {
        // Update the countdown timer
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            countdownText.text = currentTime.ToString("F1");
        }
        else
        {
            // Countdown is finished
            currentTime = 0;
            if (!win)
            {
                Win();
            }
               
        }
    }

    public void RetryGame()
    {
        isTimerRunning = false;
        //Invoke("InvokeRetryGame", 1);
        win = false;
        showPOP();
    }

    private void InvokeRetryGame()
    {
        print("Failed");
        AudioManager.Instance.Play("Retry");
        retryPanel.SetActive(true);
        retryPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
    }

    public void Retry()
    {
        AudioManager.Instance.Play("Click");
//        AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene(1);
    }

    
    
    public void Reload()
    {
        AudioManager.Instance.Play("Click");
      //  AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene(2);
    }

    public void Win()
    {
        win = true;
        isTimerRunning = false;
        Debug.Log("Win");
       
        Destroy(FindObjectOfType<PlayerController>().GetComponent<CircleCollider2D>());
        Invoke("showPOP", 1);
    }

    void showPOP()
    {
        if (!isShowd)
        {
            isShowd = true;
            var ee = EndGameBox.Setup();
            ee.Show();
            FindObjectOfType<AIPlayerTargetManager>().target.gameObject.SetActive(false);
            FindObjectOfType<AIPlayerTargetManager>().gameObject.SetActive(false);
            /*foreach (var s in FindObjectOfType<AIPlayerTargetManager>(true).allFishes)
            {
                if (s != null)
                {
                    s.GetComponent<AiPlayerController>()?.gameObject.SetActive(false);
                }
              
            }*/
            
        }
    }

    private void InvokeWinGame()
    {
        print("Win");
        AudioManager.Instance.Play("Win");

        winPanel.SetActive(true);
        winPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
    }

    public void Next()
    {
        AudioManager.Instance.Play("Click");
        //AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene(1);      
    }

    public void Home()
    {
        //AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene(0);
    }

    public void SoundToggle()
    {
        AudioManager.Instance.Play("Click");

        soundToggle = !soundToggle;
        if (soundToggle)
        {
            audioBtnImage.sprite = audioOn;
            AudioManager.Instance.bgAudioSource.mute = false;
            PlayerPrefs.DeleteKey("Sound");
        }
        else
        {
            audioBtnImage.sprite = audioOff;
            AudioManager.Instance.bgAudioSource.mute = true;
            print("Mute");

            PlayerPrefs.SetString("Sound", "");
        }
    }


    private void LateUpdate()
    {
        var aiManager = FindObjectOfType<AIPlayerTargetManager>();
        if (aiManager == null)
        {
            return;
        }

        // Sort the fishes by score in descending order
        aiManager.allFishes = aiManager.allFishes
            .Where(f => f != null) // Remove null entries
            .OrderByDescending(f => f.score) // Sort by score (highest first)
            .ToList();

        // Reset the highest rank
        playerHighestRank = -1;

        for (int i = 0; i < leaderboardItems.Length; i++)
        {
            if (i >= aiManager.allFishes.Count) break;

            var fish = aiManager.allFishes[i];
            if (fish == null)
            {
                continue;
            }

            // Update leaderboard item data
            leaderboardItems[i].SetData(fish.fishName, fish.score.ToString());

            // Update crown visibility
            if (fish.score > 0) fish.crown.SetActiveCrown(i);

            // Check if the fish belongs to the player
            var player = fish.transform.GetComponent<PlayerController>();
            if (player != null)
            {
                // Update the player's highest rank
                if (playerHighestRank == -1 || playerHighestRank > i + 1)
                {
                    playerHighestRank = i + 1; // i is zero-based, rank is 1-based
                }
            }
        }
        
    }

}
