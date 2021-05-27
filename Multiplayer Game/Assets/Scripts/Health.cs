using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    public float healthAmount;
    public Image FillImage;
    public Rigidbody2D rigidbody2d;
    public BoxCollider2D boxCollider2d;
    public SpriteRenderer spriteRenderer;

    public PlayerController playerController;
    public GameObject PlayerCanvas;

    private void Awake()
    {
        if(photonView.isMine)
        {
            GameManager.Instance.LocalPlayer = this.gameObject;
        }
    }

    [PunRPC] public void ReduceHealth(float amount)
    {
        ModifyHealth(amount);
    }

    private void CheckHealth()
    {
        FillImage.fillAmount = healthAmount / 100f;
        if (photonView.isMine && healthAmount <= 0)
        {
            GameManager.Instance.EnableRespawn();
            playerController.DisableInput = true;
            this.GetComponent<PhotonView>().RPC("Dead", PhotonTargets.AllBuffered);
        }
    }

    public void EnableInput()
    {
        playerController.DisableInput = false;

    }

    [PunRPC]
    private void Dead()
    {
        rigidbody2d.gravityScale = 0;
        boxCollider2d.enabled = false;
        spriteRenderer.enabled = false;
        PlayerCanvas.SetActive(false);
    }

    [PunRPC]
    private void Respawn()
    {
        rigidbody2d.gravityScale = 3;
        boxCollider2d.enabled = true;
        spriteRenderer.enabled = true;
        PlayerCanvas.SetActive(true);
        FillImage.fillAmount = 1f;
        healthAmount = 100f;

    }


    private void ModifyHealth(float amount)
    {
        if(photonView.isMine)
        {
            healthAmount -= amount;
            FillImage.fillAmount -= amount;
        }
        else
        {
            healthAmount -= amount;
            FillImage.fillAmount -= amount;
        }

        CheckHealth();
    }


}
