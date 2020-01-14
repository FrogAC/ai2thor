using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public Camera camMain;
    public Camera camDepth;
    public Camera camPoint;
    private Vector3 mouseDelta = new Vector3(Screen.width / 2, Screen.height / 2, 0); // Kind of in the middle of the screen, rather than at the top (play).
    private float totalRun = 1.0f;

    /// logging
    public Text loggerUi;
    /// ScreenCap
    const int RES_WIDTH = 1920;
    const int RES_HEIGHT = 1080;
    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        //cams = GameObject.FindObjectsOfType<Camera>();
        camMain.fieldOfView = camDepth.fieldOfView = camPoint.fieldOfView = .5f * (fovCapHi + fovCapLo);
        Debug.Log("Persis Path" + Application.persistentDataPath);
    }

    void Update() {

        mouseDelta = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * camSens;
        transform.eulerAngles += mouseDelta;

        // movement and camer control
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
        float newfov = camMain.fieldOfView - Input.mouseScrollDelta.y * 2f;
        newfov = Mathf.Clamp(newfov, fovCapLo, fovCapHi);
        camDepth.fieldOfView = camMain.fieldOfView = camPoint.fieldOfView = newfov;
        transform.Translate(p);

        /// ui
        loggerUi.text = string.Format("WASDRF,MouseWheel,C\ncam pos = {0}\nprojection mat =\n{1}\n",
                            camMain.transform.position,
                            camMain.projectionMatrix);

        /// Capture
        if (Input.GetKeyDown(KeyCode.C)) {
            StartCoroutine(TakeScreenShot(camMain, "Main", RES_WIDTH,RES_HEIGHT));
            StartCoroutine(TakeScreenShot(camDepth, "Depth", 0f,0f));
            StartCoroutine(TakeScreenShot(camPoint, "Points", RES_WIDTH,0f));
        }
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
    private IEnumerator TakeScreenShot(Camera cam, string camid, float x, float y) {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(RES_WIDTH * 2, RES_HEIGHT * 2, 24);
        cam.targetTexture = rt;
        Texture2D capture = new Texture2D(RES_WIDTH, RES_HEIGHT, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        capture.ReadPixels(new Rect(x, y, RES_WIDTH, RES_HEIGHT), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        // Encode texture
        byte[] bytes = capture.EncodeToPNG();
        string path = string.Format("{0}/screenshots/Shot{1}_{2}.png",
            Application.persistentDataPath,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
            camid);
        System.IO.File.WriteAllBytes(path, bytes);
    }
}
