using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;


    #region Properties

    public Vector2 PrimaryFingerPosition => inputActions.Player.PrimaryFingerPosition.ReadValue<Vector2>();
    public Vector2 SecondaryFingerPosition => inputActions.Player.SecondaryFingerPosition.ReadValue<Vector2>();
    
    #endregion
    
    
    /// <summary>
    /// UI Actions
    /// </summary>
    public event UnityAction<Vector2> Navigate = delegate { };

    public event UnityAction<Vector2> Point = delegate { };
    public event UnityAction<Vector2> ScrollWheel = delegate { };
    public event UnityAction Submit = delegate { };
    public event UnityAction Cancel = delegate { };
    public event UnityAction Click = delegate { };

    //Gameplay Action
    public event UnityAction<bool> Hold = delegate { };
    public event UnityAction DoubleTap = delegate { };
    public event UnityAction ZoomStart = delegate { };
    public event UnityAction ZoomEnd = delegate { };

    /// <summary>
    /// Create InputAction class if not exist and set the callbacks
    /// </summary>
    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.SetCallbacks(this);
        }

        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }


    #region GameInputEvents

    public void OnDoubleTap(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            DoubleTap.Invoke();
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                Hold.Invoke(true);
                break;

            case InputActionPhase.Canceled:
                Hold.Invoke(false);
                break;
        }
    }

    public void OnSecondaryTouchContact(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                ZoomStart.Invoke();
                break;

            case InputActionPhase.Canceled:
               ZoomEnd.Invoke();
                break;
        }
    }
    
    public void OnPrimaryFingerPosition(InputAction.CallbackContext context)
    {
    }

    public void OnSecondaryFingerPosition(InputAction.CallbackContext context)
    {
    }
    
    



    #endregion

    #region UIInputEvents

    public void OnNavigate(InputAction.CallbackContext context)
    {
        Navigate.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        Point.Invoke(context.ReadValue<Vector2>());
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        ScrollWheel.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        Submit.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            Cancel.Invoke();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            Click.Invoke();
    }

    //These were UI events, wont be used but have to be implemented
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }

    #endregion
}
