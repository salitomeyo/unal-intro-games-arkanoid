using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PowerUpType
{
    Slow,
    Fast,
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
    private PowerUpType _type = PowerUpType.Slow;
    private ArkanoidController _controller = null;

    public PowerUpType Type => _type;

    private const string POWERUP_PATH = "Sprites/PowerUps/PowerUp_{0}";

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

    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();

        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;

        _renderer = GetComponent<SpriteRenderer>();

        float rand = Random.value;
        if (rand < 0.20)
        {
            _type = PowerUpType.Slow;
        }
        else if (rand < 0.40)
        {
            _type = PowerUpType.Fast;
        }
        else if (rand < 0.60)
        {
            _type = PowerUpType.Multiball;
        }
        else if (rand < 0.80)
        {
            _type = PowerUpType.Bigger;
        }
        else
        {
            _type = PowerUpType.Smaller;
        }
        _renderer.sprite = GetPowerupSprite(_type);
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
