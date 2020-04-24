
using Text = UnityEngine.UI.Text;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using DataController = GameSparksTutorials.DataController;


public class GetBreadAmmount : MonoBehaviour
{
    private Text BreadAmmount;
    public static bool Updated = false;

    private void Start()
    {
        Updated = false;
    }
    void Update()
    {
        if (!Updated)
        {
            Updated = true;
            Invoke("UpdateBread", 1.0f);
        }
    }

    public void UpdateBread()
    {
        BreadAmmount = GetComponent<Text>();

        BreadAmmount.text = DataController.GetValue<int>("Bread").ToString();
    }
}