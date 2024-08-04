using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "ScriptableObjects/Storage")]
public class Storage : ScriptableObject
{
    public List<Cards> allCards;
    [Space]
    public List<Events> allEvents;
    [Space]
    public List<Characters> allCharacters;
    [Space]
    public List<Roles> allRoles;
}
