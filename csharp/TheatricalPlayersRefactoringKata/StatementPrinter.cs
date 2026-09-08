using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata;

public class StatementPrinter(Dictionary<string, Play> plays)
{
    private readonly Dictionary<string, Play> _plays = plays;
    
    public string Print(Invoice invoice)
    {
        var totalAmount = 0;
        var volumeCredits = 0;

        var result = $"Statement for {invoice.Customer}\n";
        CultureInfo cultureInfo = new("en-US");

        foreach(Performance perf in invoice.Performances) 
        {
            var play = _plays[perf.PlayID];
            var thisAmount = CalcAmount(perf);

            // add volume credits
            volumeCredits += Math.Max(perf.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) volumeCredits += perf.Audience / 5;;

            // print line for this order
            result +=$"  {play.Name}: {(thisAmount / 100m).ToString("C", cultureInfo)} ({perf.Audience} seats)\n";
            totalAmount += thisAmount;
        }
        result += $"Amount owed is {(totalAmount / 100m).ToString("C", cultureInfo)}\n";
        result += $"You earned {volumeCredits} credits\n";
        return result;
    }

    private int CalcAmount(Performance perf)
    {
        var play = _plays[perf.PlayID];
        var Result = 0;
        switch (play.Type)
        {
            case "tragedy":
                Result = 40000;
                if (perf.Audience > 30)
                {
                    Result += 1000 * (perf.Audience - 30);
                }
                break;
            case "comedy":
                Result = 30000;
                if (perf.Audience > 20)
                {
                    Result += 10000 + 500 * (perf.Audience - 20);
                }
                Result += 300 * perf.Audience;
                break;
            default:
                throw new Exception("unknown type: " + play.Type);
        }
        return Result;
    }
}
