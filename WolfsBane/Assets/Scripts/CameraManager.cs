using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int edgeScrollSize = 20;
    [SerializeField] private float cameraXmin, cameraXmax, cameraYmin, cameraYmax;
    private bool useEdgeScroll;
    private void Awake()
    {
        useEdgeScroll = false;
    }
    private void Update()
    {        
        Vector3 moveDir = new Vector3(0, 0, 0);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            useEdgeScroll = !useEdgeScroll;
            transform.position = PlayerControler.Instance.playerPosition;
        }
        if(useEdgeScroll)
        {
            if(transform.position.x > cameraXmin && Input.mousePosition.x < edgeScrollSize)
            {
                moveDir.x = -1f;
            }
            if(transform.position.y > cameraYmin && Input.mousePosition.y < edgeScrollSize)
            {
                moveDir.y = -1f;
            }
            if(transform.position.x < cameraXmax && Input.mousePosition.x > Screen.width - edgeScrollSize)
            {
                moveDir.x = +1f;
            }
            if(transform.position.y < cameraYmax && Input.mousePosition.y > Screen.height - edgeScrollSize)
            {
                moveDir.y = +1f;
            }
        }       
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
