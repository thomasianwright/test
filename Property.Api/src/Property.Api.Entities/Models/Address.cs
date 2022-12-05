namespace Property.Api.Entities.Models;

public class Address
{
    public int Id { get; set; }
    public string LineOne { get; set; }
    public string? LineTwo { get; set; }
    public string? LineThree { get; set; }
    public string Postcode { get; set; }
    public string? County { get; set; }
    public string Country { get; set; }
}