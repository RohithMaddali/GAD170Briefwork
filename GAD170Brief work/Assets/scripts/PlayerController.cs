using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterController controller;

    public float speed;
    public float jumpSpeed;
    public float gravity;

    private Vector3 moveDirection = Vector3.zero;
    //sets up vector to (0,0,0) because we dont want it to be default which is (null,null,null)
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if we are on the ground
        if (controller.isGrounded)
        {
            //if we are check inputs for rotation/movement
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //move us in that direction * speed
            moveDirection *= speed;
            //jumping
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        //apply gravity
        moveDirection.y -= gravity * Time.deltaTime;
        //move our character.
        controller.Move(moveDirection * Time.deltaTime);

    }
}
