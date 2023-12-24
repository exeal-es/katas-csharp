﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Hole1
{
    public class TakeHomeCalculator
    {
        private readonly int percent;

        public TakeHomeCalculator(int percent)
        {
            this.percent = percent;
        }

        public Pair<int, String> NetAmount(Pair<int, String> first, params Pair<int, String>[] rest)
        {
            List<Pair<int, String>> pairs = rest.ToList();

            Pair<int, String> total = first;

            foreach (Pair<int, String> next in pairs)
            {
                if (next.second != total.second)
                {
                    throw new Incalculable();
                }
            }

            foreach (Pair<int, String> next in pairs)
            {
                total = new Pair<int, String>(total.first + next.first, next.second);
            }

            Double amount = total.first * (percent / 100d);
            Pair<int, String> tax = new Pair<int, String>(Convert.ToInt32(amount), first.second);

            if (total.second == tax.second)
            {
                return new Pair<int, String>(total.first - tax.first, first.second);
            }
            else
            {
                throw new Incalculable();
            }
        }
    }
}