using UnityEngine;

namespace input
{
    public class GyroController : MonoBehaviour
    {
        [Header("Rotation Modifiers")]
        [SerializeField] private float slerpValue = 0.2f;
        [SerializeField] private int verticalOffsetAngle = -90;
        [SerializeField] private int horizontalOffsetAngle = 0;
    
    
        //Rotation variables
        private Quaternion horizontalRotationCorrection;
        private Quaternion verticalRotationCorrection;

        private void Start()
        {
           CheckGyroSupport();
        }

        private void Update()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, RotationAdjustment(Input.gyro.attitude, verticalOffsetAngle, horizontalOffsetAngle), slerpValue);
        }

        /// <summary>
        /// Checks if the phone has gyro support and enables it
        /// </summary>
        private void CheckGyroSupport()
        {
            // Check if device has a gyroscope and enable it
            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
                return;
            }
            Debug.LogWarning("Gyro not supported, App will be non-functional");
        }
        
        /// <summary>
        /// Corrects attitude to Unity Coordinates and optionally offsets the rotation. 
        /// </summary>
        /// <param name="attitude">attitude of phone</param>
        ///  <param name="verticalOffset">Vector3-Left offset rotate camera</param>
        ///  <param name="horizontalOffset">Vector3-Up offset to rotation camera</param>
        /// <returns>Quaternion representing the corrected rotation</returns>
        private Quaternion RotationAdjustment(Quaternion attitude, float verticalOffset = 0,  float horizontalOffset = 0)
        {
            // adjust by offset and account for different input systems (WHY DOES UNITY NOT DO THIS FOR ME)
            return Quaternion.AngleAxis(horizontalOffset, Vector3.up) * 
                   Quaternion.AngleAxis(verticalOffset, Vector3.left) * 
                   new Quaternion(attitude.x, attitude.y, -attitude.z, -attitude.w);

        }
    }
}


