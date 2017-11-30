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
        public Player currentPlayer = new Player();

        public enum PlayerSwitch { Player1, Player2 }
        PlayerSwitch Players;

        public  IsoRaycastHit isoRaycastHit;
        public static AstarTestScript instance;

        #region GUI
        public SpriteRenderer CurrentSelect;
        [SerializeField] RectTransform ReplaceUI;
        [SerializeField] Image BlackScreen;
        public bool isUIopen;
        #endregion

        public bool ismoving;

        [SerializeField] AnimationCurve myAlphaCurve;

        public AstarAgent[] AstarAgent = new AstarAgent[2];
        public Player[] player = new Player[2];

        public Dictionary<Player, AstarAgent> RegisterPlayer = new Dictionary<Player, AstarAgent>();
        public int index = 0;
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
            RegisterPlayer.Add(player[0], AstarAgent[0]);
            RegisterPlayer.Add(player[1], AstarAgent[1]);
        }

        void Update()
        {
            MouseInterAction();
        }

        void MouseInterAction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(currentPlayer.name);
                RegisterPlayer[currentPlayer].Interactions(true);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(currentPlayer.name);
                RegisterPlayer[currentPlayer].Interactions(false);
            }
        }

        bool CastRayFromMouse() {
            var isoRay = Isometric.MouseToIsoRay();
            return IsoPhysics.Raycast(isoRay, out isoRaycastHit);
        }

        public IsoRaycastHit gethit() {
            var isoRay = Isometric.MouseToIsoRay();
            IsoRaycastHit isoRaycasthit;
            if (IsoPhysics.Raycast(isoRay, out isoRaycasthit)) {
                return isoRaycasthit;
            }
            return isoRaycasthit;
        }

        public void getCurrentSelection(){
            if (CastRayFromMouse())
            {
                if (CurrentSelect != null) { SetGroundColor(Color.white); }
                CurrentSelect = isoRaycastHit.IsoTransform.GetComponent<SpriteRenderer>();
                //   Debug.Log(isUIopen);
                SetGroundColor(Color.gray);
            }
        }
        /*
                void OnLeftClkick() {
                    if (Input.GetMouseButtonDown(0))
                    {

                        if (!ismoving && CastRayFromMouse()) {
                            Movement(isoRaycastHit);
                        }
                    }
                }

                void Movement(IsoRaycastHit isoRaycastHit)
                {         
                    if (isUIopen) {
                        CloseUI(out isUIopen);
                    }

                    if (index > 0)
                    {
                        //	player 1 
                       // int index = getPlayerID("Player1");
                        AstarAgent[index].MoveTo(isoRaycastHit.Point);
                        Debug.Log("player1 moving");
                    }
                    else
                    {
                        //player 2
                       // int index = getPlayerID("Player2");
                        AstarAgent[index].MoveTo(isoRaycastHit.Point);
                        Debug.Log("Player2 moving");

                    }            
                }

        */


        void OnRightClick() {
            if (Input.GetMouseButtonDown(1)&&!ismoving)
            {
                if (CastRayFromMouse())
                {
                    if (CurrentSelect != null) { SetGroundColor(Color.white); }
                    CurrentSelect = isoRaycastHit.IsoTransform.GetComponent<SpriteRenderer>();
                    //   Debug.Log(isUIopen);

                    if (!isUIopen)
                    {
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

   //     int getPlayerID(string _Name)
   //     {
    //        return RegisterPlayer[_Name];
   //     }

       public void OpenUI(out bool _IsUIopen) {
            _IsUIopen = true;
            LeanTween.moveX(ReplaceUI.gameObject, 50f, .3f).setOnComplete( delegate() {
            });                  
         }

        public void OpenUI() {
            isUIopen = true;
            LeanTween.moveX(ReplaceUI.gameObject, 50f, .3f).setOnComplete(delegate () {
            });
        }
        public void CloseUI() {
            isUIopen = false;
            LeanTween.moveX(ReplaceUI.gameObject, -50f, .3f);
            SetGroundColor(Color.white);
        }

      public  void CloseUI(out bool _IsUIopen) {
            _IsUIopen = false;
        }

        void SetGroundColor(Color _color) {
            CurrentSelect.color = _color;
        }
/*
        public void changePlayer(PlayerSwitch playerSwitch)
        {
            switch (playerSwitch) {
                case PlayerSwitch.Player1: {
                        index = 0;
                        currentPlayer = AstarAgent[index].player;
                        break;
                    }
                case PlayerSwitch.Player2: {
                        index = 1;
                        currentPlayer = AstarAgent[index].player;
                        break;
                    }
            }

            index = -index;
            LeanTween.value(0, 1, 1f).setEase(myAlphaCurve).setOnUpdate((float value) =>
                {
                    BlackScreen.color = new Color(0, 0, 0, value);
                }).setOnComplete(delegate ()
                {
                    ismoving = false;
                });     
        }
        */
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
