using System.Collections;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public float walkDistance = 50f;
    public float runDistance = 1.5f;
    public float attackDistance = 10f;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float fallbackSpeed = 4f;
    public float attackCooldown = 1.5f;
    public int health = 3;
    public int damage = 5;

    private Transform car;
    private Animator animator;
    private bool isFallback = false;
    private float lastAttackTime;
    private Rigidbody rb;

    public float knockbackDistance = 3f;
    public float knockbackHeight = 3f;
    public float knockbackForce = 3f;

    public float raycastCooldown = 1f;
    private float nextRaycastTime = 0f;

    private void Awake()
    {
        GameObject carObj = GameObject.FindWithTag("Player");
        if (carObj != null)
        {
            car = carObj.transform;
        }
        else
        {
            Debug.LogError("Car object not found.");
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
{
    if (health <= 0)
    {
        StartCoroutine(FallAndDisappear());
        return;
    }

    if (car != null)
    {
        float distance = Vector3.Distance(transform.position, car.position);

        if (distance <= 1.5f)
        {
            Attack();
        }
        else if (distance <= 40f)
        {
            Run();
        }
        else
        {
            Walk();
        }

        // Perform Raycast every second
        if (Time.time >= nextRaycastTime)
        {
            nextRaycastTime = Time.time + raycastCooldown;
            PerformRaycast();
        }
    }
}

private IEnumerator FallAndDisappear()
{
    // Kích hoạt animation FallBack
    animator.SetTrigger("FallBack");

    // Đợi 1.5 giây để kẻ địch hoàn thành animation FallBack
    yield return new WaitForSeconds(1.5f);

    // Sau đó biến mất (deactivate)
    gameObject.SetActive(false);

    // Spawn kẻ địch mới nếu cần
    EnemySpawner.Instance.SpawnNewEnemy();
}


    private void PerformRaycast()
    {
        RaycastHit hit;
        Vector3 directionToCar = car.position - transform.position;

        // Log để kiểm tra xem raycast có được thực hiện hay không
        Debug.Log("Đang bắn Raycast về phía người chơi");

        // Thực hiện raycast về hướng xe (người chơi)
        if (Physics.Raycast(transform.position, directionToCar, out hit, runDistance))
        {
            // Kiểm tra xem raycast có trúng xe không
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Raycast đã trúng xe!");

                // Áp dụng sát thương cho xe
                CarController carController = hit.collider.GetComponent<CarController>();
                if (carController != null)
                {
                    Debug.Log("Đang gây sát thương cho người chơi!");
                    carController.TakeDamage(damage);  // Gây sát thương cho xe
                }
            }
            else
            {
                Debug.Log("Raycast không trúng người chơi.");
            }
        }
    }

    private void KnockbackFromCar()
    {
        Vector3 knockbackDirection = (transform.position - car.position).normalized;
        Vector3 knockback = new Vector3(knockbackDirection.x * knockbackDistance, knockbackHeight, knockbackDirection.z * knockbackDistance);
        rb.AddForce(knockback, ForceMode.Impulse);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        EnemySpawner.Instance.SpawnNewEnemy();
    }

    private void Walk()
    {
        MoveTowardsCar(walkSpeed);
    }

    private void Run()
    {
        MoveTowardsCar(runSpeed);
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("Enemy attacking the player!");
        }
    }

    private void Fallback()
    {
        Vector3 direction = (transform.position - car.position).normalized;
        transform.position += direction * fallbackSpeed * Time.fixedDeltaTime;
    }

    private void FallBack()
    {
        Debug.Log("Enemy is dead!");
    }

    private void MoveTowardsCar(float speed)
    {
        Vector3 direction = (car.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, car.position, speed * Time.fixedDeltaTime);
    }

    // Reset the enemy's health and status when it is spawned from the pool
    public void ResetEnemy()
    {
        health = 3;
        isFallback = false;
    }
}
