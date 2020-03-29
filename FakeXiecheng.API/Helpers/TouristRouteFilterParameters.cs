using System;
using System.Text.RegularExpressions;

namespace FakeXiecheng.API.Helpers
{
    public enum OperatorType
    {
        lessThan,
        largerThan,
        equal,
    }

    public class TouristRouteFilterParameters
    {
        //private RatingFilter _rating = RatingFilter.largerThen;
        public string Keyword { get; set; }

        private OperatorType _ratingOperator;
        public OperatorType RatingOperator { get { return _ratingOperator; } }
        private int _ratingVlaue;
        public int RatingValue { get { return _ratingVlaue; } }

        /**
         * 接受 url rating 参数
         * 配置 ratingVlaue 和 ratingOperator
         */
        public string Rating
        {
            get { return ""; }
            set
            {
                Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
                Match match = regex.Match(value);
                if (match.Success)
                {
                    var ratingType = match.Groups[1].Value;
                    var ratingValue = match.Groups[2].Value;
                    try
                    {
                        _ratingVlaue = Int32.Parse(ratingValue);
                        if (ratingType.Equals("lessThan"))
                        {
                            _ratingOperator = OperatorType.lessThan;
                        }
                        else if (ratingType.Equals("largerThan"))
                        {
                            _ratingOperator = OperatorType.largerThan;
                        }
                        else
                        {
                            _ratingOperator = OperatorType.equal;
                        }
                    } catch
                    {
                        // 什么都不用做
                    }
                    
                }
            }
        }
    }
}
