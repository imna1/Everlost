using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _damping;
    private Vector3 _offset;
    private Vector3 _movePosition;
    private Vector3 _velosity;
    void Start()
    {
        Invoke("EnableCam", .05f);
    }
    private void EnableCam()
    {
        _velosity = Vector3.zero;
        transform.position = new Vector3(_player.position.x, _player.position.y, transform.position.z);
        _offset = _player.position - transform.position;
    }
    void Update()
    {
        _movePosition = _player.position - _offset;
        transform.position = Vector3.SmoothDamp(transform.position, _movePosition, ref _velosity, _damping);
    }
}
