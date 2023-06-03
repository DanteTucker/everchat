using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverChat
{
    public class FriendSorter : IComparer<ChatContact>
    {
        public int Compare(ChatContact? x, ChatContact? y)
        {

            if (x == null || y == null || x._data == null || y._data == null || x._contact == null || y._contact == null)
            {
                return 0;
            }
            int xUnread = Program._cloud.Messages.GetUserMessages(x._contact.ContactUserId).UnreadCount > 0 ? 1 : 0;
            int yUnread = Program._cloud.Messages.GetUserMessages(y._contact.ContactUserId).UnreadCount > 0 ? 1 : 0;
            int currentMessageComparison = xUnread.CompareTo(yUnread);

            if (currentMessageComparison == 0)
            {
                OnlineStatus xOnline = x._data.CurrentStatus.OnlineStatus ?? OnlineStatus.Offline;
                OnlineStatus yOnline = y._data.CurrentStatus.OnlineStatus ?? OnlineStatus.Offline;
                int onlineStatusComparison = xOnline.CompareTo(yOnline);
                if (onlineStatusComparison == 0)
                {
                    int lastMessageComparison = x._contact.LatestMessageTime.CompareTo(y._contact.LatestMessageTime);
                    if (lastMessageComparison == 0)
                    {
                        return x._data.CurrentStatus.LastStatusChange.CompareTo(y._data.CurrentStatus.LastStatusChange);
                    }
                    return lastMessageComparison;
                }
                return onlineStatusComparison;
            }
            return currentMessageComparison;
        }
    }

    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;

        protected override bool SupportsSortingCore => true;
        protected override bool IsSortedCore => _isSorted;
        protected override ListSortDirection SortDirectionCore => _sortDirection;
        protected override PropertyDescriptor SortPropertyCore => _sortProperty;
        public void SortBy(string propertyName, ListSortDirection direction)
        {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(T))[propertyName];
            ApplySortCore(propertyDescriptor, direction);
        }
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            _sortProperty = prop;
            _sortDirection = direction;

            List<ChatContact> itemsList = (List<ChatContact>)Items;
            /*if (prop.PropertyType.GetInterface("IComparable") != null)
            {
                itemsList.Sort(new Comparison<Contact>((x, y) => ((IComparable)prop.GetValue(x)).CompareTo(prop.GetValue(y))));
            }
            else
            {*/
            itemsList.Sort((x, y) => ((FriendSorter)new FriendSorter()).Compare(x, y));
            //}

            if (direction == ListSortDirection.Descending)
            {
                itemsList.Reverse();
            }

            _isSorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            _isSorted = false;
            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;
        }
    }
}