using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpendingsApi.Models
{

    /// <summary>
    /// Obiekt Log  odpowiadający danym z bazy
    /// </summary>

    public class Log
    {
        [Key]
        public int id { get; set; }              //numer id Log
        public String Timestamp { get; set; }    //Data dodania Logu
        public String Level { get; set; }        //Level (poziom logu)
        public String Message { get; set; }      //Wiadomość w logu
        public String Exception { get; set; }    // 
        public String Properties { get; set; }   //
        public DateTime? _ts { get; set; }        //
    }
    
}
