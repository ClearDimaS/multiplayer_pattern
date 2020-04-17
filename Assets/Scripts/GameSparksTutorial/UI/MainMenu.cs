
using UnityEngine;
using UnityEngine.UI;

namespace GameSparksTutorials
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Text displayName;

        private void OnEnable()
        {
            displayName.text = DataController.GetValue<string>("displayName");
        }
    }
}
