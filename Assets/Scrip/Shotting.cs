using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotting : MonoBehaviour
{
    public float bulletSpeed = 20f;  // Tốc độ bắn của đạn
    
    // Hàm này sẽ được gọi sau khi đạn được lấy từ BulletPool và kích hoạt
    public void ApplyBulletPhysics(GameObject bullet, Transform firePoint)
    {
        if (bullet == null)
        {
            Debug.LogError("Bullet is null!");
            return;
        }

        // Đặt vị trí và hướng cho viên đạn
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        // Kích hoạt đạn
        bullet.SetActive(true);

        // Lấy thành phần Rigidbody của đạn để áp dụng lực bắn
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Reset vận tốc về 0
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse); // Bắn đạn về phía trước
        }
        else
        {
            Debug.LogError("Rigidbody component is missing on the bullet.");
        }
    }
}
