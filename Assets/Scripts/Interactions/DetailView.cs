using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//opens the detail view of an interactable object
public class DetailView : MonoBehaviour
{
    public Button closeBtn;
    private GameManager gm;
    public UnityEvent onClose;
    
    void Start()
    {
        transform.gameObject.SetActive(false);
        gm = GameManager.Instance;
        closeBtn.onClick.AddListener(Close);
    }

    public void Open(){
        transform.gameObject.SetActive(true);
        gm.OpenOverlay();
    }

    public void Close() {
        transform.gameObject.SetActive(false);
        gm.CloseOverlay();
        onClose.Invoke();
    }
}
