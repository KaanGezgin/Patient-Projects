using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spaceship : MonoBehaviour
{

    [Header("Ship Attributes")]
    [SerializeField] Rigidbody shipBody;
    [SerializeField] Transform landingGear;
    [SerializeField] Transform infoGatherer;

    [Header("UI")]
    [SerializeField] Text landingWarning, landingDistanceControl;
    [SerializeField] Text takeoffWarning;
    [SerializeField] Text forwardWarning;
    [SerializeField] Text modeText;
    [SerializeField] Text heightText;
    [SerializeField] Text speedText;


    [Header("Ship Fuel")]
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelConsumption = 1f;
    [SerializeField] float boostFuelConsumption = 5f;
    [SerializeField] float currentFuel;

    [Header("Ship Flying")]
    [SerializeField] Vector3 previousUpDir;
    [SerializeField] float engineStartDelay = 0.2f;
    [SerializeField] bool isDropping;

    [Header("Ship Control")]
    [SerializeField] float accelaterion = 50f;
    [SerializeField] float maxSpeed = 500f;
    [SerializeField] float maxBoost = 300f;
    [SerializeField] float brakeSpeed = 30f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float shipCurrentSpeed;

    [Header("Landing")]
    [SerializeField] bool landingControl;
    [SerializeField] bool takeoffControl;
    [SerializeField] bool forwardControl;
    LayerMask landingLayer;
    RaycastHit landLayerControl;

    [Header("Sensitivity & damping control")]
    [SerializeField] float mouseSensivity = 10.0f;

    private bool isDead;
    private bool canBoost;
    private float tempMaxSpeed;
    float mouseXaxis;
    float mouseYaxis;

    private void Start()
    {
        shipCurrentSpeed = 0f;
        tempMaxSpeed = maxSpeed;
        canBoost = true;
        isDead = false;
        currentFuel = maxFuel;
        shipBody = GetComponent<Rigidbody>();
        landingGear = GameObject.Find("Landing Gear").GetComponent<Transform>();
        shipBody.useGravity = false;
        landingControl = false;
        forwardControl = true;
        takeoffControl = false;
        landingLayer = LayerMask.GetMask("Plane");
        landingDistanceControl = GameObject.Find("Landing Height").GetComponent<Text>();
        landingWarning = GameObject.Find("LandingWarning").GetComponent<Text>();
        takeoffWarning = GameObject.Find("TakeoffWarning").GetComponent<Text>();
        forwardWarning = GameObject.Find("ForwardWarning").GetComponent<Text>();

        speedText = GameObject.Find("Speed").GetComponent<Text>();
        modeText = GameObject.Find("Mode").GetComponent<Text>();
        heightText = GameObject.Find("Height").GetComponent<Text>();
        //Cursor lock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        forwardWarning.text = "Forward warning = none";
        takeoffWarning.text = "Takeoff warning = none";
        landingWarning.text = "Landing warning = none";

    }
    private void Update()
    {
        if (isDead)
        {
            return;
        }

        speedText.text = shipCurrentSpeed.ToString();
        if (Physics.Raycast(landingGear.position, landingGear.TransformDirection(Vector3.down), out landLayerControl))
        {
            heightText.text = landLayerControl.distance.ToString();
            if (landLayerControl.distance <= 20.0f) 
            {
                landingDistanceControl.text = "Landing zone found";
            }
            else
            {
                landingDistanceControl.text = "Landing zone is not avaliable get close to the landing zone";
            }

        }

        if (engineStartDelay >= 0f)
        {
            engineStartDelay -= Time.deltaTime;
        }
        if (engineStartDelay <= 0)
        {
            landingAndTakeoffControl();

            if (forwardControl == true && takeoffControl == false && landingControl == false)
            {
                modeText.text = "Forward mode";
                PlayerRotate();
                FlyingBehavior();
            }
            if (landingControl == true && takeoffControl == false && forwardControl == false)
            {
                modeText.text = "Landing mode";
                LandingMovement();
            }
            if(takeoffControl == true && landingControl == false && forwardControl == false)
            {
                modeText.text = "Takeoff mode";
                TakeoffMovement();
            }
        }
    }

    void PlanetInfo()
    {
        PlanetAttributes planetAtt;
        RaycastHit planetInfo;
        
        if(Physics.Raycast(infoGatherer.position, infoGatherer.TransformDirection(Vector3.forward), out planetInfo))
        {
            if(planetInfo.collider.tag == "Planet Info")
            {
                Debug.Log(planetInfo.transform.gameObject.name);
                planetAtt = GameObject.Find(planetInfo.transform.gameObject.name).GetComponent<PlanetAttributes>();
                Debug.Log(planetAtt.planetName);
                Debug.Log(planetAtt.wood);
            }
        }
    }

    void PlayerRotate()
    {
        float mouseZaxis = Input.GetAxis("Vertical");

        mouseXaxis += Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        mouseYaxis -= Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(mouseYaxis, mouseXaxis, mouseZaxis);
    }
    void Landing()
    {
        if (landingControl == false && (takeoffControl == true || forwardControl == true))
        {
            if (landLayerControl.distance <= 20.0f)
            {
                landingWarning.text = "Landing Warning: Initiating Landing";
                forwardWarning.text = "Forward Warning: Forward mode off";
                takeoffWarning.text = "Takeoff Warning: Takeoff mode off";
                modeText.text = "Landing mode";
                shipCurrentSpeed = 0;
                landingControl = true;
                takeoffControl = false;
                forwardControl = false;
            }
            else
            {
                landingWarning.text = "landing Warning: Landing zone is not avaliable get close to the landing zone";
                Debug.Log("Landing zone is not avaliable");
            }
        }
    }
    void Takeoff()
    {
        if (landingControl == true && takeoffControl == false && forwardControl == false)
        {
            modeText.text = "Takeoff mode";
            landingWarning.text = "Landing Warning: Landing mode off";
            forwardWarning.text = "Forward Warning: Forward mode off";
            takeoffWarning.text = "Takeoff Warning: Initiating take off";
            takeoffControl = true;
            landingControl = false;
            forwardControl = false;
        }
    }
    
    void Forward()
    {
        if (forwardControl == false && (landingControl == true || takeoffControl == true))
        {
            modeText.text = "Forward mode";
            forwardWarning.text = "Forward Warning: Forward mode on";
            takeoffControl = false;
            landingControl = false;
            forwardControl = true;
        }
    }

    void landingAndTakeoffControl()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Landing();
        }
        if (Input.GetKey(KeyCode.T))
        {
            Takeoff();
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (landLayerControl.distance > 10.0f)
            {
                Forward();
            }
            else
            {
                forwardWarning.text = "Forward Warning: Rise to safe distance";
            }
        }
    }
    void LandingMovement()
    {
        Vector3 movement;
        float landingSpeed = 30.0f;
       
        
        if (Input.GetKey(KeyCode.S))
        {
            movement = new Vector3(0, Input.GetAxis("Vertical"), 0);
            shipBody.MovePosition(transform.position + movement * Time.deltaTime * landingSpeed);
        }
    }
    void TakeoffMovement()
    {
        Vector3 movement;

        float takeoffSpeed = 20.0f;

        if (Input.GetKey(KeyCode.W))
        {
            movement = new Vector3(0, Input.GetAxis("Vertical"), 0);
            shipBody.MovePosition(transform.position + movement * Time.deltaTime * takeoffSpeed);
        }
    }

    private void FlyingBehavior()
    {
        if (Input.GetKey(KeyCode.E))
        {
            PlanetInfo();
        }

        if (isDead)
        {
            shipCurrentSpeed = 0;
            shipBody.velocity = Vector3.zero;
            transform.gameObject.isStatic = true;
            return;
        }

        if (!isDropping)
        {
            canBoost = true;
            if (GetInput())
            {
                if (!IsBoosting())
                {
                    maxSpeed = tempMaxSpeed;
                    Accelerate(accelaterion);
                }
                else
                {
                    maxSpeed = maxBoost;
                    Accelerate(maxSpeed * 2);
                }
                if (shipCurrentSpeed >= maxSpeed)
                {
                    Brake(brakeSpeed);
                }
                else
                {
                    Brake(brakeSpeed);
                }
                if (shipCurrentSpeed <= 0f)
                {
                    shipBody.velocity = Vector3.zero;
                }
                previousUpDir = transform.up;
            }
            else
            {
                canBoost = false;
            }
            Brake(brakeSpeed / 1.5f);
        }
        MoveShip(shipCurrentSpeed * 80);
    }

   
    private bool GetInput()
    {
        return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
    }
    private void MoveShip(float speed)
    {
        shipBody.velocity = transform.forward * speed * Time.fixedDeltaTime;
    }
    private void Brake(float brakeSpeed)
    {
        if (shipCurrentSpeed > 0)
        {
            shipCurrentSpeed -= brakeSpeed * Time.deltaTime;
        }
        else
        {
            shipCurrentSpeed = 0f;
        }
    }
    private void Accelerate(float speed)
    {
        if (currentFuel <= 0f)
        {
            shipCurrentSpeed = Mathf.Lerp(shipCurrentSpeed, 0f, 1f * Time.deltaTime);
            return;
        }
        shipCurrentSpeed += (shipCurrentSpeed >= tempMaxSpeed) ? 0f : speed * Time.deltaTime;

        currentFuel -= IsBoosting() ? fuelConsumption * 2 * Time.deltaTime : fuelConsumption * Time.deltaTime;
    }
    private bool IsBoosting()
    {
        return ((Input.GetKey(KeyCode.Space) && canBoost));

    }
    private void Die()
    {
        isDead = true;
    }
}








