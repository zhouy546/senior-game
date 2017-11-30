using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.physics;
using Assets.UltimateIsometricToolkit.Scripts.Pathfinding;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Converts mouse input to 3d coordinates and invokes A* pathfinding
/// </summary>
/// 
namespace Assets.UltimateIsometricToolkit.Scripts.Core
{
    public class AstarTestScript : MonoBehaviour
    {
        public static AstarTestScript instance;

      public  SpriteRenderer CurrentSelect;


       [SerializeField] RectTransform ReplaceUI;

       [SerializeField]
        AnimationCurve myAlphaCurve;

        bool ismoving;
        public float speed;

        [SerializeField]
        Image BlackScreen;

        [SerializeField]
        Sprite[] replaceGroundSprite;

        bool isUIopen;

        public AstarAgent[] AstarAgent = new AstarAgent[2];
        public Dictionary<string, int> RegisterPlayer = new Dictionary<string, int>();
        int index = -1;
        // Update is called once per frame
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }


        void Start()
        {

            RegisterPlayer.Add("Player1", 0);
            RegisterPlayer.Add("Player2", 1);

        }

        void Update()
        {
            Movement(changePlayer);
            if (Input.GetMouseButtonDown(1))
            {
                var isoRay = Isometric.MouseToIsoRay();
                IsoRaycastHit isoRaycastHit;
                if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
                {
                    if (CurrentSelect != null) { SetGroundColor(Color.white); }
                    CurrentSelect = isoRaycastHit.IsoTransform.GetComponent<SpriteRenderer>();
                    Debug.Log(isUIopen);
                    if (!isUIopen) {
                        OpenUI(out isUIopen);
                    }    
                    SetGroundColor(Color.gray);
                }
                else
                {
                    if (isUIopen)
                    {
                        CloseUI(out isUIopen);
                    }
                }
            }
        }

        int getPlayerID(string _Name)
        {
            return RegisterPlayer[_Name];
        }

        void OpenUI(out bool _IsUIopen) {
            _IsUIopen = true;
            LeanTween.moveX(ReplaceUI.gameObject, 50f, .3f).setOnComplete( delegate() {
                Debug.Log("open");
            });                  
         }

        void CloseUI(out bool _IsUIopen) {
            _IsUIopen = false;
            LeanTween.moveX(ReplaceUI.gameObject, -50f, .3f);
            SetGroundColor(Color.white);
            CurrentSelect = null;
            Debug.Log("close"); 
        }

        void SetGroundColor(Color _color) {
            CurrentSelect.color = _color;
        }

        void Movement(UnityAction CallAction)
        {
            //raycast when mouse clicked
            if (Input.GetMouseButtonDown(0) && !ismoving)
            {
                ismoving = true;
                var isoRay = Isometric.MouseToIsoRay();
                IsoRaycastHit isoRaycastHit;
                if (IsoPhysics.Raycast(isoRay, out isoRaycastHit))
                {
                    Debug.Log("Moving to " + isoRaycastHit.Point);
                    index = -index;
                    if (index > 0)
                    {
                        //	player 1 
                        int index = getPlayerID("Player1");
                        AstarAgent[index].MoveTo(isoRaycastHit.Point);
                        Debug.Log("player1 moving");

                        // event""
                        CallAction();
                    }
                    else
                    {
                        //player 2
                        int index = getPlayerID("Player2");
                        AstarAgent[index].MoveTo(isoRaycastHit.Point);
                        Debug.Log("Player2 moving");
                        CallAction();
                        //event""
                    }
                }
            }
        }

        void changePlayer()
        {

            LeanTween.value(0, 1, 1f).setDelay(2).setEase(myAlphaCurve).setOnUpdate((float value) =>
            {
                BlackScreen.color = new Color(0, 0, 0, value);
            }).setOnComplete(delegate ()
            {
                ismoving = false;
            });
        }
        /// <summary>
        /// Changezeros the and one.
        /// </summary>
        /// <returns>The and one.</returns>
        /// <param name="_num">put one or zero</param>
        int ChangezeroAndOne(int _num)
        {
            if (_num == 0)
            {
                return 1;
            }
            if (_num == 1)
            {
                return 0;
            }

            return 0;

        }

    }
}
