using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum AttachmentRule{KeepRelative,KeepWorld,SnapToTarget}

public class TreasureHunter : MonoBehaviour
{

    public TreasureHunterInventory inventory;
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
    public GameObject coneCenter;
    Collider[] hitColliders;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!scary.GetComponent<AudioSource>().isPlaying) {
            scary.SetActive(false);
        }

        hipPos = camera.transform.position - new Vector3(0, 1, 0);

        if (OVRInput.GetDown(OVRInput.RawButton.A)){
            forceGrab(true);
        }


        void forceGrab(bool pressedA){
            RaycastHit outHit;
            if (Physics.Raycast(rightPointerObject.transform.position, rightPointerObject.transform.up, out outHit, 100.0f,collectiblesMask)){
                AttachmentRule howToAttach=pressedA?AttachmentRule.KeepWorld:AttachmentRule.SnapToTarget;
                attachGameObjectToAChildGameObject(outHit.collider.gameObject,rightPointerObject.gameObject,howToAttach,howToAttach,AttachmentRule.KeepWorld,true);
                thingIGrabbed=outHit.collider.gameObject;

                //A6:
                if (inventory.inv.Count == rand && soundPlayed == 0) { 
                    scary.SetActive(true);
                    scary.GetComponent<AudioSource>().Play(0);
                    soundPlayed = 1;
                    
                }

            }
        }

        if (OVRInput.GetUp(OVRInput.RawButton.A)) {
            letGo();
        }
        previousPointerPos=rightPointerObject.gameObject.transform.position;

        hitColliders = Physics.OverlapSphere(coneCenter.transform.position, 0.1f);
        
        if (Input.GetKeyDown("c") && hitColliders.Length > 0) {
            for (int i=0; i < hitColliders.Length; i++) {
                print(hitColliders[i].gameObject.name);
                numCube = 0;
                numRect = 0;
                numBig = 0;
                numCyl = 0;
                totalVal = 0;
                for (int j = 0; j < inventory.inv.Count; j++) {
                    if (inventory.inv[j].path == "Cube") {
                        numCube++;
                    } else if (inventory.inv[j].path == "Rect") {
                        numRect++;
                    } else if (inventory.inv[j].path == "BIG cube") {
                        numBig++;
                    } else if (inventory.inv[j].path == "Cylinder") {
                        numCyl++;
                    }
                    totalVal = totalVal + inventory.inv[j].value;
                }
                Destroy(hitColliders[i].gameObject);
            }
        }
    }

    void letGo(){
        //outputText.text = "LetGo called";
        if (thingIGrabbed) {
            if (   thingIGrabbed.transform.position.x >= (hipPos.x - 0.5f) && thingIGrabbed.transform.position.x <= (hipPos.x + 0.5f)
                && thingIGrabbed.transform.position.y >= (hipPos.y - 0.5f) && thingIGrabbed.transform.position.y <= (hipPos.y + 0.5f)
                && thingIGrabbed.transform.position.z >= (hipPos.z - 0.5f) && thingIGrabbed.transform.position.z <= (hipPos.z + 0.5f)  ) 
                {
                    inventory.inv.Add((Collectible)Resources.Load(thingIGrabbed.GetComponent<Collectible>().path, typeof(Collectible)));
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
        GOToDetach.transform.parent=null;
        handleAttachmentRules(GOToDetach,locationRule,rotationRule,scaleRule);
    }

        public static void handleAttachmentRules(GameObject GOToHandle, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule){
        GOToHandle.transform.localPosition=
        (locationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.position:
        (locationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localPosition:
        new Vector3(0,0,0);

        GOToHandle.transform.localEulerAngles=
        (rotationRule==AttachmentRule.KeepRelative)?GOToHandle.transform.eulerAngles:
        (rotationRule==AttachmentRule.KeepWorld)?GOToHandle.transform.localEulerAngles:
        new Vector3(0,0,0);

        GOToHandle.transform.localScale=
        (scaleRule==AttachmentRule.KeepRelative)?GOToHandle.transform.lossyScale:
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
                Rigidbody newRB=target.AddComponent<Rigidbody>();
                newRB.velocity=oldParentVelocity;
            }
        }
    }


}
