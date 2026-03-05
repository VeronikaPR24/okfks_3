using CinemaTicketSystem;
using Xunit;

namespace TestProject1
{
    public class CinemaTicketSystemTests
    {

        private const int BasePrice = 300;
        private const int FreePrice = 0;
        private const int ChildDiscountPrice = 180;
        private const int StudentDiscountPrice = 240;
        private const int PensionerDiscountPrice = 150;
        private const int WednesdayDiscountPrice = 210;
        private const int MorningDiscountPrice = 255;
        private const int VIP = 2;

        //расчет базовой цены обычного взрослого билета = 300
        [Fact]
        public void CalculatePrice_ShouldReturnBasePrice_ForRegularAdultTicket()
        {
            const int age = 30;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(BasePrice, price);
        }

        //расчет билета младше 6 лет = 0
        [Fact]
        public void CalculatePrice_ShouldReturnZero_ForChildUnder6()
        {
            const int age = 5;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(FreePrice, price);
        }

        //расчет билета для студента 18-25 лет = 240
        [Fact]
        public void CalculatePrice_ShouldApplyStudentDiscount_ForStudentAged18To25()
        {
            const int age = 20;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(StudentDiscountPrice, price);
        }

        //расчет билета в среду = 210
        [Fact]
        public void CalculatePrice_ShouldApplyWednesdayDiscount_OnWednesdays()
        {
            const int age = 30;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Wednesday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(WednesdayDiscountPrice, price);
        }

        //расчет билета на утренний сеанс = 255
        [Fact]
        public void CalculatePrice_ShouldApplyMorningDiscount_ForSessionsBeforeNoon()
        {
            const int age = 30;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(11, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(MorningDiscountPrice, price);
        }

        //расчет VIP билета с наценкой = 600
        [Fact]
        public void CalculatePrice_ShouldDoublePrice_ForVIPTickets()
        {
            const int age = 30;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = true,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(BasePrice * VIP, price);
        }

        //проверка применения максимальной скидки для пенсионера = 150
        [Fact]
        public void CalculatePrice_ShouldApplyMaximumDiscount_WhenMultipleDiscountsApply()
        {
            const int age = 70;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Wednesday,
                SessionTime = new TimeSpan(11, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(PensionerDiscountPrice, price);
        }

        //расчет билета с максимальной скидкой для ребенка 6-17 лет = 180
        [Fact]
        public void CalculatePrice_ShouldApplyChildDiscount_ForAge6To17()
        {
            const int age = 10;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Wednesday,
                SessionTime = new TimeSpan(11, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(ChildDiscountPrice, price);
        }

        //проверка выбора максимальной скидки для студента не в среду = 240
        [Fact]
        public void CalculatePrice_ShouldApplyStudentOrWednesdayDiscount_WhicheverIsLarger()
        {
            const int age = 20;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(11, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(StudentDiscountPrice, price);
        }

        //проверка выбора максимальной скидки для студента в среду = 210
        [Fact]
        public void CalculatePrice_ShouldApplyStudentOnWednesdayDiscount_WhicheverIsLarger()
        {
            const int age = 20;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Wednesday,
                SessionTime = new TimeSpan(11, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(WednesdayDiscountPrice, price);
        }

        //проверка применения VIP наценки после применения детской скидки = 360
        [Fact]
        public void CalculatePrice_ShouldApplyVipAfterDiscount_ForPChildWithVip()
        {
            const int age = 8;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = true,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(ChildDiscountPrice * VIP, price);
        }

        //проверка выброса исключения при передаче null запроса
        [Fact]
        public void CalculatePrice_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            Assert.Throws<ArgumentNullException>(() => calc.CalculatePrice(null));
        }

        //проверка выброса исключения при отрицательном возрасте
        [Fact]
        public void CalculatePrice_ShouldThrowArgumentOutOfRangeException_ForNegativeAge()
        {
            const int age = -1;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };
            Assert.Throws<ArgumentOutOfRangeException>(() => calc.CalculatePrice(request));
        }

        //проверка выброса исключения при возрасте больше 120
        [Fact]
        public void CalculatePrice_ShouldThrowArgumentOutOfRangeException_ForAgeAbove120()
        {
            const int age = 121;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };
            Assert.Throws<ArgumentOutOfRangeException>(() => calc.CalculatePrice(request));
        }

        //граничное значение минимальный возраст = 0
        [Fact]
        public void CalculatePrice_ShouldReturnZero_ForNewborn()
        {
            const int age = 0;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(FreePrice, price);
        }

        //граничное значение возраст 5 лет = 0
        [Fact]
        public void CalculatePrice_ShouldReturnZero_ForAge5()
        {
            const int age = 5;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(FreePrice, price);
        }

        //граничное значение возраст 6 лет = 180
        [Fact]
        public void CalculatePrice_ShouldApplyChildDiscount_ForAge6()
        {
            const int age = 6;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(ChildDiscountPrice, price);
        }

        //граничное значение возраст 17 лет = 180
        [Fact]
        public void CalculatePrice_ShouldApplyChildDiscount_ForAge17()
        {
            const int age = 17;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(ChildDiscountPrice, price);
        }

        //граничное значение возраст 65 лет = 150
        [Fact]
        public void CalculatePrice_ShouldApplyPensionerDiscount_ForAge65()
        {
            const int age = 65;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = false,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(PensionerDiscountPrice, price);
        }

        //граничное значение возраст 18 лет = 240
        [Fact]
        public void CalculatePrice_ShouldApplyStudentDiscount_ForAge18()
        {
            const int age = 18;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(StudentDiscountPrice, price);
        }

        //проверка студенческой скидки для возраста 20 лет = 240
        [Fact]
        public void CalculatePrice_ShouldApplyStudentDiscount_ForAge20()
        {
            const int age = 20;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(StudentDiscountPrice, price);
        }

        //граничное значение возраст 25 лет = 240
        [Fact]
        public void CalculatePrice_ShouldApplyStudentDiscount_ForAge25()
        {
            const int age = 25;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(StudentDiscountPrice, price);
        }

        //граничное значение возраст 26 лет = 300
        [Fact]
        public void CalculatePrice_ShouldReturnBasePrice_ForStudentAge26()
        {
            const int age = 26;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(BasePrice, price);
        }

        //граничное значение возраст 64 года = 300
        [Fact]
        public void CalculatePrice_ShouldReturnBasePrice_ForStudentAge64()
        {
            const int age = 64;
            ITicketPriceCalculator calc = new TicketPriceCalculator();
            var request = new TicketRequest
            {
                Age = age,
                IsStudent = true,
                IsVip = false,
                Day = DayOfWeek.Monday,
                SessionTime = new TimeSpan(13, 0, 0)
            };

            var price = calc.CalculatePrice(request);
            Assert.Equal(BasePrice, price);
        }
    }
}