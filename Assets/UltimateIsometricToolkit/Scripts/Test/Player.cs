using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.UltimateIsometricToolkit.Scripts.Pathfinding
{
    public class Player : MonoBehaviour, PlayerInterface
    {
        #region setupPlayerInterface
        public bool isenable { get; set; }
        public bool ismoving { get; set; }
        public bool ispave { get; set; }
        public bool standOnQuestion { get; set; }
        #endregion
        public Player()
        {

        }

        public Player(bool _Enable, bool _IsPave, bool _StandOnQuestion, bool _IsMoving = true)
        {
            isenable = _Enable;
            ismoving = _IsMoving;
            ispave = _IsPave;
            standOnQuestion = _StandOnQuestion;
        }
    }
}
