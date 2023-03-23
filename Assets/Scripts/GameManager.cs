using System;
using Level;
using ScriptableObjects;
using Ui;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [Header("Game Assets")] [SerializeField]
    private GroundCollection groundCollection;
    [SerializeField] private ColumnsCollection columnsCollection;
    [SerializeField] private BirdCollection birdCollection;
    [Space]
    [Header("Game Settings")]
    [SerializeField] private ColumnType columnType = ColumnType.GreenPipe;
    [SerializeField] private GroundType groundType = GroundType.Grass;
    [Header("UiController")]
    [SerializeField] private UiController uiController;

    private IDispatcher GroundDispatcher { get; set; }
    private IDispatcher ColumnDispatcher { get; set; }


    public State State { get; private set; }

    private int numberOfColumnsPassed;

    private void Awake()
    {
        GroundDispatcher = new GroundDispatcher(groundCollection);
        ColumnDispatcher = new ColumnDispatcher(columnsCollection);
    }

    private void Start()
    {
        GameObject groundPrefab = groundCollection.Grounds.Find(ground => ground.GroundType == groundType).Prefab;
        GameObject columnPrefab = columnsCollection.Columns.Find(column => column.ColumnType == columnType).Prefab;
        
        
        GroundDispatcher.Initialize(groundPrefab);
        ColumnDispatcher.Initialize(columnPrefab);

        State = State.WaitingToStart;
        numberOfColumnsPassed = 0;
    }

    private void Update()
    {
        if (State == State.Playing)
        {
            numberOfColumnsPassed = ColumnDispatcher.Spawn();
            GroundDispatcher.Spawn();

            uiController.OnScoreUpdate(numberOfColumnsPassed);
        }else if (State == State.BirdDead && Input.GetMouseButtonDown(0))
        {
            RestartGame();
        }
    }
    
    public void BirdOnStartedPlaying()
    {
        State = State.Playing;
        uiController.OnStartGame();
    }

    public void BirdOnDied()
    {
        State = State.BirdDead;
        float topScore = PlayerPrefs.GetFloat("Score");
        if (topScore < numberOfColumnsPassed)
        {
            PlayerPrefs.SetFloat("Score",numberOfColumnsPassed);
        }
        uiController.OnGameOver();
    }
    
    private void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}

public enum State
{
    WaitingToStart,
    Playing,
    BirdDead
}