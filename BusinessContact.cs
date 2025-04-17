public class BusinessContact : Contact
{
    public string Company { get; set; }

    public BusinessContact(string name, string phone, string company)
        : base(name, phone)
    {
        Company = company;
    }

    public override void Display()
    {
        Console.WriteLine($"[Business] {Name} @ {Company}, Phone: {Phone}");
    }
}
