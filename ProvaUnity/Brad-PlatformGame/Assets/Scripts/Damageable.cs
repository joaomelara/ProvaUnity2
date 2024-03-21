using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [Header("Damageable properties")]
    [SerializeField] protected int lives;

    public abstract void TakeDamage();
}
