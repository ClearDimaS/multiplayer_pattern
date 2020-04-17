
using DataController = GameSparksTutorials.DataController;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using GameObject = UnityEngine.GameObject;
using Image = UnityEngine.UI.Image;


public class ExpBarScript : MonoBehaviour
{
    Image Ground;
    float maxExp = 100f;
    public static float CurrentExp = 0;
    public GameObject loadingscreen;

    void Start()
    {
        Invoke("UpdateExp", 1);
    }

    private void UpdateExp()
    {
        Ground = GetComponent<Image>();
        CurrentExp = (float)DataController.GetValue<int>("Exp") % 100f;
        Ground.fillAmount = CurrentExp / maxExp;
        loadingscreen.SetActive(false);
    }
}
