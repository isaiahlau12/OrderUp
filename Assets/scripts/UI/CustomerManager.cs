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
    private bool[] satisfiedFlags;

    void Start()
    {
        satisfiedFlags = new bool[customers.Length];
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
    public int GetCurrentIndex()
    {
        return currentCustomerIndex;
    }

    public int GetTotalCustomers()
    {
        return customers != null ? customers.Length : 0;
    }

    // Marks the current customer as satisfied.
    public void MarkCurrentCustomerSatisfied()
    {
        if (currentCustomerIndex >= 0 && currentCustomerIndex < satisfiedFlags.Length)
            satisfiedFlags[currentCustomerIndex] = true;
    }

    //Returns true if all customers were satisfied.
    public bool IsAllCustomersSatisfied()
    {
        if (satisfiedFlags == null) return false;
        foreach (bool b in satisfiedFlags) if (!b) return false;
        return true;
    }
    // advance to the next customer. If there are no more customers, returns false.
    public bool NextCustomer()
    {
        currentCustomerIndex++;
        if (currentCustomerIndex >= customers.Length)
        {
            // No more customers
            return false;
        }
        LoadCustomer(currentCustomerIndex);
        return true;
    }
}
