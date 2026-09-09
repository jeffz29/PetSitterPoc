namespace PetSitter.Domain;

[Flags]
public enum ProviderSkills
{
    None = 0,
    Sitter = 1,
    Walker = 2,
    Trainer = 4
}