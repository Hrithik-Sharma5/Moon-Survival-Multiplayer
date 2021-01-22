﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField]float baseSpeed;
    [SerializeField] Transform playerCam;
    float speed;
    float turnSmoothVelocity;
    Rigidbody rb;
    bool isAlive, isRunning;

    void Start()
    {
        isAlive = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        float v = Input.GetAxis("Vertical");
        float h= Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(h, 0f, v);
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        if (v != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = baseSpeed * 2;
                playerAnim.SetInteger("Speed", 2);
                isRunning = true;
            }
            else playerAnim.SetInteger("Speed", 1);
        }
        else
        {
            playerAnim.SetInteger("Speed", 0);
            speed = baseSpeed;
        }
        Debug.Log(speed);

        rb.velocity = new Vector3(h * speed * Time.deltaTime, rb.velocity.y, v * speed * Time.deltaTime); 


        //if (Input.GetKeyUp(KeyCode.LeftShift) && isRunning)
        //{
        //    isRunning = false;
        //    speed = speed / 2;
        //}
    }
}
