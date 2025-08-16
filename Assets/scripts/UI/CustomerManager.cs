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
        Debug.Log($"Loading customer: {currentCustomer.customerName}");
    }

    public void ReactToSatisfaction(int currentScore)
    {
        if (currentCustomer == null) return;

        bool happy = currentScore >= currentCustomer.scoreToBeat;
        if (customerFace)
            customerFace.sprite = happy ? currentCustomer.happyPortrait : currentCustomer.sadPortrait;

        Debug.Log(happy
            ? $"{currentCustomer.customerName} is happy!"
            : $"{currentCustomer.customerName} is not satisfied...");
    }
    public CustomerData GetCurrentCustomer()
    {
        return currentCustomer;
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
