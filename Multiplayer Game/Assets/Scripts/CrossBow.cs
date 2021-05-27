using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : Photon.MonoBehaviour
{
    public PhotonView photonView;

    public GameObject arrow;
    public float launchForce;
    public Transform shotPoint;

    void Start()
    {
        
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            CheckInput();
        }
    }
    // Update is called once per frame
    void CheckInput()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;

        if( Input.GetMouseButtonDown(0))
        {
            GameObject newArrow = PhotonNetwork.Instantiate(arrow.name, shotPoint.position, shotPoint.rotation, 0);
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        }
    }

}
