using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField] float baseSpeed;
    [SerializeField] Transform playerCam;
    [SerializeField] GameObject shootParticle;
    [SerializeField] GameObject HitParticle;
    float speed;
    float turnSmoothVelocity;
    Rigidbody rb;
    bool isAlive, isRunning, isJumping, isShooting;

    void Start()
    {
        isAlive = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        PlayerMovement();
        Shoot();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isJumping = false;
                playerAnim.SetTrigger("LandOnGround");
            }
        }
    }

    /// <summary>
    /// To move and rotate player
    /// </summary>
    void PlayerMovement()
    {
        #region Movement and Rotation
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float targetAngle = /*Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +*/ playerCam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.2f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

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


        Vector3 playerMovement = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
        #endregion

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                playerAnim.SetTrigger("Jump");
                rb.AddForce(Vector3.up*5, ForceMode.Impulse);
            }
        }
        #endregion

        //rb.velocity = new Vector3(h * speed * Time.deltaTime, rb.velocity.y, v * speed * Time.deltaTime);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootParticle.SetActive(true);
            isShooting = true;
            StartCoroutine(ShootHit());
        }
        if (Input.GetMouseButtonUp(0))
        {
            shootParticle.SetActive(false);
            isShooting = false;
        }

    }

    IEnumerator ShootHit()
    {
        while (isShooting)
        {
            RaycastHit _hit;
            if(Physics.Raycast(playerCam.position, playerCam.forward, out _hit, 100))
            {
                Instantiate(HitParticle, _hit.point, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
