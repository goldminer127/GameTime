using System;
using System.Collections.Generic;
using System.Text;

namespace GameTime.Models
{
    public class Part : Item
    {
        public PartType PartType { get; private set; }
        public int MaterialCost { get; private set; }
        public Part()
        {

        }/*
        public Part()
        {
            PartType = type;
            MaterialCost = materialCost;
        }
        public Part Build(PartType type, int baseDirectDamageMod, double baseRelativeDamageMod, int baseDirectPercisionMod, double baseRelativePercisionMod, double baseJamMod, double baseBreakMod, double baseAccuracyMod)
        {
            PartType = type;
            DirectDamageModifier = CalculateValues(baseDirectDamageMod);
            RelativeDamageModifer = CalculateValues(baseRelativeDamageMod);
            DirectPrecisionModifer = CalculateValues(baseDirectPercisionMod);
            RelativePrecisionModifer = CalculateValues(baseRelativePercisionMod);
            JamModifer = CalculateValues(baseJamMod);
            BreakModifer = CalculateValues(baseBreakMod);
            AccuracyModifer = CalculateValues(baseAccuracyMod);
            return this;
        }*/
        
    }
}
