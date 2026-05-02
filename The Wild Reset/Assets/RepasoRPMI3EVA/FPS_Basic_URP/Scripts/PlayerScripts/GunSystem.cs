using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Necesario para trabajar con el New Input System

public class GunSystem : MonoBehaviour
{
    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam; //Referencia a la cámara desde cuyo centro se dispara (Raycast desde centro cámara)
    [SerializeField] RaycastHit hit; //Referencia a la info de impacto de los disparos (información de impacto Raycast)
    [SerializeField] LayerMask interactableLayer; //Referencia a la Layer que puede impactar el disparo
    [SerializeField] AudioSource weaponSound; //Referencia al AudioSource del arma

    [Header("Interactable Stats")]
    public float range; //Alcance de disparo (longitud del Raycast)
    public float shootingCooldown; //Tiempo de enfriamiento del arma
    public int damage; //Daño del arma

    [Header("State Bools")]
    [SerializeField] bool shooting; //Verdadero cuando ESTAMOS DISPARANDO
    [SerializeField] bool canShoot; //Verdadero cuando PODEMOS DISPARAR

    [Header("Feedback & Graphics")]
    [SerializeField] GameObject muzzleFlash; //Objeto feedback del fogonazo
    [SerializeField] bool attackIsSounding; //Si es verdadero, el sonido de disparo ya suena, por lo que no hay que repetirlo
    #endregion

    private void Awake()
    {
        weaponSound = GetComponent<AudioSource>();
        attackIsSounding = false;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        //Lectura constante del Raycast si se reúnen las condiciones
        if (canShoot && shooting)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        canShoot = false; //Estamos en el proceso de disparo, por tanto YA NO PODEMOS DISPARAR hasta que acabe
        Vector3 direction = fpsCam.transform.forward;

        //Raycast del disparo
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, interactableLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyDamage enemyScript = hit.collider.GetComponent<EnemyDamage>();
                enemyScript.TakeDamage(damage);
            }
        }

        if(!IsInvoking(nameof(ResetShoot)) && !canShoot)
        {
            Invoke(nameof(ResetShoot), shootingCooldown);
        }
    }

    void ResetShoot()
    {
        canShoot = true; //La acción de disparo ha acabado y por tanto (si se reunen las condiciones) podemos volver a disparar
    }

    //Método usado en el New Input System
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            muzzleFlash.SetActive(true);
            shooting = true;
            if (!attackIsSounding)
            {
                weaponSound.Play();
                attackIsSounding = true;
            }
        }

        if (context.canceled)
        {
            shooting = false;
            attackIsSounding = false;
            muzzleFlash.SetActive(false);
            weaponSound.Stop();
        }
    }
}
