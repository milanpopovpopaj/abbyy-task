using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Abbyy_task.Data.Models
{
    [Table("Products")]
    public class Product
    {
        #region Constructor
        public Product()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The unique id and primary key for the Product
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}