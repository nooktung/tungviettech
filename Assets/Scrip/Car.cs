using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Sử dụng thư viện UI để làm việc với Scrollbar

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField]
    private float tocDoXe = 20f;  // Tốc độ di chuyển của xe
    [SerializeField]
    private float lucReXe = 30f;  // Lực rẽ của xe
    [SerializeField]
    private float maxSpeed = 30f;  // Tốc độ tối đa của xe
    [SerializeField]
    private GameObject hieuUngPhanh;  // Hiệu ứng phanh
    [SerializeField]
    private GameObject hieuUngBan;  // Hiệu ứng bắn

    [Header("Health System")]
    public int maxHP = 100;  // Máu tối đa của xe
    public int currentHP;  // Máu hiện tại của xe
    [SerializeField]
    private Scrollbar healthBar;  // Scrollbar UI đại diện cho thanh máu
    private RectTransform handleRect;  // Lưu RectTransform của Handle để điều chỉnh kích thước
    private float originalHandleWidth;  // Chiều rộng ban đầu của Handle

    private float dauVaoDiChuyen;  // Đầu vào di chuyển (W/S)
    private float dauVaoRe;  // Đầu vào rẽ (A/D)
    private Rigidbody rb;  // Rigidbody của xe

    private void Start()
    {
        // Gán Rigidbody cho xe
        rb = GetComponent<Rigidbody>();

        // Đặt máu ban đầu là giá trị tối đa
        currentHP = maxHP;

        // Lấy RectTransform của Handle để điều chỉnh
        handleRect = healthBar.handleRect;

        // Lưu lại chiều rộng ban đầu của Handle
        originalHandleWidth = handleRect.sizeDelta.x;

        // Đặt giá trị ban đầu cho thanh máu
        UpdateHealthBar();
    }

    void FixedUpdate()
    {
        // Lấy đầu vào di chuyển và rẽ
        dauVaoDiChuyen = Input.GetAxis("Vertical");
        dauVaoRe = Input.GetAxis("Horizontal");

        // Di chuyển và rẽ xe
        DiChuyen();
        ReXe();

        // Phanh xe khi nhấn Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PhanhXe();
        }
        else
        {
            rb.drag = 0f;  // Tắt lực cản khi không phanh
            hieuUngPhanh.SetActive(false);  // Tắt hiệu ứng phanh
        }

        // Bắn khi nhấn Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            BanSung();
        }
    }

    // Hàm di chuyển xe
    private void DiChuyen()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector3 moveForce = transform.forward * dauVaoDiChuyen * tocDoXe;
            rb.AddForce(moveForce, ForceMode.Acceleration);
        }
    }

    // Hàm rẽ xe
    private void ReXe()
    {
        float normalTurn = dauVaoRe * lucReXe * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(Vector3.up * normalTurn);
        rb.MoveRotation(rb.rotation * rotation);
    }

    // Hàm phanh xe
    private void PhanhXe()
    {
        rb.drag = Mathf.Lerp(rb.drag, 5f, Time.deltaTime * 2f);  // Tăng lực cản khi phanh
        hieuUngPhanh.SetActive(true);  // Hiển thị hiệu ứng phanh
    }

    // Hàm bắn súng
    private void BanSung()
    {
        hieuUngBan.SetActive(true);  // Kích hoạt hiệu ứng bắn
        Invoke("TatHieuUngBan", 0.1f);  // Tắt hiệu ứng sau 0.1 giây
    }

    // Tắt hiệu ứng bắn
    private void TatHieuUngBan()
    {
        hieuUngBan.SetActive(false);
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        currentHP -= damage;  // Trừ máu
        if (currentHP < 0) currentHP = 0;  // Đảm bảo máu không xuống dưới 0

        // Cập nhật thanh máu và Handle
        UpdateHealthBar();

        Debug.Log("Xe bị tấn công! Máu còn lại: " + currentHP);  // Log để kiểm tra sát thương

        if (currentHP <= 0)
        {
            Die();  // Gọi hàm Die nếu máu về 0
        }
    }

    // Cập nhật giá trị thanh máu và kích thước của Handle
    private void UpdateHealthBar()
    {
        float healthPercentage = (float)currentHP / maxHP;  // Tính toán phần trăm máu còn lại

        // Cập nhật kích thước của Handle theo phần trăm máu
        Vector2 handleSize = handleRect.sizeDelta;
        handleSize.x = originalHandleWidth * healthPercentage;  // Đặt `Handle` giảm theo kích thước ban đầu
        handleRect.sizeDelta = handleSize;

        // Cập nhật vị trí để giữ mép trái cố định
        Vector3 newPosition = handleRect.localPosition;
        newPosition.x = (originalHandleWidth - handleSize.x) / 2;  // Cập nhật vị trí để giữ mép trái cố định
        handleRect.localPosition = newPosition;
    }

    // Hàm khi xe chết
    private void Die()
    {
        Debug.Log("Player died!");  // In ra thông báo khi người chơi chết
        GameManager.Instance.KetThucGame();  // Thông báo cho GameManager kết thúc game
    }
}
