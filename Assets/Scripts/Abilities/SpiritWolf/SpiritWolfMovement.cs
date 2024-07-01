using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpiritWolfMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _raycastDistance = 1f;

    [SerializeField] private Transform _raycastPoint;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Perform raycast to determine ground height
        RaycastHit hit;
        if (Physics.Raycast(_raycastPoint.position, Vector3.down, out hit, _raycastDistance))
            transform.position = new Vector3(transform.position.x, hit.point.y - _raycastPoint.localPosition.y, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _rb.MovePosition(transform.position + transform.forward * _speed * Time.fixedDeltaTime);
        Debug.DrawRay(_raycastPoint.position, Vector3.down * _raycastDistance, Color.red);
    }

}
