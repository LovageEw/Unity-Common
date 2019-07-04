using UnityEngine;
using System.Collections;

namespace Managers {
    public class ManagerPrefab : SingletonMonoBehaviour<ManagerPrefab> {
        // Use this for initialization
        void Start() {
            DontDestroyOnLoad(this);
        }
    }
}