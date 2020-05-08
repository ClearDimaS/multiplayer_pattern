using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProcess
{
    //// Deep copy constructor
    //public GameProcess(GameProcess other) 
    //{}

    public GameObject playerObj;

    public GameObject playerInfoPanel;

    public GameObject playerInterface;

    public GameObject playerLoadingScreen;

    public Buttons buttons = new Buttons();

    public GameObject playerDebuffs;

    public bool iAmLeft;

    public string MineOrOther;

    public class Buttons 
    {
        public GameObject BtnLeave;

        public GameObject BtnsPlace;


        public GameObject BtnMoveRight;

        public GameObject BtnMoveLeft;

        public GameObject BtnLight;

        public GameObject BtnMedium;

        public GameObject BtnHeavy;

        public GameObject BtnMagic;

        public GameObject BtnSleep;
    }

    public Animations animation = new Animations();

    Animator Animator;

    public int WinAnimNumber = 1;

    public bool Animating;

    public class Animations
    {
        public Animator Animator;


        public void SetAnimator(Animator animator)
        {
            Animator = animator;

            // Get a list of the animation clips
            AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;

            // Iterate over the clips and gather their information
            foreach (AnimationClip animClip in animationClips)
            {
                //Debug.Log(animClip.name + ": " + animClip.length);
            }
        }


        string animationName;

        public void setAnimatorBoolTrue(string animBoolName)
        {
            if (Animator != null)
            {
                // Since Animation CLips names match anim bool names SetBool and Play use one parameter
                //Debug.Log("Trying to play : " + animBoolName);
                Animator.Play(animBoolName, 0);
                resetAnimator(animBoolName);
            }
        }


        void resetAnimator(string boolName)
        {
            if (Animator != null)
            {
                Animator.SetBool(boolName, false);
            }
        }

        GameObject objOfAnimation;

        public Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();

        public void instantiateText(string PrefabName, Vector3 animPosition, string textOfAnObject, Vector3 textPosition, bool isLeft, bool isDmg, bool isHeal, bool isMagic, bool isText, bool isStamHeal)
        {
            if (PrefabName != "")
            {
                Debug.Log(PrefabName);
                dict.TryGetValue(PrefabName, out objOfAnimation);

                GameObject.Instantiate(objOfAnimation, animPosition, Quaternion.identity);
            }

            if (textOfAnObject != null)
            {
                Vector3 LocSc;

                float ScMlt;

                dict.TryGetValue("ParentFloating", out objOfAnimation);

                objOfAnimation = GameObject.Instantiate(objOfAnimation, textPosition, Quaternion.identity);

                ScMlt = (1 - 30 / GameObject.FindObjectOfType<Camera>().fieldOfView) * 0.2f;

                LocSc = objOfAnimation.GetComponentInChildren<RectTransform>().localScale;

                if (isLeft)
                {
                    if (isDmg)
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;

                        if (isText)
                        {
                            objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(-12.0f, 4.5f, 0.0f);
                        }
                        else
                        {
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(-12.0f, 0.0f, 0.0f);
                        }
                    }
                    else if (isHeal)
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;
                        objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(-5.0f, 0.0f, 0.0f);
                    } else if (isStamHeal) 
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                        objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(-5.0f, 5.0f, 0.0f);
                    }
                    else if (isMagic)
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;

                        if (isText)
                        {
                            objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(-9.0f, 6.0f, 0.0f);
                        }
                        else
                        { 
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(-9.0f, 2.5f, 0.0f);
                        }
                    }

                }
                else 
                {
                    if (isDmg)
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;
                        if (isText)
                        {
                            objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(12.0f, 4.5f, 0.0f);
                        }
                        else
                        {
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(12.0f, 0.0f, 0.0f);
                        }
                    }
                    else if (isHeal)
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;
                        objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(5.0f, 0.0f, 0.0f);
                    }
                    else if (isStamHeal)
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                        objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(5.0f, 5.0f, 0.0f);
                    }
                    else if (isMagic)
                    {
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                        objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;
                        if (isText)
                        {
                            objOfAnimation.GetComponentsInChildren<SpriteRenderer>()[0].enabled = false;
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(9.0f, 6.0f, 0.0f);
                        }
                        else
                        {
                            objOfAnimation.GetComponentInChildren<RectTransform>().position += new Vector3(9.0f, 2.5f, 0.0f);
                        }
                    }
                }

                //Debug.Log(objOfAnimation.GetComponentInChildren<RectTransform>().localScale + "   " + objOfAnimation.GetComponentInChildren<RectTransform>().position + "   " + ScMlt); // Maybe youd like to change how texts are placed);

                objOfAnimation.GetComponentInChildren<RectTransform>().localScale = new Vector3(Mathf.Abs(LocSc.x * ScMlt), Mathf.Abs(LocSc.y * ScMlt), Mathf.Abs(LocSc.z * ScMlt));

                objOfAnimation.GetComponentInChildren<TextMesh>().text = textOfAnObject;
            }
        }
    }


    public string magicEquipped;
    public int Lvl;

    public int attack;
    public int agility;
    public int power;
    public int strength;
    public int endurance;
    public int speed;
    public int sleep;
    public int regen;

    public int baseDamage;
    public float heavyMult;
    public float mediumMult;
    public float lightMult;

    public float lightChance;
    public float mediumChance;
    public float heavyChance;
    public float blockTemp;

    public float ifLowHpDmg;
    public float blockMult;
    public float critMult;

    public int staminaPerMove;
    public float staminaHeavyMult;
    public float staminaMediumMult;
    public float staminaLightMult;
    public float staminaMagic;
    public float blockStam;
    public float moveStamMult;

    public int stamina = 1;
    public int maxStamina = 1;
    public int baseStamForSleep;
    public int baseStamForSwap;
    public float heavyDist;
    public float distToMove;
    public float speedToMove;
    public float startPos;
    public bool moveFarer;
    public bool moveCloser;
    public float moveMult;

    public int health = 1;
    public int maxHealth = 1;
    public float regBonus = 0;
    public int baseHealthForSleep;
    public int baseHealthForSwap;

    public bool bashed;
    public bool stunned;
    public float magicDebuffMult = 0.0f;
    public int onFire;
    public int cursed;
    public float curseDebuff;
    public float fireDebuff;

    public float armorModif;
    public float damageModif;
    public float blockModif;
    public float missModif;
    public float critModif;
    public float bashModif;
    public float stunModif;
    public float magicModif;

    public System.Random rand1;
    public System.Random rand2;
    public System.Random rand3;
    public System.Random rand4;
    public System.Random rand5;
    public System.Random rand6;

    public bool myTurn;
    public float turnTimer;
}
