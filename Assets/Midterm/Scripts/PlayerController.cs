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
            UI.zoomImage.gameObject.SetActive(true); // �� �̹��� Ȱ��ȭ

            if (tpsCamera.gameObject.activeSelf) // 3��Ī�̸�
            {
                tpsCamera.fieldOfView = 30;
            }
            else if (fpsCamera.gameObject.activeSelf) // 1��Ī�̸�
            {
                fpsCamera.fieldOfView = 30;
            }
        }
        else
        {
            UI.zoomImage.gameObject.SetActive(false); // �� �̹��� ��Ȱ��ȭ

            if (tpsCamera.gameObject.activeSelf) // 3��Ī�̸�
            {
                tpsCamera.fieldOfView = 60;
            }
            else if (fpsCamera.gameObject.activeSelf) // 1��Ī�̸�
            {
                fpsCamera.fieldOfView = 60;
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�ٴڿ� ������ ������ ����
            if (!isJumping)
            {
                //print("���� ���� !");
                isJumping = true;
                rigidBody.AddForce(Vector3.up * 7, ForceMode.Impulse);
            }
        }
    }

    private void TPSCamRotate()
    {
        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * sensitivity;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        //float yRotate = tpsCamera.transform.eulerAngles.y + yRotateSize;
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * sensitivity;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 30);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(0, yRotate, 0);

        tpsCamera.transform.eulerAngles = new Vector3(xRotate, yRotate - 10, 0);
        upperBody.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }

    private void FPSCamRotate()
    {
        
        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * sensitivity;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = fpsCamera.transform.eulerAngles.y + yRotateSize;

        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * sensitivity;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 30);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        fpsCamera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        // �÷��̾� ȸ��
        upperBody.transform.eulerAngles = fpsCamera.transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }

    private void TryMove()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move.magnitude == 0) // ��ǲ�� ����
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
        //�ٴڿ� ������
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //������ ������ ���·� ����
            isJumping = false;
        }
    }
}
