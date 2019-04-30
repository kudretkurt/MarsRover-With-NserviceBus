using System;
using System.ComponentModel.DataAnnotations;
using MarsRover.Shared;
using MarsRover.Shared.Enums;

namespace MarsRover.Persistence.EFCore.Entities
{
    public class Rover
    {
        public Guid Id { get; set; }
        public Direction Direction { get; set; }
        public Point Point { get; set; }
        public Plateau Plateau { get; set; }
        public Guid? PlateauId { get; set; }

        //PS:kudretkurt-->Eğer bu araç kendisine gelen yönergeler doğrultusunda görevini yerine getirirken ,aynı anda acil bir durum oluşur ve nasa ilgili acil durum mesajını gönderir ise ConcurrencyCheck attribute'ü ile diğer hareketlerin sonlanmadan kesilmesini sağlayacaktır.

        /// <summary>
        /// Acil durumlarda nasa bir aracın hareket etmesini engellemek istediğinde bu değeri true olarak setlemelidir.
        /// </summary>
        [ConcurrencyCheck]
        public bool IsLocked { get; set; }
    }
}
