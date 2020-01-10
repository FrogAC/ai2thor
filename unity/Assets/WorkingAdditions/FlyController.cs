using UnityEngine;
using System.Collections;

public class FlyController : MonoBehaviour {

    /**
     * wasdrf : basic movement
     * shift : Makes camera accelerate
     * wheel : fov change
     */

    float mainSpeed = 3f; // Regular speed.
    float shiftAdd = 5f;  // Multiplied by how long shift is held.  Basically running.
    float maxShift = 10f; // Maximum speed when holding shift.
    float camSens = 2f;  // Camera sensitivity by mouse input.
    float fovCapLo = 50.0f;
    float fovCapHi = 100.0f;
    public Camera cam; // used for fov 
    private Vector3 mouseDelta = new Vector3(Screen.width / 2, Screen.height / 2, 0); // Kind of in the middle of the screen, rather than at the top (play).
    private float totalRun = 1.0f;

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {

        mouseDelta = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * camSens;
        transform.eulerAngles += mouseDelta;

        // Keyboard commands.
        Vector3 p = getDirection();
        if (Input.GetKey(KeyCode.LeftShift)) {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        } else {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;

        if (cam) {
            float newfov = cam.fieldOfView - Input.mouseScrollDelta.y * 2f;
            newfov = Mathf.Clamp(newfov, fovCapLo, fovCapHi);
            cam.fieldOfView = newfov;
        }

        transform.Translate(p);
    }

    private Vector3 getDirection() {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W)) {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S)) {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A)) {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D)) {
            p_Velocity += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.R)) {
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.F)) {
            p_Velocity += new Vector3(0, -1, 0);
        }
        return p_Velocity;
    }
}
