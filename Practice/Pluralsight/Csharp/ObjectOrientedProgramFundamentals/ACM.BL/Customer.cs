using System;
using System.Collections.Generic;
using Acme.Common;

namespace ACM.BL
{
    public class Customer : EntityBase, ILoggable
    {
        public static int InstanceCount { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public int CustomerId { get; private set; }

        public List<Address> AddressList { get; set; }
        public int CustomerType { get; set; }

        public Customer()
            : this(0)
        {
        }

        public Customer(int customerId)
        {
            CustomerId = customerId;
            AddressList = new List<Address>();
        }

        public override bool Validate()
        {
            bool isValid = true;
            if (String.IsNullOrWhiteSpace(LastName)) isValid = false;
            if (string.IsNullOrWhiteSpace(EmailAddress)) isValid = false;

            return isValid;
        }

        public string FullName
        {
            get
            {
                string ret = String.Empty;
                if (!String.IsNullOrWhiteSpace(LastName))
                {
                    ret = LastName;

                    if (!String.IsNullOrWhiteSpace(FirstName))
                    {
                        ret += $", {FirstName}";
                    }
                }
                else
                {
                    ret = FirstName;
                }
                return ret;
            }
        }

        public string Log()
        {
            var logString = this.CustomerId + ": " +
                            this.FullName + " " +
                            "Email: " + this.EmailAddress + " " +
                            "Status: " + this.EntityState.ToString();
            return logString;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
