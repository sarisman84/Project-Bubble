using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DataContainer : ScriptableObject
{
    [SerializeField] RelationshipLevel relationship;
    [SerializeField] int trust = 0;
    [SerializeField] int fear = 0;
    public void AffectAttribute(RelationshipAttribute attribute, int change)
    {
        switch (attribute)
        {
            case RelationshipAttribute.Trust:
                trust += change;
                break;
            case RelationshipAttribute.Fear:
                fear += change;
                break;
        }
        CalculateAndCheckRelationshipLevel();
    }

    public void AffectAttribute(List<RelationshipAttribute> attributes, List<int> changes)
    {
        if (attributes.Count != changes.Count)
        {
            Debug.LogWarning("Attributes of NPC wont change since the attributes changes are not correctly set up");
            return;
        }
        for (int i = 0; i < attributes.Count; i++)
        {
            switch (attributes[i])
            {
                case RelationshipAttribute.Trust:
                    trust += changes[i];
                    break;
                case RelationshipAttribute.Fear:
                    fear += changes[i];
                    break;
            }
        }
        CalculateAndCheckRelationshipLevel();
    }

    int trustPerLevel = 10;
    public RelationshipLevel CalculateAndCheckRelationshipLevel()
    {
        relationship = (RelationshipLevel)Mathf.FloorToInt(trust / trustPerLevel);
        return relationship;
    }
}
