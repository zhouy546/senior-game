using Assets.UltimateIsometricToolkit.Scripts.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.UltimateIsometricToolkit.Scripts.Core
{
    public class GUICtr : MonoBehaviour
    {
        [SerializeField] GridGraph gridGraph;
        public GameObject[] key;
        public Sprite[] value;
        public Dictionary<GameObject, Sprite> groundeDictionary = new Dictionary<GameObject, Sprite>();
        // Use this for initialization
        void OnEnable() {
        }


        void Start()
        {
            for (int i = 0; i < key.Length; i++)
            {
                groundeDictionary.Add(key[i], value[i]);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }




        public void ReplaceGround(GameObject _thisgameObject)
        {
            if (AstarTestScript.instance.CurrentSelect != null) {
                AstarTestScript.instance.CurrentSelect.sprite = groundeDictionary[_thisgameObject.gameObject];

                Vector2 GridPos = GridGraph.NodePosToGridPos(AstarTestScript.instance.CurrentSelect.gameObject.GetComponent<IsoTransform>().Position);
                gridGraph._gridGraph[GridPos][0].Passable = true;
                Debug.Log("am i a question" + gridGraph._gridGraph[GridPos][0].IsQuestion);

                //TriggerEVENT;
                AstarTestScript.instance.RegisterPlayer[AstarTestScript.instance.currentPlayer].PaveRoad();
            }    
        }
    }
}
