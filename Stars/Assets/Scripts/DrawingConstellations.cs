using UnityEngine;

namespace Constellations
{
    public class DrawingConstellations : MonoBehaviour
    {
        [Header("Input")] [SerializeField] private InputReader inputReader;

        [Header("Collision Detection")] [SerializeField]
        private float maxDistance = 100f;

        [SerializeField] private LayerMask layer;
        [SerializeField] private ConstellationManager constellationManager;
        [SerializeField] private HapticFeedbackManager haptics;

        private Star previousStar = null;

        private Transform mainCameraTransform;
        private int currentConstellationIndex = -1;

        private bool constellationsDrawingEnabled;

        public bool ConstDraw()
        {
            return constellationsDrawingEnabled;
        }

        private void Start()
        {
            mainCameraTransform = Camera.main.transform;
            currentConstellationIndex = constellationManager.ConstellationNum - 1;
        }

        private void EnableRayCastDrawConstellations()
        {
            constellationsDrawingEnabled = !constellationsDrawingEnabled;
            if (constellationsDrawingEnabled) currentConstellationIndex++;
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

            RayCastFromCamera();
        }

        private void RayCastFromCamera()
        {
            // Create a ray from the camera pointing forward
            Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layer))
            {
                Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
                Star star = hit.collider.gameObject.GetComponent<Star>();

              
                //
                if (!constellationsDrawingEnabled) return;
                
                //add the star to a new constellation with the constellation manager
                star.HighlightStar();
                constellationManager.AddConstellation(currentConstellationIndex, (int)star.catalogNumber);

                if (star != previousStar)
                {
                    haptics.TriggerHaptic(); //TODO Change to an event
                }

                previousStar = star;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
            }
        }
    }
}