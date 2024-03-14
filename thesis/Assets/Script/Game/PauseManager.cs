using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    bool isPause;

    private void Awake()
    {
        Instance = this;
    }

    public void TogglePause()
    {
        if (!UIManager.Instance.deadScene.gameObject.activeSelf &&
            !PlayerManager.Instance.isDead)
        {
            isPause = !isPause;
            if (isPause)
            {
                SoundManager.Instance.PlayOnShot("Button");
                UIManager.Instance.pausePanel.SetActive(true);
                GameObject resumeBut = UIManager.Instance.resumeBut.gameObject;
                GameObject quitBut = UIManager.Instance.goBackToMenuBut.gameObject;
                LeanTween.scale(resumeBut, new Vector3(1, 1, 1), 0.3f)
                    .setEase(LeanTweenType.easeInOutCubic);
                LeanTween.scale(quitBut, new Vector3(1, 1, 1), 0.3f)
                   .setEase(LeanTweenType.easeInOutCubic);
                ToggleEnableScript(false);

                EventSystem.current.SetSelectedGameObject(resumeBut);
            }
            else
            {

                SoundManager.Instance.PlayOnShot("Button");
                GameObject resumeBut = UIManager.Instance.resumeBut.gameObject;
                GameObject quitBut = UIManager.Instance.goBackToMenuBut.gameObject;
                LeanTween.scale(resumeBut, new Vector3(0, 0, 0), 0.3f)
           .setEase(LeanTweenType.easeInOutCubic);
                LeanTween.scale(quitBut, new Vector3(0, 0, 0), 0.3f)
                   .setEase(LeanTweenType.easeInOutCubic)
                   .setOnComplete(SetPauseActiveFalse);

                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    public bool GetPauseState()
    {
        return isPause;
    }

    void ToggleEnableScript(bool enable)
    {
        PlayerManager.Instance.enabled = enable;
        CenterMove.instance.enabled = enable;
        CameraFollow.instance.enabled = enable;

        //InputSystemMnanger input = PlayerManager.Instance.transform.GetComponent<InputSystemMnanger>();
        //input.enabled = enable;

        Rigidbody2D playerRb = PlayerManager.Instance.transform.GetComponent<Rigidbody2D>();
        playerRb.simulated = enable;
        if (enable) playerRb.bodyType = RigidbodyType2D.Dynamic;
        else playerRb.bodyType = RigidbodyType2D.Kinematic;

        PlayerBullet[] playerBullet = FindObjectsOfType<PlayerBullet>();
        if (playerBullet.Length > 0)
        {
            foreach (PlayerBullet b in playerBullet) { b.enabled = enable; }
            for (int i = 0; i < playerBullet.Length; i++)
            {
                Rigidbody2D rb = playerBullet[i].GetComponent<Rigidbody2D>();
                rb.simulated = enable;
                if (enable) rb.bodyType = RigidbodyType2D.Dynamic;
                else rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        EnemyBullet[] enemyBullets = FindObjectsOfType<EnemyBullet>();
        if (enemyBullets.Length > 0)
        {
            foreach (EnemyBullet b in enemyBullets) { b.enabled = enable; }
            for (int i = 0; i < enemyBullets.Length; i++)
            {
                Rigidbody2D rb = enemyBullets[i].GetComponent<Rigidbody2D>();
                rb.simulated = enable;
                if (enable) rb.bodyType = RigidbodyType2D.Dynamic;
                else rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();
        if (bombs.Length > 0)
        {
            foreach (Bomb b in bombs) { b.enabled = enable; }
            for (int i = 0; i < bombs.Length; i++)
            {
                Rigidbody2D rb = bombs[i].GetComponent<Rigidbody2D>();
                rb.simulated = enable;
                if (enable) rb.bodyType = RigidbodyType2D.Dynamic;
                else rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        BossProjectile[] bossProjectiles = FindObjectsOfType<BossProjectile>();
        if (bossProjectiles.Length > 0) foreach (BossProjectile b in bossProjectiles) { b.enabled = enable; }

        if (GameManager.Instance.curBoss != null)
        {
            BossController bossCon = GameManager.Instance.curBoss.GetComponent<BossController>();
            if (bossCon.gameObject.activeSelf)
            {
                bossCon.enabled = enable;
            }
        }
    }

    void SetPauseActiveFalse()
    {
        UIManager.Instance.pausePanel.SetActive(false);
        ToggleEnableScript(true);
    }

}
