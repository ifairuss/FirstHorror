using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class InteractableManager : MonoBehaviour
{
    [Header("Functional Option")]
    [SerializeField] private bool _canInteract = true;

    [Header("Control")]
    [SerializeField] private KeyCode _interactionKey;

    [Header("Interaction")]
    [SerializeField] private Vector3 _interactionRayPoint = default;
    [SerializeField] private float _interactionDistance = default;
    [SerializeField] private LayerMask _interactionLayer = default;

    private Interactable _currentInteraction;
    private Camera _playerCamera;

    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        InteractionManager();
    }

    private void InteractionManager()
    {
        if (_canInteract)
        {
            HandleInteractionCheck();
            HandleInteractionInput();
        }
    }

    private void HandleInteractionCheck()
    {
        if(Physics.Raycast(_playerCamera.ViewportPointToRay(_interactionRayPoint), out RaycastHit hit, _interactionDistance))
        {
            if (hit.collider.gameObject.layer == 6 && (_currentInteraction == null || hit.collider.gameObject.GetInstanceID() != _currentInteraction.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out _currentInteraction);

                if(_currentInteraction)
                {
                    _currentInteraction.OnFocus();
                }
            }   
        }
        else if (_currentInteraction)
        {
            _currentInteraction.OnLoseFocus();
            _currentInteraction = null;
        }
    }

    private void HandleInteractionInput()
    {
        if(Input.GetKey(_interactionKey) && _currentInteraction != null && Physics.Raycast(_playerCamera.ViewportPointToRay(_interactionRayPoint), out RaycastHit hit, _interactionDistance, _interactionLayer))
        {
            _currentInteraction.OnInteract();
        }
    }
}
