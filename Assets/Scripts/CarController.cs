using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : Photon.MonoBehaviour
{
    public PhotonView photonView;
    Rigidbody2D rigidbody;
    public GameObject playerCamera;
    SpriteRenderer spriteRenderer;
    public Text playerNameText;

    public float driftFactor = 0.95f;
    public float accelerationFactor = 150.0f;
    public float turnFactor = 3.5f;

    float accelerationInput = 0f;
    float steeringInput = 0f;
    float rotationAngle = 0f;
    float velocityVsUp = 0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (photonView.isMine)
        {
            playerCamera.SetActive(true);
            playerNameText.text = PhotonNetwork.playerName;
        }
        else
        {
            playerNameText.text = photonView.owner.NickName;
            playerNameText.color = Color.cyan;
        }
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            ApplyEngineForce();

            KillOrthogonalVelocity();

            ApplySteering();
        }
        // ApplyEngineForce();
        // ApplySteering();
    }

    private void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, rigidbody.velocity);

        if (accelerationInput == 0)
            rigidbody.drag = Mathf.Lerp(rigidbody.drag, 3.0f, Time.fixedDeltaTime * 3);
        else rigidbody.drag = 0;

        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        rigidbody.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        float minSpeedBeforeAllowTurningFactor = (rigidbody.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;
        rigidbody.MoveRotation(rotationAngle);
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rigidbody.velocity, transform.right);

        rigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rigidbody.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if ((accelerationInput < 0 && velocityVsUp > 0) || (accelerationInput > 0 && velocityVsUp < 0))
        {
            isBraking = true;
            return true;
        }
        if (Mathf.Abs(GetLateralVelocity()) > 50.0f)
            return true;

        return false;
    }

    public float GetVelocityMagnitude()
    {
        return rigidbody.velocity.magnitude;
    }
}
