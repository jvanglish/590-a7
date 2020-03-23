using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum AttachmentRule{KeepRelative,KeepWorld,SnapToTarget}

public class TreasureHunter : MonoBehaviour
{

    public TreasureHunterInventory inventory;
    //public Collectible one;
    //public Collectible two;
    //public Collectible three;
    public TextMesh invVal;
    public TextMesh win;
    public Camera camera;
    public GameObject leftPointerObject;
    public GameObject rightPointerObject;
    public TextMesh outputText;
    public TextMesh outputText2;
    public TextMesh outputText3;
    public TextMesh outputText4;
    public LayerMask collectiblesMask;
    GameObject thingOnGun;
    GameObject thingIGrabbed;
    Vector3 previousPointerPos;
    Vector3 hipPos;
    public int numCube;
    public int numRect;
    public int numBig;
    public int numCyl;
    public int totalVal;
    public int cubeCheck;
    public int rectCheck;
    public int bigCheck;
    public int cylCheck;
    public int soundPlayed;

    public GameObject scary;
    public int rand;

    // Start is called before the first frame update
    void Start()
    {
        cubeCheck = 0;
        rectCheck = 0;
        bigCheck = 0;
        cylCheck = 0;
        soundPlayed = 0;
        rand = Random.Range(1, 26);
        print(rand);
    }

    // Update is called once per frame
    void Update()
    {
        if (!scary.GetComponent<AudioSource>().isPlaying) {
            scary.SetActive(false);
        }
       /* if (Input.GetKeyDown("1")) {
            if (!(inventory.inv.Contains(one))) {
                inventory.inv.Add(one);
            }
        }
        if (Input.GetKeyDown("2")) {
            if (!(inventory.inv.Contains(two))) {
                inventory.inv.Add(two);
            }
        }
        if (Input.GetKeyDown("3")) {
            if (!(inventory.inv.Contains(three))) {
                inventory.inv.Add(three);
            }
        }
        if (Input.GetKeyDown("4")) {
            print("4 key pressed");
            int totalVal = 0;
            for (int i=0; i < inventory.inv.Count; i++) {
                totalVal = totalVal + inventory.inv[i].value;
            }
            invVal.text = "Value=" + totalVal.ToString() + ", " + inventory.inv.Count.ToString() + " total items. (Project by: Jordan Van Glish)";
        }
        if (inventory.inv.Count == 3) {
            win.text = "YOU ARE A CHAMPION AMONG MANKIND";
        }
        if (Input.GetKeyDown("e")) {
            print("E key pressed");
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.GetComponent<Collectible>()) {
                    //CODE GOES HERE

                    inventory.inv.Add((Collectible)Resources.Load(objectHit.gameObject.GetComponent<Collectible>().path, typeof(Collectible)));

                    //CODE GOES HERE
                    Destroy(objectHit.gameObject);
                }                
            }
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)){
            outputText3.text="Teleport";
            RaycastHit outHit;
            if (Physics.Raycast(rightPointerObject.transform.position, rightPointerObject.transform.up, out outHit, 100.0f))
            {
                //you can only see these rays when you leave Game window and look at Scene Window. Can't draw rays in-game like in UE4 unless you create your own Ray prefab
                Debug.DrawRay(rightPointerObject.transform.position, rightPointerObject.transform.up * outHit.distance, Color.red,10000);
                outputText2.text="hit ="+outHit.collider.gameObject;
                //remember that Unity is Y-up, so I need to swap the axes
                UnityEngine.AI.NavMeshHit navMeshHit;
                UnityEngine.AI.NavMesh.SamplePosition(new Vector3(outHit.point.x,this.gameObject.transform.position.y,outHit.point.z),out navMeshHit,10,UnityEngine.AI.NavMesh.AllAreas);
                this.gameObject.transform.position=new Vector3(navMeshHit.position.x,this.gameObject.transform.position.y,navMeshHit.position.z);
            }
        }*/

        //A5 start location

        hipPos = camera.transform.position - new Vector3(0, 1, 0);

        if (OVRInput.GetDown(OVRInput.RawButton.A)){
            //outputText3.text="Force GRAB";
            forceGrab(true);
        }


        void forceGrab(bool pressedA){
            RaycastHit outHit;
        //notice I'm using the layer mask again
            if (Physics.Raycast(rightPointerObject.transform.position, rightPointerObject.transform.up, out outHit, 100.0f,collectiblesMask)){
                AttachmentRule howToAttach=pressedA?AttachmentRule.KeepWorld:AttachmentRule.SnapToTarget;
                attachGameObjectToAChildGameObject(outHit.collider.gameObject,rightPointerObject.gameObject,howToAttach,howToAttach,AttachmentRule.KeepWorld,true);
                thingIGrabbed=outHit.collider.gameObject/*.GetComponent<Collectible>()*/;
                //outputText2.text="hit ="+outHit.collider.gameObject;

                //A6:
                if (inventory.inv.Count == rand && soundPlayed == 0) { 
                    scary.SetActive(true);
                    scary.GetComponent<AudioSource>().Play(0);
                    soundPlayed = 1;
                    
                }

            }
        }

        if (OVRInput.GetUp(OVRInput.RawButton.A)) {
            //outputText3.text="Force RELEASE";
            letGo();
        }
    previousPointerPos=rightPointerObject.gameObject.transform.position;
    }

