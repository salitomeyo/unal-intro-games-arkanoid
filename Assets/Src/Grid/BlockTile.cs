using UnityEngine;
using Random = UnityEngine.Random;

public enum BlockType
{
    Small,
    Big
}

public enum BlockColor
{
    Green,
    Blue,
    Orange,
    Red,
    Purple,
    Cyan,
    Brown,
    Yellow,
    Gray,
    Lime
}

public class BlockTile : MonoBehaviour
{
    [SerializeField]
    private int _score = 10;
    
    public int Score => _score;
    
    private const string BLOCK_BIG_PATH = "Sprites/BlockTiles/Big/Big_{0}_{1}";

    [SerializeField] private BlockColor _color = BlockColor.Blue;
    [SerializeField] private BlockType _type = BlockType.Big;

    private SpriteRenderer _renderer;
    private Collider2D _collider;
    
    private int _totalHits = 1;
    private int _currentHits = 0;
    private int _id;
    
    public void SetData(int id, BlockColor color)
    {
        _id = id;
        _color = color;
    }

    static Sprite GetBlockSprite(BlockType type, BlockColor color, int state)
    {
        string path = string.Empty;
        if (type == BlockType.Big)
        {
            path = string.Format(BLOCK_BIG_PATH, color, state);
        }

        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return Resources.Load<Sprite>(path);
    }

    public void Init()
    {
        _currentHits = 0;
        _totalHits = _type == BlockType.Big ? 2 : 1;

        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;

        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.sprite = GetBlockSprite(_type, _color, 0);
    }

    public void OnHitCollision(ContactPoint2D contactPoint)
    {
        _currentHits++;
        if (_currentHits >= _totalHits)
        {
            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;

            _collider.enabled = false;
            gameObject.SetActive(false);
            ArkanoidEvent.OnBlockDestroyedEvent?.Invoke(_id);

            if (Random.value < 0.25)
            {
                Powerups powerupPrefab = Resources.Load<Powerups>("Prefabs/PowerUpCapsule");

                Powerups powerup = Instantiate<Powerups>(powerupPrefab, new Vector3(x, y, 0), Quaternion.identity);
                
                powerup.Init();
            }
        }
        if (_currentHits == 1)
        {
            _renderer.sprite = GetBlockSprite(_type, _color, 1);
        }
    }
}
