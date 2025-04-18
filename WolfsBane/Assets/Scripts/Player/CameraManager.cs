using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cvCamera;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int edgeScrollSize = 20;
    [SerializeField] private float cameraXmin, cameraXmax, cameraYmin, cameraYmax, minScroll, maxScroll;
    private bool useEdgeScroll;
    [SerializeField] private float basicZoom = 1.5f;
    private void Awake()
    {
        useEdgeScroll = false;
    }
    private void Update()
    {        
        Vector3 moveDir = new Vector3(0, 0, 0);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            LockCamera();
        }
        if(useEdgeScroll)
        {
            if(transform.position.x > cameraXmin && ((Input.mousePosition.x < edgeScrollSize) || Input.GetKey(KeyCode.A)))
            {
                moveDir.x = -1f;
            }
            if(transform.position.y > cameraYmin && ((Input.mousePosition.y < edgeScrollSize) || Input.GetKey(KeyCode.S)))
            {
                moveDir.y = -1f;
            }
            if(transform.position.x < cameraXmax && ((Input.mousePosition.x > Screen.width - edgeScrollSize) || Input.GetKey(KeyCode.D)))
            {
                moveDir.x = +1f;
            }
            if(transform.position.y < cameraYmax && ((Input.mousePosition.y > Screen.height - edgeScrollSize) || Input.GetKey(KeyCode.W)))
            {
                moveDir.y = +1f;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && cvCamera.m_Lens.OrthographicSize > minScroll) // forward
            {
                cvCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel");
                if (cvCamera.m_Lens.OrthographicSize < minScroll) cvCamera.m_Lens.OrthographicSize = minScroll;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && cvCamera.m_Lens.OrthographicSize < maxScroll) // backwards
            {
                cvCamera.m_Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel");
                if (cvCamera.m_Lens.OrthographicSize > maxScroll) cvCamera.m_Lens.OrthographicSize = maxScroll;
            }
        }       
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    public void LockCamera()
    {
        useEdgeScroll = !useEdgeScroll;
        transform.position = PlayerControler.Instance.playerPosition;
        cvCamera.m_Lens.OrthographicSize = basicZoom;
    }
}
