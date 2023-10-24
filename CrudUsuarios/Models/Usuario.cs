using CrudUsuarios.Services;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CrudUsuarios.Models
{
    public class Usuario 
    {
        [Required]
        [MinLength(4)]
        public string Nome { get; set; }
        [Required]
        [MinLength(4)]
        public string Sobrenome { get; set; }
        [Required]
        [DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode = true)]
        [DateRange("01/01/1900", ErrorMessage = "Data inválida")]
        public DateTime? Nascimento { get; set; }
        [Required]
        [Key]
        public string Cpf { get; set; }
        [Required]
        public string Cep { get; set; }
        public Usuario()
        {
            
        }
        public Usuario(string nome, string sobrenome, DateTime? nascimento, string cpf, string cep)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Nascimento = nascimento;
            Cpf = cpf;
            Cep = cep;


        }
    }

    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute(string minimum) : base(typeof(DateTime), minimum, DateTime.Now.ToShortDateString())
        {
            
        }
    }

}
