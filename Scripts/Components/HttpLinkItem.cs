using Commons.Scripts.Utility;
using UnityEngine;

namespace UIs
{
    public class HttpLinkItem : MonoBehaviour
    {
        public void OpenLink(string link)
        {
            HttpUtility.Open(link);
        }
    }
}