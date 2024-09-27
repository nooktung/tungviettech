using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;           // Singleton để dễ truy cập
    public GameObject bulletPrefab;              // Prefab của đạn
    public int poolSize = 20;                    // Kích thước pool
    private List<GameObject> bulletPool;         // Danh sách các đối tượng đạn trong pool

    private void Awake()
    {
        // Singleton để có thể gọi BulletPool từ bất cứ đâu
        Instance = this;
    }

    private void Start()
{
    if (bulletPrefab == null)
    {
        Debug.LogError("Bullet prefab is not assigned in BulletPool.");
        return;
    }

    // Khởi tạo Object Pool
    bulletPool = new List<GameObject>();

    for (int i = 0; i < poolSize; i++)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);             // Tắt đối tượng sau khi tạo
        bulletPool.Add(bullet);              // Thêm đạn vào danh sách pool
    }
}


    // Lấy một viên đạn từ pool
    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)       // Nếu viên đạn chưa được sử dụng
            {
                return bullet;                   // Trả về viên đạn chưa được sử dụng
            }
        }

        // Nếu tất cả đạn đang được sử dụng, có thể mở rộng pool nếu cần thiết
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }
}
