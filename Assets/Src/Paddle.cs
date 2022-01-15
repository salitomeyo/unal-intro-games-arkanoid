using UnityEngine;
public class Paddle : MonoBehaviour
{
    [SerializeField] private float _speed = 8;
    [SerializeField] private float _movementLimit = 7;

    private float _scale = 1;

    private Vector3 _targetPosition;

    private Camera _cam;
    private Camera Camera
    {
        get
        {
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }

    void Update()
    {
        _targetPosition.x = Camera.ScreenToWorldPoint(Input.mousePosition).x;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_movementLimit, _movementLimit);
        _targetPosition.y = this.transform.position.y;
        
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed);
    }

    public void SetSize(float add)
    {
        _scale = _scale + add;
        Vector3 scaleChange = new Vector3(add, 0.0f, 0.0f);
        transform.localScale += scaleChange;
    }
}