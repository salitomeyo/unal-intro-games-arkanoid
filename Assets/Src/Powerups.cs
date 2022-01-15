using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PowerUpType
{
    Multiball,
    Bigger,
    Smaller,
    ScorePoints
}

public class Powerups : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    [SerializeField] 
    private PowerUpType _type = PowerUpType.Multiball;
    private ArkanoidController _controller = null;
    private int _state = 1;

    public PowerUpType Type => _type;
    public int State => _state;

    private const string POWERUP_PATH = "Sprites/PowerUps/PowerUp_{0}";
    private const string POWERUP_SCOREPOINTS_PATH = "Sprites/PowerUps/PowerUp_{0}_{1}";

    static Sprite GetPowerupSprite(PowerUpType type)
    {
        string path = string.Empty;
        path = string.Format(POWERUP_PATH, type);

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return Resources.Load<Sprite>(path);
    }

    public Sprite GetScorePointsSprite(PowerUpType type)
    {
        float rand = Random.value;

        if (rand < 0.25)
        {
            _state = 1;
        }
        else if (rand < 0.50)
        {
            _state = 2;
        }
        else if (rand < 0.75)
        {
            _state = 3;
        }
        else
        {
            _state = 4;
        }

        string path = string.Empty;
        path = string.Format(POWERUP_SCOREPOINTS_PATH, type, _state);

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return Resources.Load<Sprite>(path);
    }

    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();

        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;

        _renderer = GetComponent<SpriteRenderer>();

        float rand = Random.value;
        if (rand < 0.25)
        {
            _type = PowerUpType.Multiball;
        }
        else if (rand < 0.50)
        {
            _type = PowerUpType.Bigger;
        }
        else if (rand < 0.75)
        {
            _type = PowerUpType.Smaller;
        }
        else
        {
            _type = PowerUpType.ScorePoints;
        }

        if (_type == PowerUpType.ScorePoints)
        {
            _renderer.sprite = GetScorePointsSprite(_type);
        }
        else
        {
            _renderer.sprite = GetPowerupSprite(_type);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Paddle")
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
            ArkanoidEvent.OnPowerUpsEvent?.Invoke(this);
        }
    }
}
