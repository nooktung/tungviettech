using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Đối tượng đạn sẽ spawn, gán từ Inspector
    [SerializeField] private int poolSize = 20; // Kích thước pool đạn
    [SerializeField] private float bulletSpeed = 20f; // Tốc độ của đạn khi được bắn ra
    [SerializeField] private Transform firePoint; // Vị trí từ đó đạn sẽ được bắn ra

    private List<GameObject> bulletPool; // Pool các viên đạn không hoạt động

    private void Awake()
    {
        // Khởi tạo pool đạn
        bulletPool = new List<GameObject>();

        // Tạo trước một số lượng viên đạn nhất định và đưa vào pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false); // Tắt các viên đạn sau khi tạo
            bulletPool.Add(bullet); // Thêm vào danh sách pool
        }
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím Enter để bắn
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Shoot();
        }
    }

    // Phương thức bắn đạn
    private void Shoot()
    {
        // Lấy một viên đạn từ pool
        GameObject bullet = GetBulletFromPool();
        if (bullet != null)
        {
            // Đặt vị trí và hướng bắn cho viên đạn
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            // Kích hoạt viên đạn
            bullet.SetActive(true);

            // Lấy Rigidbody để áp dụng lực bắn
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero; // Đặt vận tốc ban đầu là 0 để reset
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse); // Bắn viên đạn về phía trước
        }
    }

    // Lấy một viên đạn từ pool
    private GameObject GetBulletFromPool()
    {
        // Tìm viên đạn chưa được kích hoạt trong pool
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy) // Nếu viên đạn chưa được sử dụng
            {
                return bullet;
            }
        }

        // Nếu tất cả viên đạn đều đang hoạt động, tạo thêm viên đạn mới nếu cần thiết
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false); // Tắt viên đạn sau khi tạo
        bulletPool.Add(newBullet); // Thêm vào pool
        return newBullet;
    }
}
