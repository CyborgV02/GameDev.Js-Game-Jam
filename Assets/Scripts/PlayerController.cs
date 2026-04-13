using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
<<<<<<< Updated upstream

    // Update is called once per frame
    void Update()
    {
        
=======
    void Update()
    {
        playerRb.velocity=moveInput*moveSpeed;
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveInput=context.ReadValue<Vector2>();
>>>>>>> Stashed changes
    }
}