using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Controls controls;

    public Action<Vector2> OnMove;
    public Action OnActionZ;
    public Action OnActionX;
    public Action OnActionC;

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        controls = new Controls();
        controls.PlayerActions.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        controls.PlayerActions.Interact.performed += ctx => OnActionZ?.Invoke();
        controls.PlayerActions.ExInteract.performed += ctx => OnActionX?.Invoke();
        controls.PlayerActions.Menu.performed += ctx => OnActionC?.Invoke();
    }
}
