using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float tocDoXe = 20f;  // Tốc độ di chuyển của xe
    [SerializeField]
    private float lucReXe = 30f;  // Lực rẽ của xe
    [SerializeField]
    private float lucKeoXe = 1.5f;  // Lực kéo giúp xe ổn định
    [SerializeField]
    private float lucPhanh = 10f;  // Lực phanh khi nhấn Shift
    [SerializeField]
    private float maxSpeed = 30f;  // Tốc độ tối đa của xe
    [SerializeField]
    private GameObject hieuUngPhanh;  // Hiệu ứng phanh
    [SerializeField]
    private GameObject hieuUngBan;  // Hiệu ứng bắn

    private float dauVaoDiChuyen;  // Đầu vào di chuyển (W/S)
    private float dauVaoRe;  // Đầu vào rẽ (A/D)
    private Rigidbody rb;  // Rigidbody của xe

    public int maxHP = 100;  // Máu tối đa của xe
    public int currentHP;  // Máu hiện tại của xe

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;  // Đặt máu ban đầu là giá trị tối đa
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
    public void DiChuyen()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector3 moveForce = transform.forward * dauVaoDiChuyen * tocDoXe;
            rb.AddForce(moveForce, ForceMode.Acceleration);
        }
    }

    // Hàm rẽ xe
    public void ReXe()
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
        Debug.Log("Xe bị tấn công! Máu còn lại: " + currentHP);  // Log để kiểm tra sát thương

        if (currentHP <= 0)
        {
            Die();  // Gọi hàm Die nếu máu về 0
        }
    }

    // Hàm khi xe chết
    private void Die()
    {
        Debug.Log("Player died!");  // In ra thông báo khi người chơi chết
        GameManager.Instance.KetThucGame();  // Thông báo cho GameManager kết thúc game
    }
}
