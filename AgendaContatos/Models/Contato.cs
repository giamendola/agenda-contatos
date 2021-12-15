using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AgendaContatos.Models
{
   
    public class Contato
    {       
        [SwaggerRequestBody(Required = false)]
        public int id { get; private set; }
        [Required]
        public string nome { get; set; }
        [Required]
        [Phone]
        public string telefone { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }   
}
