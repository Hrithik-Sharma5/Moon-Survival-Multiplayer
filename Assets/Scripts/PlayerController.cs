using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField] float baseSpeed, horizontalSpeed;
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
        Cursor.visible = false;
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

        if (v > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = baseSpeed * 2;
                playerAnim.SetFloat("Horizontal", 2);
                isRunning = true;
            }
            else playerAnim.SetFloat("Horizontal", 1);

            playerAnim.SetInteger("Speed", 1);
        }
        else if (v < 0)
        {
            playerAnim.SetFloat("Horizontal", -1);
            playerAnim.SetInteger("Speed", -1);
        }
        else
        {
            playerAnim.SetFloat("Horizontal", 0);
            playerAnim.SetInteger("Speed", 0);
            speed = baseSpeed;
        }

        if (h > 0)
        {
            playerAnim.SetFloat("Vertical", 1);
            playerAnim.SetInteger("Speed", 1);
        }
        else if (h < 0)
        {
            playerAnim.SetFloat("Vertical", -1);
            playerAnim.SetInteger("Speed", -1);
        }
        else
        {
            playerAnim.SetFloat("Vertical", 0);
            if(v==0)
            playerAnim.SetInteger("Speed", 0);
        }

        Vector3 playerMovement = new Vector3(h * horizontalSpeed, 0, v * speed)  * Time.deltaTime;
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
            playerAnim.SetBool("Firing", true);
            StartCoroutine(ShootHit());
        }
        if (Input.GetMouseButtonUp(0))
        {
            shootParticle.SetActive(false);
            isShooting = false;
            playerAnim.SetBool("Firing", false);
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
                Idamagable damage = _hit.transform.GetComponent<Idamagable>();
                if (damage != null)
                {
                    damage.TakeDamage();
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
