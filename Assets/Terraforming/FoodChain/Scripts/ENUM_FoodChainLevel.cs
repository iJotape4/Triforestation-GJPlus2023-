[System.Flags]
public enum ENUM_FoodChainLevel
{
    AnimalKing = 1 << 0,
    Predator = 1 << 1, 
    Prey = 1 << 2,
    Bug = 1 << 3
}