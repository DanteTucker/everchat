using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverChat
{
    public class ChatContact
    {
        public ContactData _data;
        public Contact _contact;

        public ChatContact(Contact a)
        {
            this._contact = a;
            Program._cloud.Contacts.ForeachContactData((ContactData a) =>
            {
                if (a.UserId == this._contact.ContactUserId)
                {
                    this._data = a;
                }
            });
        }

        public ChatContact()
        {
            _data = new ContactData(Program._cloud.Contacts, new Contact());
            _contact = new Contact();
        }
    }
}
