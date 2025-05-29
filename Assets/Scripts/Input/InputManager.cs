using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }
    
    private RunnerInputAction _actionScheme;
    
    // Configuration
    [SerializeField] private float swipeThreshold = 50f;

    #region Private Variables
    private bool _tap;
    private Vector2 _touchPosition;
    private Vector2 _startDragPosition;
    private bool _swipeLeft;
    private bool _swipeRight;
    private bool _swipeUp;
    private bool _swipeDown;
    #endregion

    #region Public Properties
    public bool Tap { get { return _tap; } }
    public Vector2 TouchPosition { get { return _touchPosition; } }
    public bool SwipeLeft { get { return _swipeLeft; } }
    public bool SwipeRight { get { return _swipeRight; } }
    public bool SwipeUp { get { return _swipeUp; } }
    public bool SwipeDown { get { return _swipeDown; } }
    #endregion
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }

    private void LateUpdate()
    {
        ResetInputs();
    }

    private void ResetInputs()
    {
        _tap = _swipeDown = _swipeUp = _swipeLeft = _swipeRight = false;
    }

    private void SetupControl()
    {
        _actionScheme = new RunnerInputAction();
        
        // Registering the action
        _actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        _actionScheme.Gameplay.TouchPosition.performed += ctx => OnTouchPosition(ctx);
        _actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        _actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);
    }
    
    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = _touchPosition - _startDragPosition;
        float sqrDistance = delta.sqrMagnitude;

        if (sqrDistance > swipeThreshold)
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);
            
            if (x > y) // Swipe horizontal
            {
                if (delta.x > 0)
                {
                    _swipeRight = true;
                }
                else
                {
                    _swipeLeft = true;
                }
            }
            else // Swipe vertical
            {
                if (delta.y > 0)
                {
                    _swipeUp = true;
                }
                else
                {
                    _swipeDown = true;
                }
            }
        }
    }
    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        _startDragPosition = _touchPosition;
    }
    private void OnTouchPosition(InputAction.CallbackContext ctx)
    {
        _touchPosition = ctx.ReadValue<Vector2>();
    }
    private void OnTap(InputAction.CallbackContext ctx)
    {
        _tap = true;
    }
    
    public void OnEnable()
    {
        _actionScheme.Enable();
    }
    public void OnDisable()
    {
        _actionScheme.Disable();
    }
}
