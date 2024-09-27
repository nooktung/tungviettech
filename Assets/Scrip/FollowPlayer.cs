using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float speed = 15f; // Tốc độ di chuyển của enemy
    private Transform player; // Đối tượng người chơi mà enemy sẽ theo dõi
    private float groundY; // Chiều cao mặt đất để giữ enemy không bay lên
    public float stopDistance = 0.5f; // Khoảng cách tối thiểu giữa enemy và xe

    private void Awake()
    {
        // Gán đối tượng người chơi (có thể là tag "Player") cho biến player
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy đối tượng người chơi.");
        }

        // Lấy vị trí Y (chiều cao) hiện tại của enemy, giả sử mặt đất là Y hiện tại
        groundY = transform.position.y;
    }

    // Update được gọi mỗi khung hình
    private void FixedUpdate()
    {
        if (player != null)
        {
            Follow(); // Gọi hàm để enemy theo dõi người chơi
        }
    }

    private void Follow()
    {
        // Lấy vị trí của người chơi
        Vector3 playerPos = player.position;

        // Cố định vị trí Y của enemy để nó không bay lên, chỉ di chuyển trên mặt đất
        playerPos.y = groundY;

        // Kiểm tra khoảng cách giữa enemy và người chơi
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        // Nếu khoảng cách lớn hơn stopDistance, enemy sẽ tiếp tục di chuyển

            // Di chuyển enemy đến cạnh người chơi
            transform.position = Vector3.MoveTowards(
                transform.position,
                playerPos,
                this.speed * Time.fixedDeltaTime
            );

            // Enemy nhìn vào người chơi
            transform.LookAt(new Vector3(player.position.x, groundY, player.position.z)); // Giữ enemy nhìn vào player theo trục XZ
    }
}
