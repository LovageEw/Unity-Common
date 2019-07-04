using UnityEngine;
using System.Collections;
using Managers;
using Databases;
using System.IO;

namespace Generals {
    public class GameObjectChecker : MonoBehaviour {

        void Awake() {

            if (GameObject.FindObjectOfType<TopMostCanvas>() == null) {
                Instantiate(Resources.Load("TopMostCanvas"));
            }
            if (GameObject.FindObjectOfType<ManagerPrefab>() == null) {
                Instantiate(Resources.Load("Manager"));
            }
        }
        
    }
}