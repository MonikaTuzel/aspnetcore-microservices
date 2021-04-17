using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpendingsApi.Models
{

    /// <summary>
    /// Obiekt Spendings odpowiadający danym z bazy
    /// </summary>

    public class Spendings
    {
        [Key]
        public int idSpendings { get; set; }  //numer id wydatków
        public DateTime Date { get; set; }    //Data
        public int CarID { get; set; }        //numer id auta
        public int CostID { get; set; }       //numer id kosztów
        public double Price { get; set; }     //Cena
        public string idUser { get; set; }     //Id użytkownika

    }
    [Table("AspNetUsers_has_DB_Car")]
    public class UserCars
    {
        public string AspNetUsers_Id { get; set; }
        public int DB_Car_idCar { get; set; }
    }
}
