using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarApi.Models
{
    public class Car
    {
        /// <summary>
        /// Obiekt Car odpowiadający danym z bazy
        /// </summary>
        /// 

        [Key]
        public int idCar { get; set; }              ///Numer kolejny auta - autoinkrementacja
        public string Manufacturer { get; set; }    ///Marka
        public string Model { get; set; }           ///Model
        public string Color { get; set; }           ///kolor
        public int YofProd { get; set; }            ///Rok produkcji
        public int Kilometers { get; set; }         ///Przebieg w km
        public double PriceDay { get; set; }        ///Cena za dzień wypożyczenia
        public int IsAvailable { get; set; }        ///czy dostępny - jeżeli wypożyczony - 0 
        public DateTime Insurance { get; set; }     ///Data ważności ubezpieczenia
        public int Segment { get; set; }            ///Segment aut (np. Small/Compact itp.)
        public string RegNumbers { get; set; }      ///numer rejestracyjny
        public string FilePath { get; set; }        ///ścieżka do dodatkowych danych np. zdjęcia itp.
        public DateTime TechRev { get; set; }       ///Data ważności przeglądu
    }
}
