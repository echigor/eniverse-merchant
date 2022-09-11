using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseGenerator.Model;

namespace EniverseGenerator.DataGenerators
{
    public class ProductGenerator
    {
        public const int TotalProducts = 49;

        private string[] _productNames = new string[TotalProducts]
        {
            "Водород", "Литий", "Углерод", "Азот", "Кислород",
            "Гелий - 3", "Кремний", "Титан", "Алюминий", "Железо",
            "Кобальт", "Никель", "Медь", "Молибден", "Цирконий",
            "Палладий", "Серебро", "Вольфрам", "Иридий", "Золото",
            "Ртуть", "Свинец", "Уран", "Торий", "Плутоний",
            "Соль", "Алмаз", "Александрит", "Гранат", "Рубин",
            "Пластмасса", "Композиты", "Полисмолы", "Сверхпрочные сплавы", "Высокотемпературные сплавы",
            "Универсальные запчасти", "Топливо", "Радиоэлектроника", "Микропроцессоры", "Микроэлектромеханические системы",
            "Растительные продукты", "Продукты животного происхождения", "Искусственные продукты", "Синтезированные пайки", "Вода",
            "Чистый воздух", "Бытовой мусор", "Органические отходы", "Токсичные отходы",
        };

        private int _productCount;

        public ProductGenerator()
        {
            _productCount = 0;
        }

        public Product GenerateNext()
        {
            Product result = new Product();
            result.Name = _productNames[_productCount];
            _productCount++;

            return result;
        }
    }
}