    void letGo(){
        //outputText.text = "LetGo called";
        if (thingIGrabbed) {
            //outputText.text = "Object grabbed: " + thingIGrabbed.name;
        if (   thingIGrabbed.transform.position.x >= (hipPos.x - 0.5f) && thingIGrabbed.transform.position.x <= (hipPos.x + 0.5f)
            && thingIGrabbed.transform.position.y >= (hipPos.y - 0.5f) && thingIGrabbed.transform.position.y <= (hipPos.y + 0.5f)
            && thingIGrabbed.transform.position.z >= (hipPos.z - 0.5f) && thingIGrabbed.transform.position.z <= (hipPos.z + 0.5f)  ) 
            {
                //outputText.text = "Deposited into inventory";
                inventory.inv.Add((Collectible)Resources.Load(thingIGrabbed.GetComponent<Collectible>().path, typeof(Collectible)));
                //text stuff:
                if (thingIGrabbed.GetComponent<Collectible>().path == "Cube" && cubeCheck == 0) {
                        cubeCheck = 1;
                        outputText.text = outputText.text + "Cube" + "\n";
                    } else if (thingIGrabbed.GetComponent<Collectible>().path == "Rect" && rectCheck == 0) {
                        rectCheck = 1;
                        outputText.text = outputText.text + "Rect" + "\n";
                    } else if (thingIGrabbed.GetComponent<Collectible>().path == "BIG cube" && bigCheck == 0) {
                        bigCheck = 1;
                        outputText.text = outputText.text + "BIG cube" + "\n";
                    } else if (thingIGrabbed.GetComponent<Collectible>().path == "Cylinder" && cylCheck == 0) {
                        cylCheck = 1;
                        outputText.text = outputText.text + "Cylinder" + "\n";
                    }
                //outputText.text = outputText.text + thingIGrabbed.name + "\n";

                numCube = 0;
                numRect = 0;
                numBig = 0;
                numCyl = 0;
                totalVal = 0;
                for (int i = 0; i < inventory.inv.Count; i++) {
                    if (inventory.inv[i].path == "Cube") {
                        numCube++;
                    } else if (inventory.inv[i].path == "Rect") {
                        numRect++;
                    } else if (inventory.inv[i].path == "BIG cube") {
                        numBig++;
                    } else if (inventory.inv[i].path == "Cylinder") {
                        numCyl++;
                    }
                    totalVal = totalVal + inventory.inv[i].value;
                }

                outputText2.text = numCube.ToString() + " cubes, " + numRect.ToString() + " rects,\n" + numBig.ToString() + " BIGs, " + numCyl.ToString() + " cyls";
                outputText4.text = "Total Inv. Points = " + totalVal.ToString();

                Destroy(thingIGrabbed);
                thingIGrabbed = null; 
            }
        else {
                //outputText.text = "Deposited into world";
                detachGameObject(thingIGrabbed.gameObject,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld,AttachmentRule.KeepWorld);
                simulatePhysics(thingIGrabbed.gameObject,(rightPointerObject.gameObject.transform.position-previousPointerPos)/Time.deltaTime,true);
                thingIGrabbed=null;
        }
        }
    }

    public void attachGameObjectToAChildGameObject(GameObject GOToAttach, GameObject newParent, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule, bool weld){
            GOToAttach.transform.parent=newParent.transform;
            handleAttachmentRules(GOToAttach,locationRule,rotationRule,scaleRule);
            if (weld){
                simulatePhysics(GOToAttach,Vector3.zero,false);
            }
    }

    public static void detachGameObject(GameObject GOToDetach, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule){
        //making the parent null sets its parent to the world origin (meaning relative & global transforms become the same)
        GOToDetach.transform.parent=null;
        handleAttachmentRules(GOToDetach,locationRule,rotationRule,scaleRule);
    }

        public static void handleAttachmentRules(GameObject GOToHandle, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule){
        GOToHandle.transform.localPosition=
        (locationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.position:
        //technically don't need to change anything but I wanted to compress into ternary
        (locationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localPosition:
        new Vector3(0,0,0);

        //localRotation in Unity is actually a Quaternion, so we need to specifically ask for Euler angles
        GOToHandle.transform.localEulerAngles=
        (rotationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.eulerAngles:
        //technically don't need to change anything but I wanted to compress into ternary
        (rotationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localEulerAngles:
        new Vector3(0,0,0);

        GOToHandle.transform.localScale=
        (scaleRule==AttachmentRule.KeepRelative)?GOToHandle.transform.lossyScale:
        //technically don't need to change anything but I wanted to compress into ternary
        (scaleRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localScale:
        new Vector3(1,1,1);
    }
    public void simulatePhysics(GameObject target,Vector3 oldParentVelocity,bool simulate){
        Rigidbody rb=target.GetComponent<Rigidbody>();
        if (rb){
            if (!simulate){
                Destroy(rb);
            } 
        } else{
            if (simulate){
                //there's actually a problem here relative to the UE4 version since Unity doesn't have this simple "simulate physics" option
                //The object will NOT preserve momentum when you throw it like in UE4.
                //need to set its velocity itself.... even if you switch the kinematic/gravity settings around instead of deleting/adding rb
                Rigidbody newRB=target.AddComponent<Rigidbody>();
                newRB.velocity=oldParentVelocity;
            }
        }
    }


}
