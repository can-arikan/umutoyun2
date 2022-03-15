using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjController : MonoBehaviour
{
    public enum ObjColor { blue,pink,purple,yellow,red,white}
    public ObjColor objColor;

    public enum ObjType {Null, turn, goRight, goLeft, goUp, goDown }
    public ObjType objType;

    public Collider collusionCollider,triggerCollider;

    [HideInInspector]public bool isTouced;
    [HideInInspector] public Color currentColor;

    private ParticleSystem changeParticle;
    private GameObject filledObj,pointLight;
    private bool turn, right, left, up, down;
    private Rigidbody rb;

    [SerializeField] private float turnSpeed = -04f;


    private void Start()
    {
        GetVariablesOnStart();
        GetTypeOnStart();
        SetColors();
    }

    private void FixedUpdate()
    {
        if (turn)
        {
            transform.Rotate(turnSpeed, 0, 0);
        }
    }

    private void GetVariablesOnStart()
    {
        changeParticle = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        filledObj = transform.GetChild(1).gameObject;
        rb = GetComponent<Rigidbody>();
    }

    private void GetTypeOnStart()
    {
        if (objType == ObjType.turn) turn = true;
        if (objType == ObjType.goDown) down = true;
        if (objType == ObjType.goUp) up = true;
        if (objType == ObjType.goLeft) left = true;
        if (objType == ObjType.goRight) right = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) { 
            other.collider.isTrigger = true;
        }

        if (other.gameObject.CompareTag("Player")&&!isTouced){
            filledObj.SetActive(true);
            changeParticle.Play();
            isTouced = true;
            JumpObjectsHandler.instance.lightedJumpObjectCount++;
            JumpObjectsHandler.instance.GetOutFromList(this.gameObject);
            JumpObjectsHandler.instance.FinishGameChecker();
            MovementByBools();
        }

        if (other.gameObject.CompareTag("JumpObjects") && !isTouced)
        {
            filledObj.SetActive(true);
            changeParticle.Play();
            isTouced = true;
            JumpObjectsHandler.instance.lightedJumpObjectCount++;
            JumpObjectsHandler.instance.GetOutFromList(this.gameObject);
            JumpObjectsHandler.instance.FinishGameChecker();
            MovementByBools();
        }
    }

    private void MovementByBools()
    {
        if (objType == ObjType.goDown)
        {
            StartCoroutine(GoDown());
        }
        if (objType == ObjType.goUp)
        {
            StartCoroutine(GoUp());
        }
        if (objType == ObjType.goLeft)
        {
            StartCoroutine(GoLeft());
        }
        if (objType == ObjType.goRight)
        {
            StartCoroutine(GoRight());
        }
    }

    private IEnumerator GoDown()
    {
        while (rb.velocity.y>-5f)
        {
            rb.velocity += Vector3.down * 4;
            yield return null;
        }
    }

    private IEnumerator GoUp()
    {
        while (rb.velocity.y < 5f)
        {
            rb.velocity += Vector3.up * 4;
            yield return null;
        }
    }

    private IEnumerator GoRight()
    {
        while (rb.velocity.x < 5f)
        {
            rb.velocity += Vector3.right * 4;
            yield return null;
        }
    }
    private IEnumerator GoLeft()
    {
        while (rb.velocity.x > -5f)
        {
            rb.velocity += Vector3.left * 4;
            yield return null;
        }
    }
    private void SetColors()
    {
        if(objColor == ObjColor.blue)
        {
            ColorUtility.TryParseHtmlString("#00FFCD", out currentColor);
        }
        if(objColor == ObjColor.pink)
        {
            ColorUtility.TryParseHtmlString("#FF00EA", out currentColor);
        }
        if(objColor == ObjColor.purple)
        {
            ColorUtility.TryParseHtmlString("#D11DFF", out currentColor);
        }
        if(objColor == ObjColor.yellow)
        {
            ColorUtility.TryParseHtmlString("#FFDA00", out currentColor);
        }
        if (objColor == ObjColor.white)
        {
            ColorUtility.TryParseHtmlString("#FFFFFF", out currentColor);
        }
        if (objColor == ObjColor.red)
        {
            ColorUtility.TryParseHtmlString("#FF1D4D", out currentColor);
        }
    }
}
