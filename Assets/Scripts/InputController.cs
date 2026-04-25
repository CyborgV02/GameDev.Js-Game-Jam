using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{
    private Controls controls;

    public static Action<Vector2> OnMove;
    public static Action OnActionZ;
    public static Action OnActionX;
    public static Action OnActionC;
    public static Action<int> OnNavigate;   
    public static Action OnConfirm;

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Awake()
{
    controls = new Controls();
    
    UnityEngine.InputSystem.InputSystem.settings.updateMode = 
    UnityEngine.InputSystem.InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    controls.PlayerActions.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
    controls.PlayerActions.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
    controls.PlayerActions.Interact.performed += ctx => OnActionZ?.Invoke();
    controls.PlayerActions.ExInteract.performed += ctx => OnActionX?.Invoke();
    controls.PlayerActions.Menu.performed += ctx => OnActionC?.Invoke();
}
}