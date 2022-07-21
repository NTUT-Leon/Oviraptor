using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class throwhook : MonoBehaviour
{
    public GameObject hook;

    GameObject curHook;

    bool ropeActive = false;
    bool mouseHolding = false;

    public float endPoint_x = 10;
    public float endPoint_y = 32;

    public float rolledUpSpeed = 0.1f;

    public float rotateSpeed = 0.2f;
    public float rotateSpeedMin = 500f;

    public Animator animator;

    private Vector2 initPosition;

    // for RotateBuffer()
    public float targetAngle;
    public float rotateBufferSpeedDvided;
    float t;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (mouseHolding)
        {
            Vector2 destiny = new Vector2(transform.position.x + endPoint_x, endPoint_y);

            if (ropeActive == false)
            {
                // ���Hook(�ѦҪ���, �ثe��m(player��), ����?)
                curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);

                // �p��_�I����I����V
                Vector2 direction = destiny - (Vector2)transform.position;
                direction = direction.normalized;
                // �q�_�I�Hdirection����V �M��O�_������(��LayerMask��filter)
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Obstacle"));

                // �p�G�o�g��V������N�g�b����W�A�S�����ܴN�g�b���I�W
                if (hit)
                {
                    curHook.GetComponent<RopeScript>().destiny = hit.point;
                }
                else
                {
                    // �NHook���I�]�����I
                    curHook.GetComponent<RopeScript>().destiny = destiny;
                }

                ropeActive = true;
            }
            transform.position = Vector2.MoveTowards(transform.position, destiny, rolledUpSpeed * Time.deltaTime);
            RotateBuffer();
        }
        else
        {
            if (ropeActive)
            {
                // delete rope
                Destroy(curHook);
                ropeActive = false;
            }

            // �a�ŮɥH�T�w�t�ױ���
            GetComponent<Rigidbody2D>().transform.Rotate(0, 0, rotateSpeedMin +  GetComponent<Rigidbody2D>().velocity.x * rotateSpeed * Time.deltaTime);
            t = 0; // for RotateBuffer()
        }
    }

    // Update is called once per frame
    void Update()
    {
        mouseHolding = Input.GetMouseButton(0);

        if(mouseHolding)
        {
            animator.SetBool("IsSwing", true);
        }
        else
        {
            animator.SetBool("IsSwing", false);
        }

        if(Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void RotateBuffer() // �H�T�w��V�C�C���ؼШ���
    {
        t += Time.deltaTime / rotateBufferSpeedDvided;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, targetAngle, t));
    }
}
