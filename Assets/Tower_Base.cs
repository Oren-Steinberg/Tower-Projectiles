using UnityEngine;
using UnityEngine.InputSystem;

public class Tower_Base : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Buggy buggyPrefab;
    private float buggyCreationDelay = 0.5f;
    private float lastBuggyCreation = 0f;

    void Update()
    {

        if(Time.time - lastBuggyCreation > buggyCreationDelay)
        {
            lastBuggyCreation += buggyCreationDelay;
            CreateBuggy();
        }

        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("A Main Camera is required to detect tower clicks.", this);
            return;
        }

        Ray clickRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 targetPosition;
        if (Physics.Raycast(clickRay, out RaycastHit hit))
        {
            targetPosition = hit.point;
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            targetPosition = groundPlane.Raycast(clickRay, out float distance)
                ? clickRay.GetPoint(distance)
                : clickRay.GetPoint(100f);
        }

        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned.", this);
            return;
        }

        Collider towerCollider = GetComponent<Collider>();
        float heightOffset = towerCollider != null ? towerCollider.bounds.extents.y : 0f;
        Vector3 spawnPosition = transform.position + Vector3.up * (heightOffset + 0.5f);
        Vector3 direction = targetPosition - spawnPosition;
        if (direction.sqrMagnitude == 0f)
        {
            direction = clickRay.direction;
        }

        Projectile projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.LookRotation(direction));
        projectile.Fire(direction);
    }

    void CreateBuggy()
    {
        Vector3 spawnPosition = transform.position;
        while((spawnPosition - transform.position).magnitude < 25)
        {
            spawnPosition.x += Random.Range(-50, 50);
            spawnPosition.z += Random.Range(-50, 50);
            spawnPosition.y = 0;
        }
        Vector3 direction = Random.onUnitSphere * 5;
        direction.y = 0;
        Buggy buggy = Instantiate(
            buggyPrefab,
            spawnPosition,
            Quaternion.LookRotation(direction)
        );
        buggy.Fire(direction);
    }
}
