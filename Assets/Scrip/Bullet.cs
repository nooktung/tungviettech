using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Sát thương của đạn
    private float maxDistance = 100f; // Khoảng cách tối đa đạn có thể di chuyển
    private Vector3 startPosition;    // Vị trí bắt đầu của viên đạn
    private Rigidbody rb;             // Thành phần Rigidbody của viên đạn

    public ParticleSystem hitEffect; // Hiệu ứng khi đạn biến mất (va chạm)

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Tắt trọng lực để đạn không bị rơi
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Bật chế độ phát hiện va chạm liên tục
    }

    private void OnEnable()
    {
        // Khi đạn được kích hoạt, lưu vị trí bắt đầu
        startPosition = transform.position;
    }

    private void Update()
    {
        // Kiểm tra khoảng cách đã di chuyển
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            // Hiển thị hiệu ứng khi đạn biến mất
            PlayHitEffect();
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // Đặt lại vận tốc và lực quay khi đạn bị vô hiệu hóa
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu đạn va chạm với Target
        if (collision.gameObject.CompareTag("Target"))
        {
            // Gọi hàm TakeDamage của đối tượng Target (ví dụ EnemyCtrl) để giảm máu
            EnemyCtrl enemy = collision.gameObject.GetComponent<EnemyCtrl>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Hiển thị hiệu ứng khi đạn va chạm
            PlayHitEffect();

            // Vô hiệu hóa viên đạn sau khi va chạm với Target
            gameObject.SetActive(false);
        }
        else
        {
            // Hiển thị hiệu ứng khi đạn va chạm
            PlayHitEffect();
            // Vô hiệu hóa đạn khi va chạm với các đối tượng khác
            gameObject.SetActive(false);
        }
    }

    // Hàm để hiển thị hiệu ứng khi đạn biến mất
    private void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            // Kích hoạt hiệu ứng particle tại vị trí hiện tại
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration); // Xóa hiệu ứng sau khi nó hoàn thành
        }
    }
}
