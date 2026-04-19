using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private int updates = 0;
    [SerializeField] private int direction = 1;
    [SerializeField] private int maxUpdates = 500;
    [SerializeField] private float speed = 0.01f;

    private void Update()
    {
        updates += direction;
        if (Mathf.Abs(updates) > maxUpdates)
        {
            direction *= -1;
        }

        gameObject.transform.position = transform.position + new Vector3(0, 0, speed * direction);
    }
}
