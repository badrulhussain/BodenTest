using System;

namespace App.Service
{
    public class CustomerService : ICustomerService
    {
        public bool AddCustomer(Customer customer)
        {
            if(!IsCustomerVaild(customer))
            {
                return false;
            }

            var companyRepository = new CompanyRepository();
            var company = companyRepository.GetById(customer.Company.Id);

            customer.Company = company;

            if (company.Name == "VeryImportantClient")
            {
                // Skip credit check
                customer.HasCreditLimit = false;
            }
            else if (company.Name == "ImportantClient")
            {
                // Do credit check and double credit limit
                customer.HasCreditLimit = true;
                using (var customerCreditService = new CustomerCreditServiceClient())
                {
                    var creditLimit = customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
                    creditLimit = creditLimit*2;
                    customer.CreditLimit = creditLimit;
                }
            }
            else
            {
                // Do credit check
                customer.HasCreditLimit = true;
                using (var customerCreditService = new CustomerCreditServiceClient())
                {
                    var creditLimit = customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
                    customer.CreditLimit = creditLimit;
                }
            }

            if (customer.HasCreditLimit && customer.CreditLimit < 500)
            {
                return false;
            }

            CustomerDataAccess.AddCustomer(customer);

            return true;
        }

        private bool IsCustomerVaild(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Firstname) || string.IsNullOrEmpty(customer.Surname))
            {
                return false;
            }

            if (!customer.EmailAddress.Contains("@") && !customer.EmailAddress.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - customer.DateOfBirth.Year;
            if (now.Month < customer.DateOfBirth.Month ||
                (now.Month == customer.DateOfBirth.Month && now.Day < customer.DateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }
    }
}
