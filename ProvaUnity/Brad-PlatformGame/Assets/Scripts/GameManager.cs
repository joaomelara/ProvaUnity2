using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public InputManager inputManager;
    [SerializeField] private UIManager uiManager;
    private int enemiesLeftCount = 0;
    
    public event Action OnPlayerGetHurt;
    public event Action OnEnemyDie;
    public event Action OnPlayerDie;
    public event Action OnAllEnemiesDie;
    
    private void Awake()
    {
        if(Instance != null) Destroy(this.gameObject);

        Instance = this;
        inputManager = new InputManager();

        enemiesLeftCount = FindObjectsOfType<Enemy>().Length;
        uiManager.SetupEnemiesCountText(enemiesLeftCount);
    }

    public void InvokeOnPlayerDieEvent()
    {
        inputManager.DisableInput();
        OnPlayerDie?.Invoke();
    }

    public void InvokeOnPlayerGetHurtEvent()
    {
        print("Invoking event");
        OnPlayerGetHurt?.Invoke();
    }

    public void InvokeOnEnemyDieEvent()
    {
        inputManager.DisableInput();
        enemiesLeftCount--;
        OnEnemyDie?.Invoke();
        CheckAndHandleEnemiesDead();
    }

    private void CheckAndHandleEnemiesDead()
    {
        if (enemiesLeftCount <= 0)
        {
            OnAllEnemiesDie?.Invoke();
        }
    }
}
