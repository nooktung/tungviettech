using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killsText;  // Thêm tham chiếu để hiển thị số kẻ địch đã bị giết

    private void Update()
    {
        HienThiThoiGianGame();
        HienThiSoKills();  // Hiển thị số lượng kẻ địch đã bị giết
    }

    public void HienThiThoiGianGame()
    {
        timeText.SetText(Mathf.FloorToInt(GameManager.Instance.thoiGianChoPhepVeDich).ToString());
    }

    public void HienThiSoKills()
    {
        killsText.SetText("Kill: " + GameManager.Instance.enemyKillCount);
    }

    public void ChoiLai()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void VeMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
