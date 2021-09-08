using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace YahooFinanceApi1
{
    class Program
    {
        static void Main(string[] args)
        {
            Start:
            char continueStr = 'y';
            while (continueStr == 'y')
            {
                

                Console.WriteLine("Gib eine Aktie ein für die du Daten haben willst");
                string symbol = Console.ReadLine().ToUpper();
                Console.WriteLine("Enter the amount of months of historic data you want to retrieve: ");
                int timespan;
                try
                {
                    timespan = Convert.ToInt32(Console.ReadLine());
                    
                }
                catch (FormatException ex)
                {
                    Console.Clear();
                    goto Start;
                }
                DateTime endDate = DateTime.Today;
                DateTime startDate = DateTime.Today.AddMonths(-timespan);

                StockData stock = new StockData();
                var awaiter = stock.getStockData(symbol, startDate, endDate);
                if (awaiter.Result == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Möchtest du die Daten für einen anderen Ticker ? [y/n]");
                    continueStr = Convert.ToChar(Console.ReadLine().ToLower());
                }
            }
            Console.WriteLine();
            Console.WriteLine("Das Programm wurde beendet hab einen schönen Tag :) !");

        }




    }




    public class StockData
    {

        public async Task<int> getStockData(string symbol, DateTime startDate, DateTime endDate)
        {
            try
            {
                var historic_data = await Yahoo.GetHistoricalAsync(symbol, startDate, endDate);
                var security = await Yahoo.Symbols(symbol).Fields(Field.LongName).QueryAsync();
                var ticker = security[symbol];
                var companyName = ticker[Field.LongName];
                for (int i = 0; i < historic_data.Count; i++)
                {

                    Console.WriteLine($"{companyName} Closing price on: {historic_data.ElementAt(i).DateTime.Month} / {historic_data.ElementAt(i).DateTime.Day} / {historic_data.ElementAt(i).DateTime.Year} :Dollar {historic_data.ElementAt(i).Close} ");
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Failed to get symbol: " + symbol);
            }
            return 1;
        }

    }
}
