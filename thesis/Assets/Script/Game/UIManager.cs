using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("===== HP =====")]
    public Transform hpParent;
    public GameObject heartPrefab;

    [Header("===== Score =====")]
    public TextMeshProUGUI scoreText;

    [Header("===== Dead Scene =====")]
    public Transform deadScene;
    public TextMeshProUGUI scoreOnDeadScene;
    public Button returnTolobbyButton;

    private void OnEnable()
    {
        PlayerManager.Instance.onTakeDamage += RemoveHeartUI;
        PlayerManager.Instance.onDead += EnableDeadScene;
        PlayerManager.Instance.onHeal += AddHeartUI;

        returnTolobbyButton.onClick.AddListener(() => ReturnToLobby());
    }

    private void OnDisable()
    {
        PlayerManager.Instance.onTakeDamage -= RemoveHeartUI;
        PlayerManager.Instance.onDead -= EnableDeadScene;
        PlayerManager.Instance.onHeal -= AddHeartUI;

    }

    private void Awake()
    {
        deadScene.gameObject.SetActive(false);
    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.currentScore.ToString();
        scoreOnDeadScene.text = GameManager.Instance.currentScore.ToString();
    }

    void AddHeartUI()
    {
        GameObject heart = Instantiate(heartPrefab, hpParent, false);
    }

    void RemoveHeartUI()
    {
        if (hpParent.childCount > 0)
        {
            Destroy(hpParent.GetChild(0).gameObject);
        }
    }

    void EnableDeadScene()
    {
        deadScene.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        hpParent.gameObject.SetActive(false);
    }

    void ReturnToLobby()
    {
        SceneManager.LoadScene(0);
    }

}
