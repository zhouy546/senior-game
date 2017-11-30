using System;
using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;
namespace Assets.UltimateIsometricToolkit.Scripts.Pathfinding {
    /// <summary>
    /// Astar monobehaviour
    /// </summary>
    [RequireComponent(typeof(IsoTransform)), AddComponentMenu("UIT/Pathfinding/A* Agent")]
    public class AstarAgent : MonoBehaviour {
        public delegate void DelegateEventHandler();
        public static event DelegateEventHandler Pave;


        //[HideInInspector]
        public Player player;
        public enum PlayerState { Pave, move, question, Idel }
        #region setupPlayerProperty    
        public bool amIGoingFirst;
        [System.Serializable]
        public struct Paramater {
            public int Pavetimes;
            public PlayerState playerState;
            public bool standOnQuestion;
        }

        public Paramater BaseParamater;
        public Paramater CurrentParamater;

        private bool _enable;
        public bool enable
        {
            get
            {
                return _enable;
            }
            set
            {
                player.isenable = _enable = value;
                player.ismoving = _ismoving = false;
                player.ispave = false;
                player.standOnQuestion = false;
                Debug.Log("enable is set to" + value);
            }
        }

        private bool _ismoving;
        public bool ismoving
        {
            get
            {
                return _ismoving;
            }
            set
            {
                _ismoving = value;
                player.ismoving = _ismoving = value;
                Debug.Log("enable is set to" + player.ismoving);
                if (_ismoving && standOnQuestion)
                {
                    //doingquestion
                }
                else {
                    ChangePlayer();
                    //changePlayer
                }
            }
        }

        private bool _ispave;
        public bool ispave
        {
            get
            {
                return _ispave;
            }
            set
            {
                Debug.Log("enable is set to" + value);
                _ispave = value;
                player.ispave = _ismoving = value;
            }
        }

        private bool _standOnQuestion;
        public bool standOnQuestion
        {
            get
            {
                return _standOnQuestion;
            }
            set
            {
                _standOnQuestion = value;
                player.standOnQuestion = _ismoving = value;
                Debug.Log("enable is set to" + value);
            }
        }
        #endregion

        public AstarTestScript astarTestScript;
        public float JumpHeight = 1; //vertical distance threshold to next node
        public float Speed = 2; //units per second
        public GridGraph Graph;
        public Heuristic heuristic;

        void OnEnable() {
            Pave += PaveRoad;
        }


        public void Start() {
            StartCoroutine(ini());

        }

        public void Interactions(bool _isLeftClisck) {
            CheckAndRunPlayerState(CurrentParamater.playerState, _isLeftClisck);
        }

        IEnumerator ini() {
            player.isenable = amIGoingFirst;
            if (amIGoingFirst)
            {
                AstarTestScript.instance.currentPlayer = AstarTestScript.instance.player[0] = player;
                CurrentParamater.playerState = PlayerState.Pave;
            }
            else {
                AstarTestScript.instance.player[1] = player;
                CurrentParamater.playerState = PlayerState.Idel;
            }
            yield return new WaitForSeconds(1f);
            CheckAndRunPlayerState(CurrentParamater.playerState);
        }

        public void ChangePlayer() {

            CurrentParamater.playerState = PlayerState.Idel;
           
                AstarTestScript.instance.index++;
                int a = AstarTestScript.instance.index % 2;

                Player player = AstarTestScript.instance.player[a];
                AstarTestScript.instance.currentPlayer = player;

            AstarAgent agent = AstarTestScript.instance.RegisterPlayer[AstarTestScript.instance.currentPlayer];
            agent.CurrentParamater.playerState = PlayerState.Pave;

            agent.enable = true;
            enable = false;
            CheckAndRunPlayerState(CurrentParamater.playerState,false);
          //  AstarTestScript.instance.OpenUI();
        }

        public void PaveStreet()
        {
            if (Pave != null)
            {
                Pave();
            }
        }

        public void CheckAndRunPlayerState(PlayerState _playerState, bool _isLeftClisck = false)
        {
            switch (_playerState)
            {
                case PlayerState.Pave:
                    EnablePlayerSetup(_isLeftClisck);
                    break;
                case PlayerState.move:
                    PlayerMove(_isLeftClisck);
                    break;
                case PlayerState.question:
                    break;
                case PlayerState.Idel:
                    break;
                default:
                    break;
            }
        }

