using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    public string fishName;
    public bool ismainPlayer = false;
    public GameObject bonePrefab;
    public Animator anim;
    public float boostForce, increaseSizeVal, speed;
    [HideInInspector] public int score;
    public TextMeshPro scoreText;
    public GameObject boostParticle, level1FishBody, level2FishBody, level3FishBody;
    public ParticleSystem killParticle;
    private FishSkin skins;
    private GameManager gameManager;

    [SerializeField]internal int currentIndex;
    
    public Crown crown;

    [Header("Lvl 1")]
    public SpriteRenderer lvl_1_Body;   
    public SpriteRenderer lvl_1_tail;  
    public SpriteRenderer lvl_1_oan;   
    public SpriteRenderer lvl_1_openMouth;

    [Header("Lvl 2")]
    public SpriteRenderer lvl_2_Body;
    public SpriteRenderer lvl_2_tail;
    public SpriteRenderer lvl_2_oan;
    public SpriteRenderer lvl_2_openMouth;

    [Header("Lvl 3")]
    public SpriteRenderer lvl_3_Body;
    public SpriteRenderer lvl_3_tail;
    public SpriteRenderer lvl_3_oan;
    public SpriteRenderer lvl_3_openMouth;

    private void Awake()
    {
        fishName = PlayerPrefsData.GetName();
    }

    private void Start()
    {
        
        gameManager = FindObjectOfType<GameManager>();

        if (GetComponent<PlayerController>())
            score = 200;
        else
            score = gameManager.initialScoreVal[Random.Range(0, gameManager.initialScoreVal.Length)];

        SetSize(score);

        ShowScore();

        skins = FindObjectOfType<FishSkin>();

        SetRandomSkin();
    }

    private void SetRandomSkin()
    {
        int randomIndex = PlayerPrefsData.GetCurrentIndex();
        if (randomIndex < 0 )
        {
            randomIndex = 0;
        }
        currentIndex = randomIndex;
        lvl_1_Body.sprite      =      skins.skin[randomIndex].lvl_1_Body;
        lvl_1_tail.sprite      =      skins.skin[randomIndex].lvl_1_tail;
         lvl_1_oan.sprite       =       skins.skin[randomIndex].lvl_1_oan;
   lvl_1_openMouth.sprite = skins.skin[randomIndex].lvl_1_openMouth;


        lvl_2_Body.sprite =      skins.skin[randomIndex].lvl_2_Body;
        lvl_2_tail.sprite =      skins.skin[randomIndex].lvl_2_tail;
         lvl_2_oan.sprite =       skins.skin[randomIndex].lvl_2_oan;
   lvl_2_openMouth.sprite = skins.skin[randomIndex].lvl_2_openMouth;


        lvl_3_Body.sprite =      skins.skin[randomIndex].lvl_3_Body;
        lvl_3_tail.sprite =      skins.skin[randomIndex].lvl_3_tail;
         lvl_3_oan.sprite =       skins.skin[randomIndex].lvl_3_oan;
   lvl_3_openMouth.sprite = skins.skin[randomIndex].lvl_3_openMouth;

    }

    public void Die()
    {
        Instantiate(bonePrefab, transform.position, Quaternion.identity);
        FindObjectOfType<AIPlayerTargetManager>().allFishes.Remove(this);
        FindObjectOfType<AIPlayerTargetManager>().SpawnAiPlayer();
        if (GetComponent<PlayerController>())
        {
            //gameManager.gamePanel.SetActive(false);;

            AudioManager.Instance.Play("Die");
            gameManager.RetryGame();
        }

        Destroy(gameObject);
    }

    public void Kill(int killScore)
    {
        if(GetComponent<PlayerController>())
            AudioManager.Instance.Play("Bite");

        killParticle.Play();
        Bite();

        score += killScore;

        ShowScore();

        SetSize(killScore);

        if (GetComponent<AiPlayerController>())
        {
            GetComponent<AiPlayerController>().FindTarget();
        }
    }
    private int loopCount;
    private Vector3 size;

    private void SetSize(int killScore)
    {
        if (transform.lossyScale.x >= 2)
            return;

        // Increase Size
        size = transform.lossyScale;

        loopCount = killScore / 100;

        if (loopCount > 13)
            loopCount = 13;

        for (int i = 0; i < loopCount; i++)
        {
            size.x += .1f;
            size.y += .1f;

            if (transform.lossyScale.x >= 2)
                break;
        }

        if (size.x > 1.3f && size.x < 2)
        {
            level2FishBody.SetActive(true);
            level1FishBody.SetActive(false);
            level3FishBody.SetActive(false);

            anim = level2FishBody.GetComponent<Animator>();
        }
        else if (size.x >= 2)
        {
            level3FishBody.SetActive(true);
            level1FishBody.SetActive(false);
            level2FishBody.SetActive(false);

            anim = level3FishBody.GetComponent<Animator>();
        }

        transform.localScale = size;
    }

    public void Bite()
    {
        if(anim != null)
            anim.Play("Bite");
    }

    public int GetScore()
    {
        return score;
    }

    public void Boost()
    {
        if(GetComponent<PlayerController>())
            AudioManager.Instance.Play("Boost");

        if (boostParticle)
            boostParticle.SetActive(true);

        speed += boostForce;
        Bite();

        Invoke("StopBoost", .5f);
    }

    private void StopBoost()
    {
        boostParticle.SetActive(false);
        speed -= boostForce;
    }

    public void ShowScore()
    {
        scoreText.text = score.ToString();
    }
}
