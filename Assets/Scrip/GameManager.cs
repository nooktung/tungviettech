using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float thoiGianChoPhepVeDich = 30f;
    public bool ketThucGame = false;
    private static GameManager instance;
    public GameObject gameOverObject;
    public CarController player;

    // Thêm biến để theo dõi số kẻ địch bị giết
    public int enemyKillCount = 0;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject gameManagerGameObject = new GameObject("GameManager");
                    instance = gameManagerGameObject.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Update()
    {
        if (ketThucGame) return;

        thoiGianChoPhepVeDich -= Time.deltaTime;

        if (thoiGianChoPhepVeDich <= 0)
        {
            thoiGianChoPhepVeDich = 0;
            if (gameOverObject != null)
            {
                gameOverObject.SetActive(true);
            }
            KetThucGame();
        }

        if (player != null && player.currentHP <= 0)
        {
            if (gameOverObject != null)
            {
                gameOverObject.SetActive(true);
            }
            KetThucGame();
        }
    }

    public void IncrementKillCount()
    {
        enemyKillCount++;
        Debug.Log("Số kẻ địch đã bị giết: " + enemyKillCount);
    }

    public void KetThucGame()
    {
        ketThucGame = true;
        if (gameOverObject != null)
        {
            gameOverObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        ketThucGame = false;
        thoiGianChoPhepVeDich = 30f;
        enemyKillCount = 0;  // Đặt lại số lượng kẻ địch đã bị giết khi khởi động lại trò chơi

        if (player != null)
        {
            player.currentHP = player.maxHP;
        }

        if (gameOverObject != null)
        {
            gameOverObject.SetActive(false);
        }

        Debug.Log("Trò chơi đã được khởi động lại.");
    }
}
