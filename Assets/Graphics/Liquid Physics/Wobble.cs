using UnityEngine;

public class Wobble : MonoBehaviour {
    Renderer rend;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;
    Vector3 angularVelocity;

    [Header("Liquid")]
    [Range(0f, 1f)]
    public float Fill = 0.58f;

    [Header("Wobble")]
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;

    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;

    void Start() {
        rend = GetComponent<Renderer>();
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }

    private void Update() {
        time += Time.deltaTime;

        // Decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(
            wobbleAmountToAddX, 0, Time.deltaTime * Recovery
        );

        wobbleAmountToAddZ = Mathf.Lerp(
            wobbleAmountToAddZ, 0, Time.deltaTime * Recovery
        );

        // Make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        // Send values to the shader
        rend.material.SetFloat("_Fill", Fill);
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // Velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;

        // Add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp(
            (velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble,
            -MaxWobble,
            MaxWobble
        );

        wobbleAmountToAddZ += Mathf.Clamp(
            (velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble,
            -MaxWobble,
            MaxWobble
        );

        // Keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }
}