
using Text = UnityEngine.UI.Text;


namespace GameSparksTutorials
{
    public class GetUserName : GS_Base
    {
        private Text UserNam;

        private void Start()
        {
            UserNam = GetComponent<Text>();
            UserNam.text = DataController.GetValue<string>("displayName");
        }
    }
}