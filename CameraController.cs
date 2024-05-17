using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 chestOffset;
    public Transform _transform;
    public Vector3 dialogueOffset;
    public Vector3 bowOffset;
    public CharacterController characterController;
    float z = 0;
    public LayerMask mask;
    public float x;
    public float xLerp;
   public float zScroll = 40;
    float zScrollLerp;
    const float xx = 1.5f;

    private MoveControll moveControll;
    public CinemachineFreeLook cinemachineFreeLook;
    public static CameraController instance;
    public float sensitive = 0.5f;
    private void OnEnable()
    {
        if (!instance)
            instance = this;
        CharacterChanger.OnMenuOpenedEvent += EnableCustomizationHeading;
        CharacterChanger.OnMenuClosedEvent += DisableCustomizationHeading;
        
    }
    private void OnDisable()
    {
        CharacterChanger.OnMenuOpenedEvent -= EnableCustomizationHeading;
        CharacterChanger.OnMenuClosedEvent -= DisableCustomizationHeading;
    }
    void Start()
    {
        _transform = transform;
       // if (GOManager.MainCamera)
       //   DestroyImmediate(GOManager.MainCamera.gameObject);
        GOManager.MainCamera = GetComponent<Camera>();
        zScrollLerp = zScroll;
        
    }
    void EnableCustomizationHeading()
    {
        cinemachineFreeLook.m_Heading.m_Bias = -180;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
        cinemachineFreeLook.m_YAxis.Value = 0.59f;
    }
    void DisableCustomizationHeading()
    {
        cinemachineFreeLook.m_Heading.m_Bias = 0;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = sensitive;
    }
    void Update()
    {
        if (!GOManager.player) return;
        if (!characterController) characterController = GOManager.player.GetComponent<CharacterController>();
        if (!target)
        {
            target = GOManager.playerTransform.GetChild(0);
            moveControll = target.parent.GetComponent<MoveControll>();

            

            cinemachineFreeLook.Follow = target;
            cinemachineFreeLook.LookAt = target;
        }
        if (!moveControll.controllBlock)
        {
            if (cinemachineFreeLook.m_YAxis.m_MaxSpeed != sensitive)
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = sensitive;
        }
        else
        {
            if (cinemachineFreeLook.m_YAxis.m_MaxSpeed != 0)
                cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
        }
      
        if (!moveControll.controllBlock)
        {

            zScroll -= Input.GetAxis("Mouse ScrollWheel") * 20;
            if (zScroll > 51) zScroll = 51;
            else
                if (zScroll < 29) zScroll = 29;
            zScrollLerp = Mathf.Lerp(zScrollLerp, zScroll, 0.3f);
            if (cinemachineFreeLook.m_Lens.FieldOfView != zScrollLerp)
            cinemachineFreeLook.m_Lens.FieldOfView = zScrollLerp;



        }
        /*
      RaycastHit raycastHit;
      if (Physics.Linecast(target.position, target.position + target.TransformDirection(0, Mathf.Sin(xLerp) * 4f, Mathf.Cos(xLerp) * (-4f + zScrollLerp)), out raycastHit, mask))
      {
          z = -target.InverseTransformDirection(target.position - raycastHit.point).z + 0.3f;
      }
      else
      {
          z = -4 + zScrollLerp;
      }
      if (dialogueOffset.magnitude > 0.1f)
              _transform.position = Vector3.Lerp(_transform.position, target.position + target.TransformDirection(dialogueOffset), 1);
          else
              _transform.position = Vector3.Lerp(_transform.position, target.position + target.TransformDirection(new Vector3(0, Mathf.Sin(xLerp) * 4, Mathf.Cos(xLerp) * z) + chestOffset + dialogueOffset), 0.33f);
          //characterController.Move(Vector3.Lerp(_transform.position, target.position + new Vector3(0, 0.5f, 0) + target.TransformDirection(new Vector3(0, 0, z) + chestOffset + dialogueOffset), 0.5f) - _transform.position);
          Quaternion q = Quaternion.LookRotation(target.position + target.forward - _transform.position);
          _transform.rotation = q; //Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, q.eulerAngles.z);
      */

    }
}
