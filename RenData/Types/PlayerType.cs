namespace RenData.Types;

public enum PlayerType
{
    PLAYERTYPE_FIRST = -4,

    PLAYERTYPE_SPECTATOR = PLAYERTYPE_FIRST,    // -4
    PLAYERTYPE_MUTANT,                                          // -3
    PLAYERTYPE_NEUTRAL,                                         // -2
    PLAYERTYPE_RENEGADE,                                            // -1
    PLAYERTYPE_NOD,                                             //  0
    PLAYERTYPE_GDI,                                             //  1

    PLAYERTYPE_LAST = PLAYERTYPE_GDI,

    PLAYERTYPE_MUTATION_MUTATED = PLAYERTYPE_NOD,
    PLAYERTYPE_MUTATION_REGULAR = PLAYERTYPE_GDI,
}
