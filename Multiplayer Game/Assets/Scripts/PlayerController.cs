using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Photon.MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public Animator animator;
    public BoxCollider2D boxCollider2D;

    public PhotonView photonView;
    public SpriteRenderer spriteRenderer;
    public Text playerNameText;

    public bool isGrounded = false;
    public float movementSpeed;
    public float jumpForce;

    public bool DisableInput = false;

    [SerializeField] private LayerMask platformsLayerMask;

    private void Awake()
    {
        if (photonView.isMine)
        {
            playerNameText.text = PhotonNetwork.playerName;
        }
        else
        {
            playerNameText.text = photonView.owner.name;
            playerNameText.color = Color.cyan;
        }
    }

    private void Update()
    {
        if (photonView.isMine && !DisableInput)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * movementSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }

        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }


        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2D.velocity = Vector2.up * jumpForce;
        }

    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }

    [PunRPC]
    private void FlipTrue()
    {
        spriteRenderer.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        spriteRenderer.flipX = false;
    }
}
