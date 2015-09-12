using System;
using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;
using Global.Core.Helper;

namespace FFLTask.BLL.Entity
{
    public class Message : BaseEntity
    {
        public virtual string Content { get; set; }
        public virtual Task Task { get; set; }
        public virtual Project Project { get; set; }

        public virtual User Addresser { get; set; }
        public virtual User Addressee { get; set; }

        private DateTime? _readTime;
        public virtual DateTime? ReadTime { get { return _readTime; } }

        private bool _hideForAddresser;
        public virtual bool HideForAddresser { get { return _hideForAddresser; } }
        private bool _hideForAddressee;
        public virtual bool HideForAddressee { get { return _hideForAddressee; } }

        public virtual void Send()
        {
            if (Addressee != null)
            {
                Addressee.MessagesToMe = Addressee.MessagesToMe ?? new List<Message>();
                Addressee.MessagesToMe.Add(this);
            }

            if (Addresser != null)
            {
                Addresser.MessagesFromMe = Addresser.MessagesFromMe ?? new List<Message>();
                Addresser.MessagesFromMe.Add(this);
            }
        }

        public virtual void Read()
        {
            _readTime = SystemTime.Now();
        }

        public virtual void Hide(MessageFor hideMessageFor)
        {
            if (hideMessageFor == MessageFor.Addresser)
            {
                _hideForAddresser = true;
            }
            if (hideMessageFor == MessageFor.Addressee)
            {
                _hideForAddressee = true;
            }
        }
    }
}
