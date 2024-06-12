using System;

namespace GameTime.Models.Items
{
    public class Attachment : Item
    {
        public AttachmentSlotType SlotType { get; private set; }
        public int DirectDamageModifier { get; private set; } = 0;
        public double RelativeDamageModifier { get; private set; } = 0.0;
        public int DirectPercisionModifier { get; private set; } = 0;
        public double RelativePercisionModifier { get; private set; } = 0.0;  //For very special attachments
        public double AccuracyModifier { get; private set; } = 0.0;
        public Attachment()
        {

        }
        public Attachment(AttachmentSlotType slotType, int directDamageModifier, double relativeDamageModifier, int directPercisionModifier, double relativePercisionModifier, double accuracyModifier)
        {
            SlotType = slotType;
            DirectDamageModifier = directDamageModifier;
            RelativeDamageModifier = relativeDamageModifier;
            DirectPercisionModifier = directPercisionModifier;
            RelativePercisionModifier = relativePercisionModifier;
            AccuracyModifier = accuracyModifier;
        }
        private int CalculateValues(int baseValue)
        {
            return Quality switch
            {
                Quality.Bad => (int)(baseValue * .50),
                Quality.Mediocre => (int)(baseValue * .80),
                Quality.Standard => baseValue,
                Quality.Good => (int)(baseValue * 1.25),
                Quality.Excellent => (int)(baseValue * 1.75),
                Quality.Master => (int)(baseValue * 2.25),
                _ => baseValue
            };
        }
        private double CalculateValues(double baseValue)
        {
            return Quality switch
            {
                Quality.Bad => Math.Round(baseValue * .50, 3),
                Quality.Mediocre => Math.Round(baseValue * .80, 3),
                Quality.Standard => Math.Round(baseValue, 3),
                Quality.Good => Math.Round(baseValue * 1.25, 3),
                Quality.Excellent => Math.Round(baseValue * 1.75, 3),
                Quality.Master => Math.Round(baseValue * 2.25, 3),
                _ => baseValue
            };
        }
    }
}
