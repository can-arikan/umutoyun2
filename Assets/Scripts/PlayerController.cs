using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Rigidbody myRb;
    private GameObject stickman;
    private float jumpCount = 2,animBlend;
    private Vector3 localPos;
    private bool isLocal;
    private int starCount = -1;

    [SerializeField] Animator anim;
    [SerializeField] SkinnedMeshRenderer animSkinnedMeshRenderer,poseSkinnedMeshRenderer;
    [SerializeField] GameObject pose,headAnim,headPose,checkObj;
    [SerializeField] Material mat;
    [SerializeField] ParticleSystem jumpEffect,startEffect;
    [SerializeField] GameObject[] starts;
    [SerializeField] Collider myCol;
    [SerializeField] LayerMask layerMask;

    public float jumpForce;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        GetVariablesOnStart();
        mat.color = Color.white;
        mat.SetColor("_EmissionColor", Color.white);
    }
    private void GetVariablesOnStart()
    {
        myRb = GetComponent<Rigidbody>();
        poseSkinnedMeshRenderer.enabled = false;
        headPose.SetActive(false);
        animSkinnedMeshRenderer.enabled = false;
        headAnim.SetActive(false);
        myRb.isKinematic = true;
    }

    private void Update()
    {
        if (!Physics.CheckSphere(checkObj.transform.position, 0.5f, layerMask))
        {
            if (myCol.isTrigger) myCol.isTrigger = false;
        }

        MovementFunc();
        AnimBlend();

        if (isLocal)
        {
            transform.localPosition = localPos;
        }
    }

    private void AnimBlend()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            // Do stuf !!
            //Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
            animBlend = Mathf.Lerp(animBlend, 1.2f, Time.deltaTime * 3);
            anim.SetFloat("Blend", animBlend);
        }
        else
        {
            animBlend = Mathf.Lerp(animBlend, 0, Time.deltaTime * 20);
            anim.SetFloat("Blend", animBlend);
        }
    }

    private void MovementFunc()
    {
        if(MyGameManager.instance.gameState == MyGameManager.GameState.Null)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.LeftArrow))
                MyGameManager.instance.StartGame();

        }


        if(MyGameManager.instance.gameState == MyGameManager.GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && jumpCount > 0)
            {
                //Jump Right
                pose.transform.localPosition = new Vector3(Mathf.Abs(pose.transform.localPosition.x), pose.transform.localPosition.y, pose.transform.localPosition.z);
                isLocal = false;
                transform.parent = null;
                myRb.velocity = (Vector3.right + Vector3.up * 2) * jumpForce;
                myRb.isKinematic = false;
                transform.rotation = Quaternion.Euler(0, 90, 0);
                poseSkinnedMeshRenderer.enabled = false;
                headPose.SetActive(false);
                headAnim.SetActive(true);
                animSkinnedMeshRenderer.enabled = true;
                anim.SetTrigger("Jump");
                myRb.useGravity = true;
                jumpCount--;
                jumpEffect.Play();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && jumpCount > 0)
            {
                //Jump Left
                pose.transform.localPosition = new Vector3(-Mathf.Abs(pose.transform.localPosition.x), pose.transform.localPosition.y, pose.transform.localPosition.z);
                isLocal = false;
                transform.parent = null;
                myRb.velocity = (Vector3.left + Vector3.up * 2) * jumpForce;
                myRb.isKinematic = false;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                poseSkinnedMeshRenderer.enabled = false;
                headPose.SetActive(false);
                headAnim.SetActive(true);
                animSkinnedMeshRenderer.enabled = true;
                anim.SetTrigger("Jump");
                myRb.useGravity = true;
                jumpCount--;
                jumpEffect.Play();
            }
        }
    }

    public void StartPlayer()
    {
        animSkinnedMeshRenderer.enabled = true;
        headAnim.SetActive(true);
        startEffect.Play();
        myRb.isKinematic = false;

        anim.SetTrigger("Jump");

        Destroy(startEffect.gameObject.transform.parent.gameObject, 1.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fail"))
        {
            MyGameManager.instance.Fail();
        }
        if (other.gameObject.CompareTag("Star"))
        {
            other.GetComponent<StarController>().novaEffect.gameObject.transform.parent = null;
            other.GetComponent<StarController>().novaEffect.Play();
            other.gameObject.SetActive(false);
            ScoreManager.instance.AddStarCount();
            starCount++;
            starts[starCount].SetActive(true);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("JumpObjects"))
        {
            myRb.useGravity = false;
            myRb.velocity = Vector3.zero;
            transform.parent = other.gameObject.transform;
            isLocal = true;
            localPos = transform.localPosition;
            Vector3 lookPos = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
            transform.LookAt(lookPos);

            animSkinnedMeshRenderer.enabled = false;
            poseSkinnedMeshRenderer.enabled = true;
            headPose.SetActive(true);
            headAnim.SetActive(false);
            jumpCount = 2;
            SetColor(other.gameObject.GetComponent<JumpObjController>().currentColor);              
        }
    }

    public int GetStarCount()
    {
        return starCount;
    }
    private void SetColor(Color color)
    {
        mat.color = color;
        mat.SetColor("_EmissionColor", color);
    }
}
