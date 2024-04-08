using System.ComponentModel.DataAnnotations;

namespace LR10.Models
{
    public class Customer
    {
        [Required(ErrorMessage = "Ім'я та прізвище обов'язкові!")]
        public string? Initials { get; set; }

        [Required(ErrorMessage = "Для заповнення потрібна електронна адреса!")]
        [EmailAddress(ErrorMessage = "Недійсна електронна адреса!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Бажана дата консультації обов'язкова!")]
        [FutureDate(ErrorMessage = "Бажана дата консультації має бути в майбутньому!")]
        [NotWeekendDay(ErrorMessage = "Бажана дата консультації не може бути вихідним днем!")]
        public DateTime? ConsultationDate { get; set; }

        [Required(ErrorMessage = "Виберіть тему для консультації!")]
        [NotMonday(ErrorMessage = "Тема не може бути «Основи», якщо дата консультації - понеділок!")]
        public string? Product { get; set; }

        public override string ToString()
        {
            return $"Initials: {Initials}.\nEmail: {Email}.\nConsultation date: {ConsultationDate}.\nChosen product: {Product}";
        }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = (DateTime)value;
            return dt.Date > DateTime.Now.Date;
        }
    }

    public class NonWeekendAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = (DateTime)value;
            return dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday;
        }
    }

    public class NotMondayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            if (customer.ConsultationDate.HasValue && customer.ConsultationDate.Value.DayOfWeek == DayOfWeek.Monday)
            {
            
                if (value != null && value.ToString().Equals("Основи", StringComparison.OrdinalIgnoreCase))
                {
                
                    return new ValidationResult("Тема не може бути «Основи», якщо дата консультації - понеділок");
                    
                }
            }

            return ValidationResult.Success;
        }
    }
}
