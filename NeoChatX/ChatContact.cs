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

        //This god damn nonsense is because there is no way to get a single contacts status anymore.
        //The only way ContactData is exposed is through this ForeachContactData method, absolute shit.
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
