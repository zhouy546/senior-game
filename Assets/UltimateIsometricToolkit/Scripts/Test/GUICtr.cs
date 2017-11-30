using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.UltimateIsometricToolkit.Scripts.Core
{
    public class GUICtr : MonoBehaviour
    {
        public GameObject[] key;
        public Sprite[] value;
        public Dictionary<GameObject, Sprite> groundeDictionary = new Dictionary<GameObject, Sprite>();
        // Use this for initialization
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
            AstarTestScript.instance.CurrentSelect.sprite = groundeDictionary[_thisgameObject.gameObject];
        }
    }
}
