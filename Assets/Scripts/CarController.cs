using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set the input system and the Physics of the Car.
/// </summary>
public class CarController : MonoBehaviour
{
    #region variables
    public Rigidbody sphere;

    private float speed, currentSpeed;
    private float rotate, currentRotate;
    public LayerMask layerMask;

    [Header ("Car Physics Parameters")]
    public float maxSpeed = 30f;
    public float turboMaxSpeed = 100f;
    public float steering = 80f;
    private float gravity = 10f;

    [Space]

    [Header ("Turbo Options")]
    public int turbosAvailables;
    public bool canTurbo;
    private bool turboState = false;
    private bool whileTurbo = false;
    #endregion

    void Update()
    {
        // Input que setea la maxima velocidad.
        if (Input.GetButton("Fire1") && !turboState && !whileTurbo)
        {
            speed = maxSpeed;
        }

        // Input para doblar, obteniendo direccion y una cantidad (peso) para la funcion de rotar.
        if (Input.GetAxis("Horizontal") != 0)
        {
            int direction = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(direction, amount);
        }

        // Input que setea la velocidad de turbo, ademas de entrar en estado turbo y quitar un turbo de la cantidad disponible.
        if (Input.GetKeyDown("space") && !turboState && !whileTurbo){
            if (canTurbo && turbosAvailables > 0)
            {
                speed = turboMaxSpeed;
                turboState = true;
                whileTurbo = true;
                turbosAvailables -= 1;
            }
        }

        // Genera con uso de interpolaciones la curva de rapidez de cambio entre la velocidad actual con la maxima, o la rotacion actual con la maxima.
        // Luego se actualizan las variables para que el motor de fisicas genere las fuerzas usando la velocidad y rotacion actual.
        if (whileTurbo)
        {
            if (turboState)
            {
                StartCoroutine(TurboState());
            }
            turboState = false;
            currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 200f);
        }
        else
        {
            currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
            speed = 0f;
        }

        // Set the rotation with a linear interpolation to make a smooth transition.
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;

    }

    /// <summary>
    /// Motor de Fisicas del auto, es aqui en donde se efectua el movimiento del auto.
    /// </summary>
    private void FixedUpdate()
    {
        // Aplica la Fuerza del "motor" del auto para avanzar.
        sphere.AddForce(-transform.right * currentSpeed, ForceMode.Acceleration);

        // Gravedad
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        // Aplica la fuerza para rotar el auto
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        // Sistema de RayCast para poner restricciones en el movimiento del rigidBody, esto soluciona algunos problemas con las fisicas del auto
        // Este sistema tiene mucho espacio para mejorar, en el estado actual el sistema no es tan robusto como podria esperar.
        RaycastHit hitFront;
        RaycastHit hitDown;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.4f, 0), Vector3.forward, out hitFront, 2.0f, layerMask))
        {
            sphere.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        }

        else if ((Physics.Raycast(transform.position + new Vector3(0, 0.4f, 0), Vector3.down, out hitDown, 0.3f, layerMask)))
        {
            if (hitDown.collider.gameObject.tag == "Ground")
            {
                sphere.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY;
            }
        }
        else if (!(Physics.Raycast(transform.position + new Vector3(0, 0.4f, 0), Vector3.down, out hitDown, 1f, layerMask)))
        {
            sphere.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        }

        else if ((Physics.Raycast(transform.position, Vector3.down, out hitDown, 2f, layerMask)))
        {
            if (hitDown.collider.gameObject.tag == "Ramp")
            {
                sphere.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            }
        }
    }

    /// <summary>
    /// Procesa el input y genera una nueva variable de rotacion a la cual la rotacion actual debe llegar.
    /// </summary>
    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    /// <summary>
    /// Toma el input de velocidad maxima correspondiente, la asigna para generar la aceleracion posteriormente.
    /// </summary>
    private void Speed(float velocity)
    {
        currentSpeed = velocity;
    }

    /// <summary>
    /// Co-Rutina de aceleracion, en estado de turbo, genera un cambio de velocidad entre la actual y la velocidad de turbo en un tiempo
    /// muy pequeño ( gran aceleracion )
    /// </summary>
    private IEnumerator TurboState()
    {
        yield return new WaitForSeconds(2.0f);
        whileTurbo = false;
    }
}


