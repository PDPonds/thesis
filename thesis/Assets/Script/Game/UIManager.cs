using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("===== HP =====")]
    [SerializeField] Image hpFill;

    [Header("===== Score Coin =====")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinInGameText;

    [Header("===== Dead Scene =====")]
    public Transform deadScene;
    public TextMeshProUGUI scoreOnDeadScene;
    public TextMeshProUGUI coinText;
    public Button reviveButton;
    public Image reviveFill;
    public float reviveTime;
    float curReviveTime;
    public Button returnTolobbyButton;

    [Header("===== Momentum =====")]
    public GameObject momentumEffect;


    private void OnDisable()
    {
        PlayerManager.Instance.onDead -= EnableDeadScene;

    }

    private void Awake()
    {
        deadScene.gameObject.SetActive(false);

    }

    private void Start()
    {
        PlayerManager.Instance.onDead += EnableDeadScene;

        returnTolobbyButton.onClick.AddListener(() => ReturnToLobby());
        reviveButton.onClick.AddListener(() => ReviveBut());
    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.currentScore.ToString();
        scoreOnDeadScene.text = GameManager.Instance.currentScore.ToString();
        coinInGameText.text = PlayerManager.Instance.inGameCoin.ToString();

        if (GameManager.Instance.isMomentum) momentumEffect.gameObject.SetActive(true);
        else momentumEffect.gameObject.SetActive(false);

        float hpPercent = PlayerManager.Instance.currentHp / PlayerManager.Instance.maxHp;
        hpFill.fillAmount = hpPercent;

        if (deadScene.gameObject.activeSelf)
        {
            coinText.text = PlayerManager.Instance.inGameCoin.ToString();
            if (PlayerManager.reviveItemCount > 0)
            {
                reviveButton.gameObject.SetActive(true);
                returnTolobbyButton.gameObject.SetActive(false);
                curReviveTime += Time.deltaTime;
                if (curReviveTime >= reviveTime)
                {
                    reviveButton.gameObject.SetActive(false);
                    returnTolobbyButton.gameObject.SetActive(true);
                }
                float revivePercent = curReviveTime / reviveTime;
                reviveFill.fillAmount = revivePercent;
            }
            else
            {
                reviveButton.gameObject.SetActive(false);
                returnTolobbyButton.gameObject.SetActive(true);
            }
        }

    }

    void ReviveBut()
    {
        PlayerManager.Instance.currentHp = PlayerManager.Instance.maxHp;
        PlayerManager.Instance.isDead = false;
        PlayerManager.Instance.anim.SetBool("isDead", false);
        PlayerManager.Instance.noDamage = true;
        PlayerManager.reviveItemCount--;
        deadScene.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        coinInGameText.gameObject.SetActive(true);
    }

    void EnableDeadScene()
    {
        deadScene.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        coinInGameText.gameObject.SetActive(false);
    }

    void ReturnToLobby()
    {
        PlayerManager.coin += PlayerManager.Instance.inGameCoin;
        SceneManager.LoadScene(0);
    }

}
