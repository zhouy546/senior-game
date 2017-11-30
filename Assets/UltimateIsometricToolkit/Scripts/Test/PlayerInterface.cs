using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.UltimateIsometricToolkit.Scripts.Pathfinding
{
    public interface playerInterface
    {
        bool enable { get; set; }
        bool ismoving { get; set; }
        bool ispave { get; set; }
        bool standOnQuestion { get; set; }
    }
}
