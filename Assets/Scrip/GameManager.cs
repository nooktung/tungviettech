using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float thoiGianChoPhepVeDich = 30f;  // Thời gian giới hạn của trò chơi
    public bool ketThucGame = false;  // Cờ để kiểm tra xem game đã kết thúc hay chưa
    private static GameManager instance;  // Singleton để lưu trữ thể hiện của GameManager
    public GameObject gameOverObject;  // Đối tượng hiển thị màn hình Game Over
    public CarController player;  // Tham chiếu đến player (xe) để kiểm tra HP

    // Tạo thể hiện duy nhất (Singleton) của GameManager
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
        // Nếu game đã kết thúc, không làm gì cả
        if (ketThucGame) return;

        // Giảm thời gian chơi
        thoiGianChoPhepVeDich -= Time.deltaTime;

        // Kiểm tra nếu hết thời gian
        if (thoiGianChoPhepVeDich <= 0)
        {
            thoiGianChoPhepVeDich = 0;
            if (gameOverObject != null)
            {
                gameOverObject.SetActive(true);  // Hiển thị màn hình Game Over
            }
            KetThucGame();  // Gọi hàm kết thúc game
        }

        // Kiểm tra HP của player
        if (player != null && player.currentHP <= 0)
        {
            Debug.Log("HP của người chơi đã về 0, sẽ hiển thị Game Over");
            if (gameOverObject != null)
            {
                gameOverObject.SetActive(true);  // Hiển thị màn hình Game Over
            }
            KetThucGame();  // Gọi hàm kết thúc game
        }
    }

    // Hàm kết thúc game
    public void KetThucGame()
{
    ketThucGame = true;  // Đặt cờ kết thúc game
    Debug.Log("Kết thúc game - hiển thị màn hình Game Over.");
    
    if (gameOverObject != null)
    {
        gameOverObject.SetActive(true);  // Hiển thị màn hình Game Over
        Debug.Log("gameOverObject đã được kích hoạt.");
    }
    else
    {
        Debug.Log("gameOverObject chưa được gán trong Inspector!");
    }
}


    // Hàm để khởi động lại trò chơi nếu cần thiết
    public void RestartGame()
    {
        ketThucGame = false;  // Đặt lại cờ kết thúc game
        thoiGianChoPhepVeDich = 30f;  // Đặt lại thời gian chơi

        if (player != null)
        {
            player.currentHP = player.maxHP;  // Đặt lại HP của player
        }

        if (gameOverObject != null)
        {
            gameOverObject.SetActive(false);  // Ẩn màn hình Game Over
        }

        Debug.Log("Game đã được khởi động lại.");
    }
}
