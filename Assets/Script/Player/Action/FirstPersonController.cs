using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMov { get; private set; } = true;

    private bool IsSprinting => _canSprint && Input.GetKey(_sprintKey);
    private bool ShouldJump => Input.GetKeyDown(_jumptKey) && _characterController.isGrounded;
    private bool ShouldCrouch => Input.GetKeyDown(_crouchKey) && !_duringCrouchAnimation && _characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool _canSprint = true;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canCrouch = true;
    [SerializeField] private bool _canUseHeadbob = true;
    [SerializeField] private bool _willSlideOnSlopes= true;
    [SerializeField] private bool _canZoom = true;
    [SerializeField] private bool _canStamina;

    [Header("Control")]
    [SerializeField] private KeyCode _sprintKey;
    [SerializeField] private KeyCode _jumptKey;
    [SerializeField] private KeyCode _zoomKey;
    [SerializeField] private KeyCode _crouchKey;

    [Header("Movement Parameters")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _slopeSpeed;

    [Header("Stamina Parameters")]
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _staminaUseMultiplier;
    [SerializeField] private float _timeBeforeStaminaRegenStarts;
    [SerializeField] private float _staminaValueIncrement;
    [SerializeField] private float _staminaTimeIncrement;
    [SerializeField] private Slider _staminaSlider;

    private float _currentStamina;
    private Coroutine _regeneratingStamina;
    public static Action<float> OnStaminaChange;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float _lookSpeedX; 
    [SerializeField, Range(1, 10)] private float _lookSpeedY;
    [SerializeField, Range(1, 100)] private float _upperLookLimit;
    [SerializeField, Range(1, 100)] private float _lowerLookLimit;

    [Header("Jumping Parameters")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;

    [Header("Crouch Parameters")]
    [SerializeField] private float _crouchHeight;
    [SerializeField] private float _standingHeight;
    [SerializeField] private float _timeToCrouch;
    [SerializeField] private Vector3 _crouchingCenter;
    [SerializeField] private Vector3 _standingCenter;

    [Header("Headbob Parameters")]
    [SerializeField] private float _walkBobSpeed;
    [SerializeField] private float _walkBobAmount;
    [SerializeField] private float _sprintBobSpeed;
    [SerializeField] private float _sprintBobAmount;
    [SerializeField] private float _crouchBobSpeed;
    [SerializeField] private float _crouchBobAmount;

    [Header("Zoom Parameters")]
    [SerializeField] private float _timeToZoom;
    [SerializeField] private float _zoomFOV;

    [Header("Objects")]
    [SerializeField] private Camera _handCamera;

    //SLIDING PARAMETERS
    private Vector3 _hitPointNormal;
    private bool IsSliding
    {
        get
        {
            if(_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                _hitPointNormal = slopeHit.normal;
                return Vector3.Angle(_hitPointNormal, Vector3.up) > _characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    private float _defaulthFOV;
    private Coroutine _zoomRuntime;

    private float _defaultYPos = 0f;
    private float _timer;

    private bool _isCrouching;
    private bool _duringCrouchAnimation;

    private Camera _playerCamera;
    private CharacterController _characterController;

    private Vector3 _moveDirection;
    private Vector3 _currentInput;

    private float _rotationX = 0f;

    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();

        _defaultYPos = _playerCamera.transform.localPosition.y;
        _defaulthFOV = _playerCamera.fieldOfView;

        _currentStamina = _maxStamina;
        _staminaSlider.maxValue = _maxStamina;
        _staminaSlider.value = _currentStamina;
        _staminaSlider.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if(CanMov)
        {
            if(_canJump)
            {
                HandleJump();
            }
            
            if(_canCrouch)
            {
                HandlCrouch();
            }

            if(_canUseHeadbob)
            {
                HandlHeadBob();
            }

            if (_canZoom)
            {
                HandlZoom();
            }

            if (_canStamina)
            {
                HandleStamina();
            }

            HandleMovementInput();
            HandleMouseLook();
            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        _currentInput = new Vector2(
            (_isCrouching ? _crouchSpeed : IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Vertical"),
            (_isCrouching ? _crouchSpeed : IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = _moveDirection.y;
        _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x) + (transform.TransformDirection(Vector3.right) * _currentInput.y);
        _moveDirection.y = moveDirectionY;
    }

    private void HandleStamina()
    {
        if (!_isCrouching && IsSprinting && _currentInput != Vector3.zero)
        {
            if(_regeneratingStamina != null)
            {
                StopCoroutine(_regeneratingStamina);
                _regeneratingStamina = null;
            }

            _currentStamina -= _staminaUseMultiplier * Time.deltaTime;
            _staminaSlider.gameObject.SetActive(true);

            if (_currentStamina < 0)
            {
                _currentStamina = 0;
            }

            OnStaminaChange?.Invoke(_currentStamina);

            if (_currentStamina <= 0)
            {
                _canSprint = false;
                _canJump = false;
            }
        }

        if (!IsSprinting && _currentStamina < _maxStamina && _regeneratingStamina == null)
        {
            _regeneratingStamina = StartCoroutine(RegenerateStamina());
        }

        if (_currentStamina >= 100)
        {
            _staminaSlider.gameObject.SetActive(false);
        }

        _staminaSlider.value = _currentStamina;
    }

    private void HandleJump()
    {
        if(_currentStamina >= 20 && !_isCrouching)
        {
            if (ShouldJump)
            {
                _moveDirection.y = _jumpForce;
                _currentStamina -= 20;
                _staminaSlider.gameObject.SetActive(true);


                if (_regeneratingStamina != null)
                {
                    StopCoroutine(_regeneratingStamina);
                    _regeneratingStamina = null;
                }
            }
        }
    }

    private void HandlZoom()
    {
        if (Input.GetKeyDown(_zoomKey))
        {
            if (_zoomRuntime != null)
            {
                StopCoroutine(_zoomRuntime);
                _zoomRuntime = null;
            }

            _zoomRuntime = StartCoroutine(ToggleZoom(true));
        }

        if (Input.GetKeyUp(_zoomKey))
        {
            if (_zoomRuntime != null)
            {
                StopCoroutine(_zoomRuntime);
                _zoomRuntime = null;
            }

            _zoomRuntime = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandlCrouch()
    {
        if(ShouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandlHeadBob()
    {
        if (!_characterController.isGrounded) return;

        if(Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            _timer += Time.deltaTime * (_isCrouching ? _crouchBobSpeed : IsSprinting ? _sprintBobSpeed : _walkBobSpeed);
            _playerCamera.transform.localPosition = new Vector3(
                _playerCamera.transform.localPosition.x,
                _defaultYPos + Mathf.Sin(_timer) * (_isCrouching ? _crouchBobAmount : IsSprinting ? _sprintBobAmount : _walkBobAmount),
                _playerCamera.transform.localPosition.z);
        }
    }

    private void HandleMouseLook()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * _lookSpeedY;
        _rotationX = Mathf.Clamp(_rotationX, -_upperLookLimit, _lowerLookLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeedX, 0);
    }

    private void ApplyFinalMovements()
    {
        if(!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        if(_willSlideOnSlopes && IsSliding)
        {
            _moveDirection = new Vector3(_hitPointNormal.x, -_hitPointNormal.y, _hitPointNormal.z) * _slopeSpeed;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if(_isCrouching && Physics.Raycast(_playerCamera.transform.position, Vector3.up, 1f))
        {
            yield break;
        }

        _duringCrouchAnimation = true;

        float timeElapse = 0f;
        float targetHeight = _isCrouching ? _standingHeight : _crouchHeight;
        float currentHeight = _characterController.height;
        Vector3 targetCenter = _isCrouching ? _standingCenter : _crouchingCenter;
        Vector3 currentCenter = _characterController.center;

        while(timeElapse < _timeToCrouch)
        {
            _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapse/_timeToCrouch);
            _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapse/_timeToCrouch);
            timeElapse += Time.deltaTime;
            yield return null;
        }

        _characterController.height = targetHeight;
        _characterController.center = targetCenter;

        _isCrouching = !_isCrouching;

        _duringCrouchAnimation = false;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? _zoomFOV : _defaulthFOV;
        float startingFOV = _playerCamera.fieldOfView;
        float timeElapse = 0;

        while (timeElapse < _timeToZoom)
        {
            _playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapse / _timeToZoom);
            _handCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapse / _timeToZoom);
            timeElapse += Time.deltaTime;
            yield return null;
        }

        _playerCamera.fieldOfView = targetFOV;
        _handCamera.fieldOfView= targetFOV;
        _zoomRuntime = null;
    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(_timeBeforeStaminaRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(_staminaTimeIncrement);

        while(_currentStamina < _maxStamina)
        {
            if(_currentStamina > 0)
            {
                _canSprint = true;

                if(_currentStamina > 20)
                {
                    _canJump = true;
                }
            }

            _currentStamina += _staminaValueIncrement;


            if (_currentStamina > _maxStamina)
            {
                _currentStamina = _maxStamina;
            }

            OnStaminaChange?.Invoke(_currentStamina);

            yield return timeToWait;
        }

        _regeneratingStamina = null;
    }

}
