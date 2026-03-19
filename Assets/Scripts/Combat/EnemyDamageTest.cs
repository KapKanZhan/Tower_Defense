using UnityEngine;

public class EnemyDamageTest : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

            foreach (EnemyHealth enemy in enemies)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}