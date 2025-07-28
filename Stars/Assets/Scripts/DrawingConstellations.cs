using System;
using UnityEngine;

public class DrawingConstellations : MonoBehaviour
{
    [Header("Input")] 
    [SerializeField] private InputReader inputReader;
    
    [Header("Collision Detection")]
    [SerializeField] private float maxDistance = 100f; 
    [SerializeField] private LayerMask layer;
    [SerializeField] private ConstellationManager _constellationManager;
    [SerializeField] private HapticFeedbackManager haptics;

    private Star previousStar = null;

    private Camera mainCamera;
    private int currentIndex = -1;
    
    private bool constellationsDrawingEnabled;

    public bool ConstDraw()
    {
        return constellationsDrawingEnabled;
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        currentIndex = _constellationManager.ConstellationNum - 1;
    }
    
    private void EnableRayCastDrawConstellations()
    {
        constellationsDrawingEnabled = !constellationsDrawingEnabled;
        if(constellationsDrawingEnabled)  currentIndex++;
    }

    private void OnEnable()
    {
        inputReader.DoubleTap += EnableRayCastDrawConstellations;
    }
    
    private void OnDisable()
    {
        inputReader.DoubleTap -= EnableRayCastDrawConstellations;
    }

    void Update()
    {
        if (!constellationsDrawingEnabled) return;
        
        // Create a ray from the camera pointing forward
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layer))
        {
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            Star star = hit.collider.gameObject.GetComponent<Star>();
            
            //add the star to a new constellation with the constellation manager
            if (constellationsDrawingEnabled)
            {
                star.SetScale();
                _constellationManager.AddConstellation(currentIndex, (int)star.catalogNumber);

                if (star != previousStar)
                {
                    haptics.TriggerHaptic();
                }
                previousStar = star;
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        }
    }
}
