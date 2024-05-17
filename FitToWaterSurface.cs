using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FitToWaterSurface : MonoBehaviour
{
    public WaterSurface waterSurface;
    public Rigidbody rigidBody;
    public float depthBefSub;
    public float displacementAmt;
    public int floaters;
    public float waterDrag;
    public float waterAngularDrag;
    private WaterSearchParameters search;
    WaterSearchResult searchResult;
    private new Transform transform;
    
    private float updateTimer = 0;
    public static float updateTime = 0.1f;
    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        float fixedDeltaTime = Time.fixedDeltaTime;
        rigidBody.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);
        search.startPositionWS = transform.position;
        waterSurface.ProjectPointOnWaterSurface(search, out searchResult);
        if (transform.position.y < searchResult.projectedPositionWS.y)
        {
            float displacementMulti = Mathf.Clamp01((searchResult.projectedPositionWS.y - transform.position.y) / depthBefSub) * displacementAmt;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulti, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMulti * -rigidBody.linearVelocity * waterDrag * fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMulti * -rigidBody.angularVelocity * waterAngularDrag * fixedDeltaTime, ForceMode.VelocityChange);
        }

    }
}
