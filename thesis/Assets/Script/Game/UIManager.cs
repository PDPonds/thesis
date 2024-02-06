using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("===== Score =====")]
    public TextMeshProUGUI scoreText;

    [Header("===== Dead Scene =====")]
    public Transform deadScene;
    public TextMeshProUGUI scoreOnDeadScene;
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

    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.currentScore.ToString();
        scoreOnDeadScene.text = GameManager.Instance.currentScore.ToString();

        if (GameManager.Instance.isMomentum) momentumEffect.gameObject.SetActive(true);
        else momentumEffect.gameObject.SetActive(false);

    }


    void EnableDeadScene()
    {
        deadScene.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
    }

    void ReturnToLobby()
    {
        SceneManager.LoadScene(0);
    }

}
