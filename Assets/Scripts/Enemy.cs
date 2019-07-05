using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables
    public int damage;

    // Variables para controlar los rangos de de vision y ataque de la Inteligencia Artificial.
    public float lookRadius;
    private float attackDistance = 1.0f;

    Transform target;
    NavMeshAgent agent;

    bool exit = false;
    bool isDetected = false;

    // Ventana en donde el player es inmune a atackes consecutivos del auto enemigo.
    bool flickr = false;
    #endregion

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Sistema de Rayos que actuan como campo de vision para detectar al player y seguirlo para atacar, se generan dos rayos con distintos angulos
    /// para detectar al jugador que pase por la izquierda o derecha del auto enemigo.
    /// </summary>
    void FixedUpdate()
    {
        if (!isDetected)
        {
            agent.isStopped = true;
            RaycastHit hit;
            RaycastHit hit2;

            Vector3 directionRight = Quaternion.AngleAxis(70, transform.up) * transform.forward;
            Vector3 directionLeft = Quaternion.AngleAxis(300, transform.up) * transform.forward;

            if (Physics.Raycast(transform.position + new Vector3(0, 0.4f, 0), directionRight, out hit, 100.0f))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    isDetected = true;
                    exit = true;
                    agent.isStopped = false;
                }
            }
            if (Physics.Raycast(transform.position + new Vector3(0, 0.4f, 0), directionLeft, out hit2, 100.0f))
            {
                if (hit2.collider.gameObject.tag == "Player")
                {
                    isDetected = true;
                    exit = true;
                    agent.isStopped = false;
                }
            }
        }
    }

    /// <summary>
    /// Controla el comportamiento de la inteligencia artificial.
    /// </summary>
    void Update()
    {
        // Persigue.
        float distance = Vector3.Distance(target.position, transform.position);
        if (isDetected)
        {
            agent.SetDestination(target.position);
        }

        // Se detiene al estar muy lejano.
        if (exit && (distance > lookRadius))
        {
            agent.isStopped = true;
        }

        // Ataca.
        if (distance < attackDistance && !flickr)
        {
            GameManagement.life -= damage;
            flickr = true;
            StartCoroutine(WaitForAttack());
        }

    }

    // Funcion que genera una pequeña ventana de tiempo en donde el auto jugador es inmune.
    private IEnumerator WaitForAttack()
    {     
        yield return new WaitForSeconds(2f);
        flickr = false;
    }
}
