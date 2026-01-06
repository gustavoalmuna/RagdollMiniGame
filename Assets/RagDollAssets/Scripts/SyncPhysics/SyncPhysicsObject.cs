using UnityEngine;

public class SyncPhysicsObject : MonoBehaviour
{
    private Rigidbody _rb;
    private ConfigurableJoint joint;

    [SerializeField] private Rigidbody animatedRigidbody;
    [SerializeField] private bool syncAnimation = false;

    private Quaternion startLocalRotation;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();
        startLocalRotation = transform.localRotation;
    }

    public void UpdateJointFromAnimation()
    {
        if (!syncAnimation) { return;  }

        ConfigurableJointExtensions.SetTargetRotationLocal(joint, animatedRigidbody.transform.localRotation, startLocalRotation);
    }
}
