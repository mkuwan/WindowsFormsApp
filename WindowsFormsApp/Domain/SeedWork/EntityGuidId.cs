using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Domain.Events;

namespace WindowsFormsApp.Domain.SeedWork
{
    public abstract class EntityGuidId
    {
        private int? _requestedHashCode;
        private Guid _Id;

        public virtual Guid Id
        {
            get => _Id;
            protected set => _Id = value;
        }

        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvent => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public bool IsTransient()
        { 
            return this.Id == default || this.Id == Guid.Empty;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EntityGuidId)) return false;

            if(Object.ReferenceEquals(this, obj)) return true;

            if(this.GetType() != obj.GetType()) return false;

            EntityGuidId item = (EntityGuidId)obj;

            if (item.IsTransient() == this.IsTransient())
                return false;
            else
                return item.Id.ToString() == this.Id.ToString();
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            return base.GetHashCode();
        }

        public static bool operator ==(EntityGuidId left, EntityGuidId right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(EntityGuidId left, EntityGuidId right)
        {
            return !(left == right);
        }
    }
}
