using UnityEngine;

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
    [SerializeField] private GameObject _crosshairInteractable;

    private Interactable _currentInteraction;
    public Camera PlayerCamera;

    private void Awake()
    {
       PlayerCamera = GetComponentInChildren<Camera>();
        _crosshairInteractable.SetActive(false);
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
        if (Physics.Raycast(PlayerCamera.ViewportPointToRay(_interactionRayPoint), out RaycastHit hit, _interactionDistance))
        {

            if (hit.collider.gameObject.layer == 6 && (_currentInteraction == null || hit.collider.gameObject.GetInstanceID() != _currentInteraction.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out _currentInteraction);

                if (_currentInteraction)
                {
                    _currentInteraction.OnFocus();
                    _crosshairInteractable.SetActive(true);
                }
            }
            else if (hit.collider.gameObject.layer != 6 && (_currentInteraction != null))
            {
                _currentInteraction.OnLoseFocus();
                _crosshairInteractable.SetActive(false);
                _currentInteraction = null;
            }
        }
        else if (_currentInteraction)
        { 
            _currentInteraction.OnLoseFocus();
            _crosshairInteractable.SetActive(false);
            _currentInteraction = null;
        }
    }

    private void HandleInteractionInput()
    {
        if(Input.GetKeyDown(_interactionKey) && _currentInteraction != null && Physics.Raycast(PlayerCamera.ViewportPointToRay(_interactionRayPoint), out RaycastHit hit, _interactionDistance, _interactionLayer))
        {
            _currentInteraction.OnInteract();
            _crosshairInteractable.SetActive(false);
        }
    }
}
