namespace POS.Products.Data.Models
{
    public enum ProductType
    {
        Generic = 0, // Default value for items that do not have modifiers like a Can or Bottle of a beverage
        Single = 1, // Used for products that have simple modifiers like a burguer that can have extra cheese, remove bacon, etc. to the whole product. 
        Halves = 2 // Used for products that have complex modifiers like a pizza that can have different toppings on either the whole pizza or just the left/right half.
    }
}