        public void PaveRoad()
        {
            CurrentParamater.Pavetimes--;
            if (CurrentParamater.Pavetimes == 0)
            {
                ispave = true;
                CurrentParamater = BaseParamater;
                EnablePlayerMove();
                CheckAndRunPlayerState(CurrentParamater.playerState);
            }
        }

        public void EnablePlayerPave()
        {
            CurrentParamater.playerState = PlayerState.Pave;
        }

        public void EnablePlayerSetup(bool _isLeftClisck)
        {
            Debug.Log("EnablePlayerSetup+AstarTestScript.instance.isUIopen"+ AstarTestScript.instance.isUIopen);

            if (!AstarTestScript.instance.isUIopen)
            {
                AstarTestScript.instance.OpenUI();
            }
            if (!_isLeftClisck)
            {
                AstarTestScript.instance.getCurrentSelection();
            }
        }

        public void EnablePlayerMove()
        {
            CurrentParamater.playerState = PlayerState.move;
            if (AstarTestScript.instance.isUIopen)
            {
                AstarTestScript.instance.CloseUI();
            }

        }

        public void PlayerMoverSetup()
        {

        }

        public void PlayerMove(bool _isLeftClisck)
        {
            if (_isLeftClisck)
            {
                MoveTo(AstarTestScript.instance.gethit().Point);
            }
        }

        public void EnablePlayerQuestion()
        {
            CurrentParamater.playerState = PlayerState.question;
        }

        public void EnablePlayerIdel()
        {
            CurrentParamater.playerState = PlayerState.Idel;
        }
        /// <summary>
        /// Finds a path to given destination under a heuristic if such path exists
        /// </summary>
        /// <param name="destination"></param>
        public void MoveTo(Vector3 destination)
        {
            var astar = new Astar(GetFromEnum(heuristic));

            var startNode = Graph.ClosestNode(GetComponent<IsoTransform>().Position);
            var endNode = Graph.ClosestNode(destination);
            if (startNode == null)
            {
                Debug.LogError("Invalid position, no node found close enough to " + GetComponent<IsoTransform>().Position);
                return;
            }
            if (endNode == null)
            {
                Debug.LogError("Invalid position, no node found close enough to " + destination);
                return;
            }
            astar.SearchPath(startNode, endNode, JumpHeight, path =>
            {
                astarTestScript.ismoving = true;
                StopAllCoroutines();
                StartCoroutine(MoveAlongPathInternal(path));
            }, () =>
            {
                Debug.Log(this.gameObject.name + "No path found");
            });
        }

        private IEnumerator StepTo(Vector3 from, Vector3 to, float speed)
        {
            var timePassed = 0f;
            var isoTransform = GetComponent<IsoTransform>();
            var maxTimePassed = Vector3.Distance(from, to) / speed;
            while (timePassed + Time.deltaTime < maxTimePassed)
            {
                timePassed += Time.deltaTime;
                isoTransform.Position = Vector3.Lerp(from, to, timePassed / maxTimePassed);
                //	Debug.Log (isoTransform.Position);
                yield return null;
            }
        }

        private IEnumerator MoveAlongPathInternal(IEnumerable<Vector3> path)
        {
            foreach (var pos in path)
            {
                yield return StepTo(GetComponent<IsoTransform>().Position, pos + new Vector3(0, GetComponent<IsoTransform>().Size.y / 2, 0), Speed);
            }
            ismoving = true;
        }

        private Astar.Heuristic GetFromEnum(Heuristic heuristic)
        {
            switch (heuristic)
            {
                case Heuristic.EuclidianDistance:
                    return Astar.EuclidianHeuristic;
                case Heuristic.MaxAlongAxis:
                    return Astar.MaxAlongAxisHeuristic;
                case Heuristic.ManhattenDistance:
                    return Astar.ManhattanHeuristic;
                case Heuristic.AvoidVerticalSteeps:
                    return Astar.AvoidVerticalSteepsHeuristic;
                default:
                    throw new ArgumentOutOfRangeException("heuristic", heuristic, null);
            }
        }


        public enum Heuristic
        {
            EuclidianDistance,
            MaxAlongAxis,
            ManhattenDistance,
            AvoidVerticalSteeps
        }
    }
}