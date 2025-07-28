using System.Collections;
using UnityEngine;
public class CameraZoom : MonoBehaviour
{
    [Header("Input")] 
    [SerializeField] private InputReader inputReader;

    [Header("Zoom Controls")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 20f;
    [SerializeField] private float maxZoom = 100f;

    private Coroutine zoomCoroutine;
    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputReader.ZoomStart += StartZoom;
        inputReader.ZoomEnd += StopZoom;
    }
    private void OnDisable()
    {
        inputReader.ZoomStart -= StartZoom;
        inputReader.ZoomEnd -= StopZoom;
    }


    /// <summary>
    /// Stop Zoom Detection
    /// </summary>
    private void StopZoom()
    {
        StopCoroutine(zoomCoroutine);
    }
    
    /// <summary>
    /// Start Zoom Detection
    /// </summary>
    private void StartZoom()
    {
        Debug.Log("Zooming");
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    
    /// <summary>
    /// Zooms the camera if a zoom is detected
    /// </summary>
    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f;
        float distance = 0f;

        //keep checking until zoom ends.
        while (true)
        {
            distance = Vector2.Distance(inputReader.PrimaryFingerPosition, inputReader.SecondaryFingerPosition);

            float distanceDelta = distance - previousDistance;
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - distanceDelta * zoomSpeed, minZoom, maxZoom);

            previousDistance = distance;
            yield return null;
        }
        // mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - increment, minZoom, maxZoom);
    }
}
