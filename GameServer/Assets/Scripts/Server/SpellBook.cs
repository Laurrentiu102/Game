using UnityEngine;

public class SpellBook : MonoBehaviour
{
    [SerializeField]
    private Spell[] spells;
    
    public Spell GetSpell(int index)
    {
        return spells[index];
    }
}