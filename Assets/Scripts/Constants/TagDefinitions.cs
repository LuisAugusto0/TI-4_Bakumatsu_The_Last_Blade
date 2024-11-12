
// Enum to limit selection type and allow dropdown on editor
public enum Tag
{
    Player,
    Enemy,
    NPC,
    None
}

public static class TagExtensions
{
    public static string ToTagString(this Tag tag)
    {
        return tag.ToString();
    }
}




// -- Previous much more complex implementation. In here in case
// its needed again -- 

// using System;

// public readonly struct Tag : IEquatable<Tag>
// {
//     private readonly string value;
    
//     private Tag(string value)
//     {
//         this.value = value;
//     }

//     public static readonly Tag Player = new Tag("Player");
//     public static readonly Tag Enemy = new Tag("Enemy");

//     // Implicit conversion to string to use directly in comparisons
//     public static implicit operator string(Tag tag) => tag.value;
    
//     // Use string for hashcode
//     public override int GetHashCode() => value.GetHashCode();

//     public bool Equals(Tag other) => value == other.value;
//     public static bool operator ==(Tag left, Tag right) => left.Equals(right);
//     public static bool operator !=(Tag left, Tag right) => !left.Equals(right);


//     // Necessary for operator consisency. Libraries such as Dictionary may use this method
//     public override bool Equals(object obj) => obj is Tag other && Equals(other);

// }

// public static class TagDefinitions
// {
//     public static readonly Tag Player = Tag.Player;
//     public static readonly Tag Enemy = Tag.Enemy;
// }

