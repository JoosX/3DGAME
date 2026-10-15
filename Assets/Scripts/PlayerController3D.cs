using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -20f;

    [Header("Cámara en Primera Persona")]
    [SerializeField] Transform cameraPivot;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float topClamp = -85f;
    [SerializeField] float bottomClamp = 85f;

    CharacterController controller;
    Vector3 velocity;
    bool enSuelo;
    float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        // Oculta y bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Control del Mouse (Mirar en Primera Persona)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotación vertical (Pitch) en la cámara
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // Rotación horizontal (Yaw) en el cuerpo del jugador
        transform.Rotate(Vector3.up * mouseX);

        // 2. Comprobación de suelo y gravedad
        enSuelo = controller.isGrounded;
        if (enSuelo && velocity.y < 0f)
        {
            velocity.y = -2f; // Pega al suelo
        }

        // 3. Movimiento relativo a hacia dónde mira el personaje
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 4. Salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}