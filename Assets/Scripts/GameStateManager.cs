using Events.Bool;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool IsEditMode { get; set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetGameState(bool isEditMode)
    {
        IsEditMode = isEditMode;
    }
}