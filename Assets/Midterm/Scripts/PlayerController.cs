using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera tpsCamera;
    [SerializeField] Camera fpsCamera;
    [SerializeField] GameObject upperBody;
    [SerializeField] Animator animator;

    public UIManager UI { get; set; }

    public AudioSource shotSound, reloadSound;
    Rigidbody rigidBody;

    public bool isPlaying = true;
    public int attackDamage = 50;
    public float sensitivity = 4.0f;

    float xRotate = 0;
    bool isJumping = false;
    bool isZoomed = false;

    private void Start()
    {
        var sounds = GetComponents<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        foreach (var sound in sounds)
        {
            if (sound.clip.name == "Reload")
                reloadSound = sound;
            else if (sound.clip.name == "Shot")
                shotSound = sound;
        }
    }

    void Update()
    {
        if (!isPlaying)
            return;
        TryMove();
        //if(GameManager.Instance.InGameScene().UI.isTps)
            TPSCamRotate();
        //else
            FPSCamRotate();

        if (Input.GetMouseButtonDown(0))
            Shoot();
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.V))
            GameManager.InGameScene.UI.isTps = !GameManager.InGameScene.UI.isTps;
        if (Input.GetMouseButtonDown(1))
            TryZoom();
    }

    private void TryZoom()
    {
        isZoomed = !isZoomed; // toggle

        if (isZoomed)
        {
            UI.zoomImage.gameObject.SetActive(true); // 줌 이미지 활성화

            if (tpsCamera.gameObject.activeSelf) // 3인칭이면
            {
                tpsCamera.fieldOfView = 30;
            }
            else if (fpsCamera.gameObject.activeSelf) // 1인칭이면
            {
                fpsCamera.fieldOfView = 30;
            }
        }
        else
        {
            UI.zoomImage.gameObject.SetActive(false); // 줌 이미지 비활성화

            if (tpsCamera.gameObject.activeSelf) // 3인칭이면
            {
                tpsCamera.fieldOfView = 60;
            }
            else if (fpsCamera.gameObject.activeSelf) // 1인칭이면
            {
                fpsCamera.fieldOfView = 60;
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //바닥에 있으면 점프를 실행
            if (!isJumping)
            {
                //print("점프 가능 !");
                isJumping = true;
                rigidBody.AddForce(Vector3.up * 7, ForceMode.Impulse);
            }
        }
    }

    private void TPSCamRotate()
    {
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * sensitivity;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        //float yRotate = tpsCamera.transform.eulerAngles.y + yRotateSize;
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * sensitivity;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 30);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(0, yRotate, 0);

        tpsCamera.transform.eulerAngles = new Vector3(xRotate, yRotate - 10, 0);
        upperBody.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }

    private void FPSCamRotate()
    {
        
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * sensitivity;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = fpsCamera.transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * sensitivity;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 30);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        fpsCamera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        // 플레이어 회전
        upperBody.transform.eulerAngles = fpsCamera.transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }

    private void TryMove()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move.magnitude == 0) // 인풋이 없음
        {
            animator.SetBool("Running", false);
            return;
        }
        animator.SetBool("Running", true);

        var newMove = new Vector3(transform.TransformDirection(move).x, 0, transform.TransformDirection(move).z);
        float speed = 5f;
        if (Input.GetKey(KeyCode.LeftShift))
            speed = 10f;
        transform.position += newMove * Time.deltaTime * speed;
        //controller.Move(newMove * Time.deltaTime * speed);
    }

    private void Shoot()
    {
        if(GameManager.InGameScene.UI.playerAmmo <= 0)
        {

            return;
        }
        animator.SetTrigger("Shoot");
        shotSound.PlayOneShot(shotSound.clip);
        GameManager.InGameScene.UI.playerAmmo -= 1;

        RaycastHit[] result = default;

        if (GameManager.InGameScene.UI.isTps)
            result = Physics.RaycastAll(tpsCamera.transform.position, tpsCamera.transform.forward, 10000);
        else
            result = Physics.RaycastAll(fpsCamera.transform.position, fpsCamera.transform.forward, 10000);

        foreach (var hit in result)
        {
            if(hit.collider.tag == "Zombie")
            {
                var zombie = hit.transform.GetComponent<Zombie>();
                zombie.OnDamage(attackDamage);
                return;
            }
        }
    }

    private void Reload()
    {
        reloadSound.Play();
        GameManager.InGameScene.UI.playerAmmo = GameManager.InGameScene.UI.playerEntireAmmo;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //바닥에 닿으면
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //점프가 가능한 상태로 만듦
            isJumping = false;
        }
    }
}
