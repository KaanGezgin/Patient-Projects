using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingTest : MonoBehaviour
{
    [Header("Ship Attributes")]
    [SerializeField] Rigidbody shipBody;
    [SerializeField] Transform landingGearTransform;
    [SerializeField] Transform infoGatherer;

    [Header("UI")]
    [SerializeField] Text landingWarning, landingDistanceControl;
    [SerializeField] Text takeoffWarning;
    [SerializeField] Text forwardWarning;
    [SerializeField] Text modeText;
    [SerializeField] Text heightText;
    [SerializeField] Text speedText;

    [Header("Ship Control")]
    [SerializeField] float throttleIncrement = 0.1f;
    [SerializeField] float maxThrust = 500f;
    [SerializeField] float responsiveness = 1f;
    [SerializeField] float shipCurrentSpeed;
    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    [Header("Landing")]
    [SerializeField] bool landingControl;
    [SerializeField] bool takeoffControl;
    [SerializeField] bool forwardControl;
    [SerializeField] bool idleControl;
    LayerMask landingLayer;
    RaycastHit landLayerControl;

    [Header("Ship Fuel")]
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelConsumption = 1f;
    [SerializeField] float boostFuelConsumption = 5f;
    [SerializeField] float currentFuel;

    private float responseModifier
    {
        get
        {
            return (shipBody.mass / 10f) * responsiveness;
        }
    }
    private void Awake()
    {
        shipBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        shipCurrentSpeed = 0f;
        currentFuel = maxFuel;
        shipBody = GetComponent<Rigidbody>();
        landingGearTransform = GameObject.Find("Landing Gear").GetComponent<Transform>();
        shipBody.useGravity = false;
        landingControl = false;
        forwardControl = true;
        takeoffControl = false;
        idleControl = false;
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
        speedText.text = shipCurrentSpeed.ToString();
        if (Physics.Raycast(landingGearTransform.position, landingGearTransform.TransformDirection(Vector3.down), out landLayerControl))
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
        landingAndTakeoffControl();
    }

    private void FixedUpdate()
    {
        if (forwardControl == true && takeoffControl == false && landingControl == false && idleControl == false)
        {
            modeText.text = "Forward mode";
            ShipMovement();
        }
        if (landingControl == true && takeoffControl == false && forwardControl == false && idleControl == false)
        {
            modeText.text = "Landing mode";
            LandingMovement();
        }
        if (takeoffControl == true && landingControl == false && forwardControl == false && idleControl == false)
        {
            modeText.text = "Takeoff mode";
            TakeoffMovement();
        }
        if(idleControl == true && takeoffControl == false && forwardControl == false && landingControl == false)
        {
            modeText.text = "Idle mode";
        }
    }

    void ShipMovement()
    {
        HandleInput();

        shipBody.AddForce(this.transform.forward * maxThrust * throttle);
        shipBody.AddTorque(this.transform.up * yaw * responseModifier);
        shipBody.AddTorque(this.transform.right * -pitch * responseModifier);
        shipBody.AddTorque(-this.transform.forward * -roll * responseModifier);

        if (Input.GetKey(KeyCode.R))
        {
            PlanetInfo();
        }
    }
    private void HandleInput()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Mouse Pitch");
        yaw = Input.GetAxis("Mouse Yaw");

        if (Input.GetKey(KeyCode.Space))//Speed up
        {
            throttle += throttleIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftControl))//Break
        {
            throttle -= throttleIncrement;
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);
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
                throttle = 0;
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
        if (idleControl == true && landingControl == false && takeoffControl == false && forwardControl == false)
        {
            modeText.text = "Takeoff mode";
            landingWarning.text = "Landing Warning: Landing mode off";
            forwardWarning.text = "Forward Warning: Forward mode off";
            takeoffWarning.text = "Takeoff Warning: Initiating take off";
            takeoffControl = true;
            landingControl = false;
            forwardControl = false;
            idleControl = false;

        }
    }
    void Idle()
    {
        if (landLayerControl.distance <= 0.05f)
        {
            modeText.text = "Idle mode";
            landingControl = false;
            idleControl = true;
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
            idleControl = false;
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
        if (landLayerControl.distance < 0.05f && landingControl == true)
        {
            Idle();
        }
    }
    void LandingMovement()
    {
        Vector3 movement;
        float landingSpeed = 10.0f;

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
    void PlanetInfo()
    {
        PlanetAttributes planetAtt;
        RaycastHit planetInfo;

        if (Physics.Raycast(infoGatherer.position, infoGatherer.TransformDirection(Vector3.forward), out planetInfo))
        {
            if (planetInfo.collider.tag == "Planet Info")
            {
                Debug.Log(planetInfo.transform.gameObject.name);
                planetAtt = GameObject.Find(planetInfo.transform.gameObject.name).GetComponent<PlanetAttributes>();
                Debug.Log(planetAtt.planetName);
                Debug.Log(planetAtt.wood);
            }
        }
    }
}
