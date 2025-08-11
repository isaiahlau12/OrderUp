using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerManager : MonoBehaviour
{
    public Image customerFace;
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI scoreToBeatText;

    public CustomerData[] customers;
    private int currentCustomerIndex = 0;
    private CustomerData currentCustomer;

    void Start()
    {
        LoadCustomer(currentCustomerIndex);
    }

    public void LoadCustomer(int index)
    {
        if (index < 0 || index >= customers.Length) return;

        currentCustomer = customers[index];

        customerFace.sprite = currentCustomer.neutralPortrait;
        customerNameText.text = currentCustomer.customerName;
        scoreToBeatText.text = $"Score to beat: {currentCustomer.scoreToBeat}";
    }

    public void ReactToSatisfaction(int currentScore)
    {
        if (currentCustomer == null) return;

        if (currentScore >= currentCustomer.scoreToBeat)
        {
            customerFace.sprite = currentCustomer.happyPortrait;
        }
        else
        {
            customerFace.sprite = currentCustomer.sadPortrait;
        }
    }

    public int GetCurrentScoreToBeat()
    {
        return currentCustomer != null ? currentCustomer.scoreToBeat : 0;
    }

    public void NextCustomer()
    {
        currentCustomerIndex++;
        if (currentCustomerIndex >= customers.Length)
            currentCustomerIndex = 0;

        LoadCustomer(currentCustomerIndex);
    }
}
