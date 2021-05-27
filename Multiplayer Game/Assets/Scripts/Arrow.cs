using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrow : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public float destroyTime;
    public float damage;

    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;
    bool hasHit;
    bool firstHit = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }


    // Update is called once per frame
    void CheckInput()
    {
        if (hasHit == false)
        {
            float angle = Mathf.Atan2(rigidbody2d.velocity.y, rigidbody2d.velocity.x) *
                Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            CheckInput();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!photonView.isMine)
        {
            return;
        }

        if(firstHit == true)
        {
            firstHit = false;
            Debug.Log("hitplayer");
            return;
        }

        Debug.Log("passedplayer");
        hasHit = true;

        photonView.RPC("arrows", PhotonTargets.AllBuffered);

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.isMine || target.isSceneView))
        {
            if (target.tag == "Player")
            {
                target.RPC("ReduceHealth", PhotonTargets.AllBuffered, damage);
            }
        }
    }

    [PunRPC] private void arrows()
    {
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.isKinematic = true;
        boxCollider2d.enabled = !boxCollider2d.enabled;
    }
}
