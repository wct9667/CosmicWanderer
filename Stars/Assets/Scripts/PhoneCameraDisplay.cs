using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class PhoneCameraDisplay : MonoBehaviour
{
  
  [Header("XR Camera Controls")]
  [SerializeField] private RawImage background;
  [SerializeField] private AspectRatioFitter fit;
  [SerializeField] private float cameraBackgroundAlpha = 61/255f;

  private bool camAvailable;
  private WebCamTexture backCam;
  private Texture defaultBackground;
  private Camera mainCam; 
  
  private void Start()
  {
    mainCam = Camera.main;
    defaultBackground = background.texture;
    
    //Get available cameras
    WebCamDevice[] devices = WebCamTexture.devices;
    
    if (devices.Length == 0)
    {
      Debug.LogWarning("No Cameras detected");
      camAvailable = false;
      return;
    }
    
    //check for multiple cameras, 
    foreach (WebCamDevice device in devices)
    {
      //ignore cameras  facing user (selfie)
      if (device.isFrontFacing) continue;
      
      backCam = new WebCamTexture(device.name, Screen.width, Screen.height);
    }

    if (backCam == null) 
    {
      Debug.LogWarning("Phone has no front facing camera");
      background.texture = defaultBackground;
      return;
    }
    
    backCam.Play();
    background.texture = backCam;

    camAvailable = true;
  }

  /// <summary>
  /// Turn camera on or off
  /// </summary>
  /// <param name="toggle">Bool to control camera</param>
  public void ToggleCameraActive(bool toggle)
  {
    background.enabled = toggle;
    Color camColor = mainCam.backgroundColor;
    
    //make camera clear alpha
    if (toggle)
    {
      mainCam.backgroundColor = new Color(camColor.r, camColor.b, camColor.b,
        cameraBackgroundAlpha);
      return;
    }
    //set camera background to black
    mainCam.backgroundColor = new Color(camColor.r, camColor.b, camColor.b,
      255f);
  }

  private void Update()
  {
    if (!camAvailable) return;
    
    float ratio = backCam.width / (float)backCam.height;
    fit.aspectRatio = ratio;
    
    //set scale of texture
    float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
    background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
    background.rectTransform.localEulerAngles = new Vector3(0, 0, -backCam.videoRotationAngle);
  }
}
