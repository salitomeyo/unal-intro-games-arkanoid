using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkanoidController : MonoBehaviour
{
    [SerializeField] private GridController _gridController;
    
    [Space(20)]
    [SerializeField] private List<LevelData> _levels = new List<LevelData>();
    
    private const string BALL_PREFAB_PATH = "Prefabs/Ball";
    private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f);

    private Ball _ballPrefab = null;
    private List<Ball> _balls = new List<Ball>();

    [SerializeField]
    private Paddle _paddle;
        
    private int _totalScore = 0;

    private void Start()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
        ArkanoidEvent.OnPowerUpsEvent += onPowerUps;
    }

    private void OnDestroy()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
        ArkanoidEvent.OnPowerUpsEvent -= onPowerUps;
    }

    private void InitGame()
    {
        _currentLevel = 0;
        _totalScore = 0;
        _gridController.BuildGrid(_levels[0]);
        SetInitialBall();
        ArkanoidEvent.OnGameStartEvent?.Invoke();
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(0, _totalScore);
    }

    private void SetInitialBall()
    {
        ClearBalls();
        Ball ball = CreateBallAt(BALL_INIT_POSITION);
        ball.Init();
        _balls.Add(ball);
    }

    private void SetBall()
    {
        Ball ball = CreateBallAt(BALL_INIT_POSITION);
        ball.Init();
        _balls.Add(ball);
    }

    private Ball CreateBallAt(Vector2 position)
    {
        if (_ballPrefab == null)
        {
            _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        }
        return Instantiate(_ballPrefab, position, Quaternion.identity);
    }

    private void ClearBalls()
    {
        for (int i = _balls.Count - 1; i >= 0; i--)
        {
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i]);
        }
    
        _balls.Clear();
    }

    private int _currentLevel = 0;
        
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitGame();
        }
    }

    private void OnBallReachDeadZone(Ball ball)
    {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        CheckGameOver();
    }
    
    private void CheckGameOver()
    {
        if (_balls.Count == 0)
        {
            ClearBalls();
            
            Debug.Log("Game Over: LOSE!!!");
            ArkanoidEvent.OnGameOverEvent?.Invoke();
        }
    }

    private void OnBlockDestroyed(int blockId)
    {
        BlockTile blockDestroyed = _gridController.GetBlockBy(blockId);
        if (blockDestroyed != null)
        {
            _totalScore += blockDestroyed.Score;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(blockDestroyed.Score, _totalScore);
        }

        if (_gridController.GetBlocksActive() == 0)
        {
            _currentLevel++;
            if (_currentLevel >= _levels.Count)
            {
                ClearBalls();
                Debug.LogError("Game Over: WIN!!!!");
            }
            else
            {
                SetInitialBall();
                _gridController.BuildGrid(_levels[_currentLevel]);
            }

        }
    }

    private void onPowerUps(Powerups powerup)
    {
        if (powerup.Type == PowerUpType.Slow)
        {
            Debug.Log(_balls[0].rigbod.velocity.magnitude);
            _balls[0].SetSpeed(-5);
            Debug.Log(_balls[0].rigbod.velocity.magnitude);
        }
        else if (powerup.Type == PowerUpType.Fast)
        {
            Debug.Log(_balls[0].rigbod.velocity.magnitude);
            _balls[0].SetSpeed(5);
            Debug.Log(_balls[0].rigbod.velocity.magnitude);
        }
        else if (powerup.Type == PowerUpType.Multiball)
        {
            if (_balls.Count == 1)
            {
                SetBall();
                SetBall();
            }
            else if (_balls.Count == 2)
            {
                SetBall();
            }
            else {}
        }
        else if (powerup.Type == PowerUpType.Bigger)
        {
            _paddle.SetSize(0.2f);
        }
        else
        {
            _paddle.SetSize(-0.2f);
        }
    }
}

