using System;
using System.Collections.Generic;

namespace GameTime.Models
{
    public class Firearm : Item
    {
        public int BaseMaxDamage { get; private set; }
        public int BaseMinDamage { get; private set; }
        public int MaxDamage { get; private set; }
        public int MinDamage { get; private set; }
        public List<Part> Parts { get; private set; }
        public List<PartType> PartSlots { get; private set; }
        public AttachmentSlot[] AttachmentSlots { get; private set; }
        public double BaseJamProbability { get; private set; }
        public double JamProbability { get; private set; }
        public double BaseAccuracy { get; private set; }
        public double Accuracy { get; private set; }
        public string Ammo { get; private set; }
        public Firearm()
        {

        }
        public Firearm(int baseMinDamage, int baseMaxDamage, List<Part> parts, List<PartType> PartSlots, AttachmentSlot[] attachmentSlots, double baseJamProbability, double baseAccuracy, string ammo)
        {
            //Firearm properties
            BaseMinDamage = baseMinDamage;
            BaseMaxDamage = baseMaxDamage;
            Parts = parts;
            AttachmentSlots = attachmentSlots;
            BaseJamProbability = baseJamProbability;
            BaseAccuracy = baseAccuracy;
            Ammo = ammo;

            SetDamage();
            SetJam();
            SetAccuracy();
            CalculateValue();
            CalculateQuality();
        }
        public Firearm Build(int baseMinDamage, int baseMaxDamage, List<Part> parts, AttachmentSlot[] attachmentSlots, double baseJamProbability, double baseAccuracy, string ammo)
        {
            //Firearm properties
            BaseMinDamage = baseMinDamage;
            BaseMaxDamage = baseMaxDamage;
            Parts = parts;
            AttachmentSlots = attachmentSlots;
            BaseJamProbability = baseJamProbability;
            BaseAccuracy = baseAccuracy;
            Ammo = ammo;

            SetDamage();
            SetJam();
            SetAccuracy();
            CalculateValue();
            CalculateQuality();
            return this;
        }
        public void UpdateWeapon()
        {
            
        }
        //Relative modifers are calculated first, then direct modifers calculated next.
        private void SetDamage()
        {
            if (Parts == null)
            {
                MaxDamage = BaseMaxDamage;
                MinDamage = BaseMinDamage;
            }
            else
            {
                var maxRelativeModifier = 1.0;
                var minRelativeModifier = 1.0;
                var maxDirectRelativeModifier = 0;
                var minDirectRelativeModifier = 0;
                /*foreach (var part in Parts)
                {
                    maxRelativeModifier += part.RelativeDamageModifer - part.RelativePrecisionModifer;
                    minRelativeModifier += part.RelativeDamageModifer + part.RelativePrecisionModifer;
                    maxDirectRelativeModifier += part.DirectDamageModifier - part.DirectPrecisionModifer;
                    minDirectRelativeModifier += part.DirectDamageModifier + part.DirectPrecisionModifer;
                }*/
                MaxDamage = (int)Math.Round(BaseMaxDamage * maxRelativeModifier, MidpointRounding.ToPositiveInfinity);
                MinDamage = (int)Math.Round(BaseMinDamage * minRelativeModifier, MidpointRounding.ToPositiveInfinity);
            }
            
            if(AttachmentSlots != null)
            {
                foreach(var slot in AttachmentSlots)
                { 
                    if(!slot.IsEmpty())
                    {
                        var attachment = slot.Attachment;
                        MaxDamage = (int)Math.Round(MaxDamage * (attachment.RelativeDamageModifier - attachment.RelativePercisionModifier), MidpointRounding.ToPositiveInfinity) + (attachment.DirectDamageModifier - attachment.DirectPercisionModifier);
                        MinDamage = (int)Math.Round(MinDamage * (attachment.RelativeDamageModifier + attachment.RelativePercisionModifier), MidpointRounding.ToPositiveInfinity) + (attachment.DirectDamageModifier + attachment.DirectPercisionModifier);
                    }
                }
            }
        }
        private void SetJam()
        {
            if (Parts == null)
            {
                JamProbability = BaseJamProbability;
            }
            else
            {
                var modifier = 1.0;
                /*foreach (var part in Parts)
                {
                    modifier *= part.JamModifer;
                }*/
                JamProbability = Math.Round(BaseJamProbability * modifier, 3);
            }
        }
        private void SetAccuracy()
        {
            if (Parts == null)
            {
                Accuracy = BaseAccuracy;
            }
            else
            {
                var modifier = 1.0;
                /*foreach (var part in Parts)
                {
                    modifier *= part.AccuracyModifer;
                }*/
                Accuracy = Math.Round(BaseAccuracy * modifier, 3);
            }
        }
        private void CalculateValue()
        {
            Value = Quality switch
            {
                Quality.Bad => Value * 0.50m,
                Quality.Mediocre => Value * 0.80m,
                Quality.Standard => Value,
                Quality.Good => Value * 1.25m,
                Quality.Excellent => Value * 1.75m,
                Quality.Master => Value * 2.25m,
                _ => Value
            };
            foreach (var part in Parts)
            {
                Value = part.Quality switch
                {
                    Quality.Bad => Value * 0.50m + part.Value,
                    Quality.Mediocre => Value * 0.80m + part.Value,
                    Quality.Standard => Value + part.Value,
                    Quality.Good => Value * 1.25m + part.Value,
                    Quality.Excellent => Value * 1.75m + part.Value,
                    Quality.Master => Value * 2.25m + part.Value,
                    _ => Value + part.Value
                };
            }
            Value = Math.Round(Value, 2);
        }
        private void CalculateQuality()
        {
            var qualityPoints = 0;
            var partPoints = 0;
            foreach(var part in Parts)
            {
                qualityPoints += (int)part.PartType * (int)part.Quality;
                partPoints += (int)part.PartType;
            }
            Quality = (qualityPoints / partPoints) switch
            {
                1 => Quality.Bad,
                2 => Quality.Mediocre,
                3 => Quality.Standard,
                4 => Quality.Good,
                5 => Quality.Excellent,
                6 => Quality.Master,
                7 => Quality.BlackMark,
                _ => Quality.Bad,
            };
        }
        public string GetPartSlotsString()
        {
            var result = "";
            for (int i = 0; i < PartSlots.Count; i++)
            {
                if (i != PartSlots.Count - 1)
                    result += $"{PartSlots[i]}, ";
                else
                    result += PartSlots[i];
            }
            return result;
        }
        public string GetPartsString()
        {
            var result = "";
            for(int i = 0; i < Parts.Count; i++)
            {
                if (i != Parts.Count - 1)
                    result += $"{Parts[i].Name}, ";
                else
                    result += Parts[i].Name;
            }
            return result;
        }
        public bool Equals(Firearm item)
        {
            return Id == item.Id && Parts.Equals(item.Parts) && AttachmentSlots.Equals(item.AttachmentSlots) && Ammo == item.Ammo;
        }
    }
}
