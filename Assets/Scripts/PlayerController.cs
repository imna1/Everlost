using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool CanMove = true;
    [SerializeField] private GameObject _weaponObj;
    [SerializeField] private List<PreAbility> _abilities;
    private CharacteristicManager cm;
    private GameManager gm;
    private Rigidbody2D rb;
    private Weapon _weapon;
    private Camera cam;
    private Vector2 _input;
    private Vector2 _direction;
    private Vector3 _mousePosition;
    private float _timeToCharge;
    private float _chargeTime;
    private float _weaponCooldown;
    private bool _isFacingRight = true;
    private bool _canShoot;
    private int _choosingAbility = -1;
    public void FreezePlayer()
    {
        CanMove = false;
    }
    private void Start()
    {
        cm = CharacteristicManager.instance;
        gm = GameManager.instance;
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        _weapon = _weaponObj.GetComponent<Weapon>();
        _timeToCharge = _weapon.TimeToCharge;
        _weaponCooldown = _weapon.Cooldown;
        CanMove = true;
    }
    private void Update()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");
        
        _mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        _direction = transform.position - _mousePosition;
        float rotZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        if (_weapon.CanRotate && _choosingAbility == -1)
        {
            _weaponObj.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 180);
            if ((_isFacingRight && _mousePosition.x < transform.position.x) || (!_isFacingRight && _mousePosition.x > transform.position.x))
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                _weaponObj.transform.localScale = new Vector3(-_weaponObj.transform.localScale.x, -_weaponObj.transform.localScale.y, _weaponObj.transform.localScale.z);
                _isFacingRight = !_isFacingRight;
            }
        }
        if(_weaponCooldown <= 0 && _weapon.CanRotate)
        {
            if (!_canShoot)
            {
                _canShoot = true;
                if (Input.GetMouseButton(0))
                {
                    _chargeTime = _timeToCharge;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                _chargeTime = _timeToCharge;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _canShoot = false;
                if (_chargeTime <= 0)
                {
                    _weaponCooldown = _weapon.ChargedCooldown / cm.attackSpeed;
                    _weapon.ChargedAtack(_mousePosition);
                }
                else
                {
                    _weaponCooldown = _weapon.Cooldown / cm.attackSpeed;
                    _weapon.Atack();
                }
            }
        }
        if (_choosingAbility == -1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && _abilities[0].CanChoose())
            {
                _choosingAbility = 0;
                gm.Pause();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && _abilities[1].CanChoose())
            {
                _choosingAbility = 1;
                gm.Pause();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && _abilities[2].CanChoose())
            {
                _choosingAbility = 2;
                gm.Pause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(_abilities[_choosingAbility].Use(transform.position, rotZ, 0))
                {
                    _choosingAbility = -1;
                    gm.UnPause();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (_abilities[_choosingAbility].Use(transform.position, rotZ, 1))
                {
                    _choosingAbility = -1;
                    gm.UnPause();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (_abilities[_choosingAbility].Use(transform.position, rotZ, 2))
                {
                    _choosingAbility = -1;
                    gm.UnPause();
                }
            }

        }
        
        _chargeTime -= Time.deltaTime;
        _weaponCooldown -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!CanMove)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        _input.Normalize();
        float difSpeedX = _input.x * cm.moveSpeed * cm.moveSpeedinInvisibilityMultiplayer - rb.velocity.x;
        float difSpeedY = _input.y * cm.moveSpeed * cm.moveSpeedinInvisibilityMultiplayer - rb.velocity.y;
        if (_input.magnitude > 0.5)
        {
            difSpeedX *= cm.accelration;
            difSpeedY *= cm.accelration;
        }
        else
        {
            difSpeedX *= cm.deccelration;
            difSpeedY *= cm.deccelration;
        }
        rb.AddForce(new Vector2(difSpeedX, difSpeedY));
    }
}
