namespace UIHandlerScripts
{
    using UnityEngine;
    using UnityEngine.UI;
    using GameplayScripts;

    public class SellButtonHandler : MonoBehaviour
    {
        private Button _sellButton;

        private void Start()
        {
            _sellButton = GetComponent<Button>();
            _sellButton.onClick.AddListener(OnSellButtonClick);
            _sellButton.gameObject.SetActive(false);
        }

        private void OnMouseEnter()
        {
            _sellButton.gameObject.SetActive(true);
        }

        private void OnMouseExit()
        {
            _sellButton.gameObject.SetActive(false);
        }

        private void OnSellButtonClick()
        {
            Shooter shooter = GetComponentInParent<Shooter>();
            if (shooter != null)
            {
                shooter.RefundAndRemoveAlly();
            }
        }
    }
}