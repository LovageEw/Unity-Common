using System.Collections.Generic;

namespace UIs
{
    public class SlideScreenStacker : SingletonMonoBehaviour<SlideScreenStacker>
    {
        private readonly Dictionary<string, Stack<string>> sceneStackDict = new Dictionary<string, Stack<string>>();
        
        public Stack<string> GetStackReference(string sceneName)
        {
            if (!sceneStackDict.ContainsKey(sceneName))
            {
                sceneStackDict.Add(sceneName, new Stack<string>());
            }
            return sceneStackDict[sceneName];
        }

        public void Clear(string title)
        {
            if (sceneStackDict.ContainsKey(title))
            {
                sceneStackDict.Remove(title);
            }
        }
    }
}