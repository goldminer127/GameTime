using System;
using System.Collections.Generic;
using System.Text;

namespace GameTime.Models.Items
{
    public class AttachmentSlot
    {
        public string Name { get; private set; }
        public bool Available { get; set; } = true;
        public Attachment Attachment { get; private set; } = null;
        public AttachmentSlot(string name)
        {
            Name = name;
        }
        public void EquipSlot(Attachment attachment)
        {
            Attachment = attachment;
        }
        public bool IsEmpty()
        {
            return Attachment == null;
        }
    }
}
