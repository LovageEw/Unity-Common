using UnityEngine;
using System.Collections;

namespace UIs {
    public abstract class LayoutGroupMonoBehaviour : MonoBehaviour {
        [SerializeField] protected GameObject childObject;
        [SerializeField] protected Transform container;

        protected virtual GameObject CreateChild() {
            GameObject child = Instantiate(childObject);
            child.transform.SetParent(container.transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
            return child;
        }

        protected virtual void OnDisable() {
            ClearContainer();
        }

        protected void ClearContainer() {
            foreach (Transform child in container) {
                Destroy(child.gameObject);
            }
        }
    }
}